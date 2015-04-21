﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Forum
{
    public class Forum
    {
        private List<int> registeredUsersID,logedUsersId, adminsID;
        private string forumName;
        private int forumID;
        private List<SubForum> subForums;
        private Policy poli;
        private static int subForumIdCounter;
        private UserManager usrMngr;

        public Forum(string name, int id)
        {
            forumName = name;
            forumID = id;
            registeredUsersID = new List<int>();
            adminsID = new List<int>();
            logedUsersId = new List<int>();
            subForums = new List<SubForum>();
            poli = new Policy();
            usrMngr = new UserManager();
            subForumIdCounter = 100;
        }

        public int createSubForum(string topic, int callerUserId)
        {
            foreach (SubForum sbfrm in subForums)
                if (sbfrm.getTopic().CompareTo(topic) == 0)
                    return -1;
            if (adminsID.Contains(callerUserId))
            {
                subForums.Add(new SubForum(topic, subForumIdCounter));
                subForumIdCounter++;
                return subForumIdCounter - 1;
            }
            return -2;
        }

        public Policy getPolicy()
        {
            return poli;
        }

        public void addAdmin(int userId)
        {
            if (registeredUsersID.Contains(userId))
                adminsID.Add(userId);
        }

        public void removeAdmin(int userId)
        {
            adminsID.Remove(userId);
        }

        public void showSubForums()
        {
            foreach (SubForum sf in subForums)
            {
                Console.Write(sf.ToString());
            }
        }

        public Boolean isAdmin(int userId)
        {
            return adminsID.Contains(userId);
        }
        
        public int register(string username, string password, string mail)
        {
            if (!(poli.isValid(password)))
                return -1;

            int id = usrMngr.register(username, password, mail);
            if (id < 0)
                return -2;
            if (!(registeredUsersID.Contains(id)))
                registeredUsersID.Add(id);
            return id;
        }

        public int login(string username, string password)
        {
            int id = usrMngr.login(username, password);
            if (registeredUsersID.Contains(id))
                if (!(logedUsersId.Contains(id)))
                    logedUsersId.Add(id);
                else
                    return -1;
            return id;
        }

        public Boolean logout(int userId)
        {
            if (!(logedUsersId.Contains(userId)))
                return false;
            usrMngr.logout(userId);
            logedUsersId.Remove(userId);
            return true;
        }

        public void setPolicy(int numOfModerators, string degreeOfEnsuring,
                       Boolean uppercase, Boolean lowercase, Boolean numbers,
                       Boolean symbols, int minLength)
        {
            poli.setNumOfModerators(numOfModerators);
            poli.setdefreeOfEnsuring(degreeOfEnsuring);
            poli.setUpper(uppercase);
            poli.setLower(lowercase);
            poli.setNumbers(numbers);
            poli.setSymbols(symbols);
            poli.setMinLength(minLength);
        }

        public int getId()
        {
            return forumID;
        }

        internal Boolean isModerator(int userId, int subForumId)
        {
            foreach (SubForum sbfrm in subForums)
                if (sbfrm.getId() == subForumId)
                    return sbfrm.isModerator(userId);
            return false;
        }

        internal void addModerator(int userId, int subForumId)
        {
            if (registeredUsersID.Contains(userId))
                foreach (SubForum sbfrm in subForums)
                    if (sbfrm.getId() == subForumId)
                        sbfrm.addModerator(userId);
        }

        internal void removeModerator(int userId, int subForumId)
        {
            foreach (SubForum sbfrm in subForums)
                if (sbfrm.getId() == subForumId)
                    sbfrm.removeModerator(userId);
        }

        internal void setSubTopic(string topic, int subForumId)
        {
            foreach (SubForum sbfrm in subForums)
                if (sbfrm.getId() == subForumId)
                    sbfrm.setTopic(topic);
        }

        internal int compareName(string name)
        {
            return this.forumName.CompareTo(name);
        }

        internal void unRegister(int userId)
        {
            logedUsersId.Remove(userId);
            registeredUsersID.Remove(userId);
        }

        internal int getSubForumId(string topic)
        {
            foreach (SubForum sbfrm in subForums)
                if (sbfrm.getTopic().CompareTo(topic) == 0)
                    return sbfrm.getId();
            return -1;
        }

        internal void removeSubForum(int subForumId)
        {
            SubForum tmp = null;
            foreach (SubForum sbfrm in subForums)
                if (sbfrm.getId() == subForumId)
                    tmp = sbfrm;
            subForums.Remove(tmp);
        }

        internal bool isValidPassword(string password)
        {
            return poli.isValid(password);
        }
    }
}
