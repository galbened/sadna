﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class Forum
    {
        private List<int> registeredUsersID,logedUsersId, adminsID;
        private string forumName;
        private int forumID;
        private List<SubForum> subForums;
        private Policy poli;
        public static int subForumIdCounter;

        public Forum(string name, int id)
        {
            forumName = name;
            forumID = id;
            registeredUsersID = new List<int>();
            adminsID = new List<int>();
            logedUsersId = new List<int>();
            subForums = new List<SubForum>();
            poli = new Policy();
            subForumIdCounter = 100;
        }

        public void createSubForum(string topic)
        {
            subForums.Add(new SubForum(topic, subForumIdCounter));
            subForumIdCounter++;
        }

        public Policy getPolicy()
        {
            return poli;
        }

        public void addAdmin(int userId)
        {
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

        public void register(string username, string password)
        {
            int id = User.getUserId(username, password);
            if (!(registeredUsersID.Contains(id)))
                registeredUsersID.Add(id);
        }

        public void login(string username, string password)
        {
            int id = User.getUserId(username, password);
            if (registeredUsersID.Contains(id))
                if (!(logedUsersId.Contains(id)))
                    logedUsersId.Add(id);
        }

        public void logout(int userId)
        {
            logedUsersId.Remove(userId);
        }

        public void setPolicy(int numOfModerators, string degreeOfEnsuring)
        {
            poli.setNumOfModerators(numOfModerators);
            poli.setdefreeOfEnsuring(degreeOfEnsuring);
        }

        public int getId()
        {
            return forumID;
        }

        internal bool isModerator(int userId, int subForumId)
        {
            foreach (SubForum sbfrm in subForums)
                if (sbfrm.getId() == subForumId)
                    return sbfrm.isModerator(userId);
            return false;
        }

        internal void addModerator(int userId, int subForumId)
        {
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
    }
}
