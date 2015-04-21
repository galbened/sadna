﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace User
{
    public class UserManager : IUserManager
    {
        List<Member> UsersList;
        int newestMemberID;
        List<String> userTypes;

        public UserManager()
        {
            this.UsersList = new List<Member>();
            this.userTypes = new List<string>();
            userTypes.Add("Normal");
            userTypes.Add("Gold");
            userTypes.Add("Silver");
            this.newestMemberID = 1;
        }


        public int login(string username, string password)
        {
            foreach (Member member in UsersList)
            {
                if ((member.getMemberUsername().CompareTo(username)==0))
                {
                    if (member.login(password) == true)
                        return member.getMemberID();

                }
                    
            }
            return -1;
        }

        public bool logout(int userID)
        {
            Member mem = getMemberByID(userID);
            if(mem.getLoggerStatus()==false)    //is user already logged out?
                return false; 
            mem.setLoggerStatus(false);         //logging the user out.
            return true;
        }

       public int register(string username, string password, string email)
        {
            if (!isNameAvilable(username))
                return -1;
            UsersList.Add(new Member(newestMemberID, username, password, email));
            newestMemberID++;
            return newestMemberID - 1;
        }

        public bool enterForum(string forumName)
        {
            throw new NotImplementedException();
        }

        public int changePassword(int userID, string oldPassword, string newPassword)
        {
            if (getMemberByID(userID).setPassword(oldPassword, newPassword))
                return userID;
            return -1;
           
        }

        public int changeUsername(int userID, string newUsername, string password)
        {
            if(getMemberByID(userID).getPassword().CompareTo(password)==0){ // checks if the password is correct
                if (isNameAvilable(newUsername))
                {
                    getMemberByID(userID).setUsername(newUsername);
                    return userID;
                }
            }
            return -1;
        }

        public int addFriend(int userID, int friendID)
        {
            Member mem = getMemberByID(userID);
            Member friend = getMemberByID(friendID);
            mem.addFriend(friend);
            return 1;
        }

        public string getUsername(int userID)
        {
            return getMemberByID(userID).getMemberUsername();
        }

        public string getPassword(int userID)
        {
            return getMemberByID(userID).getPassword();
        }

        public void removeFriend(int userID, int friendID)
        {
            Member mem = getMemberByID(userID);
            Member friend = getMemberByID(friendID);
            mem.removeFriend(friend);
        }

        public void approveRequest(int notificationID)//delayed for next buid
        {
            throw new NotImplementedException();
        }

        public void deactivate(int userID)
        {
            Member mem = getMemberByID(userID);
            UsersList.Remove(mem);
        }

        public bool getConfirmationCodeFromUser(int userID, int code)
        {
            Member mem = getMemberByID(userID);
            return mem.checkConfirmationCodeFromUser(code);
        }

        //Helper methods

        /*
         * returns Member instance that matches the given userID, throws exception if ID not found.
         */
        private Member getMemberByID(int userID)
        {
            foreach (Member member in UsersList)
            {
                if (member.getMemberID() == userID)
                    return member; 
            }
            throw new System.InvalidOperationException("userID not found.");
        }

        private Boolean isNameAvilable(String userName){
            foreach (Member member in UsersList)
            {
                if (member.getMemberUsername().CompareTo(userName)==0)
                    return false;
            }
            return true;

        }
    }
}
