﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using ForumLoggers;
using Message;
using Notification;


namespace Forum
{
    public class ForumManager : IForumManager
    {
        //private List<Forum> forums;
        private static ForumManager instance = null;
        private static int forumIdCounter;
        private IMessageManager MM;
        private const string error_emptyTitle = "Cannot create forum without title";
        private const string error_emptyTitleSub = "Cannot create subForum without topic";
        private const string error_existTitle = "Cannot create forum with already exit title";
        private const string error_forumID = "No such forum: ";
        private const string error_accessDenied = "You have no permissions to perform this operation";
        private const string error_noSuchUser = "Requested user not exist";

        IDBManager<Forum> DBforumMan;

        private ForumManager()
        {
            //forums = new List<Forum>();
            forumIdCounter = 1000;
            MM = MessageManager.Instance();

            DBforumMan = new DBforumManager();
            /* 
            DBforumMan.add(new Forum("Sports3", 1));
            DBforumMan.add(new Forum("News3", 2));
            DBforumMan.add(new Forum("Science3", 3));

            var obj = DBforumMan.getObj(4);
            obj.CreateSubForum("Politic");
            obj.CreateSubForum("Economy");
            obj.CreateSubForum("Weather");

            var obj2 = DBforumMan.getObj(1);
            obj2.CreateSubForum("Soccer");
            obj2.CreateSubForum("Basketball");
            obj2.CreateSubForum("Weather");

            DBforumMan.update();
             */
        }

        public static ForumManager getInstance()
        {
            if (instance == null)
                return new ForumManager();
            return instance;
        }


        public int CreateForum(int userRequesterId, string name)
        {
            if (userRequesterId != 1)
                throw new UnauthorizedAccessException(error_accessDenied);
            if ((name == null) || (name == ""))
                throw new ArgumentException(error_emptyTitle);
            foreach (Forum frm in DBforumMan.getAll())
                if (frm.CompareName(name) == 0)
                {
                    throw new ArgumentException(error_existTitle);
                }
            Forum forum = new Forum(name, forumIdCounter);
            DBforumMan.add(forum);
            forumIdCounter++;
            DBforumMan.update();
            return forum.forumID;
        }

        public int CreateSubForum(int userRequesterId, string topic, int forumId)
        {
            if ((topic == null) || (topic == ""))
                throw new ArgumentException(error_emptyTitleSub);

            int ans = -1;

            Forum forum = DBforumMan.getObj(forumId);
            //foreach (Forum frm in forums)
            //{
            //    if (frm.ForumID == forumId)
            //        ans = frm.CreateSubForum(topic);
            //}
            ans = forum.CreateSubForum(userRequesterId, topic);
            DBforumMan.update();
            return ans;
        }
        public void RemoveForum(int userRequesterId, int forumId)
        {
            if (userRequesterId != 1)
                throw new UnauthorizedAccessException(error_accessDenied);
            Forum tmp = null;
            tmp = DBforumMan.getObj(forumId);
            //foreach (Forum frm in forums)
            //    if (frm.ForumID == forumId)
            //        tmp = frm;
            DBforumMan.remove(tmp);
            DBforumMan.update();

        }
        public Boolean RemoveSubForum(int userRequesterId, int forumId, int subForumId)
        {

            foreach (Forum frm in DBforumMan.getAll())
                if (frm.ForumID == forumId)
                    return frm.RemoveSubForum(userRequesterId, subForumId);
            DBforumMan.update();
            return false;
        }
        public void AddAdmin(int userRequesterId, int userId, int forumId)
        {

            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                    frm.AddAdmin(userRequesterId, userId);
            }
            DBforumMan.update();
        }
        public void RemoveAdmin(int userRequesterId, int userId, int forumId)
        {

            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                    frm.RemoveAdmin(userRequesterId, userId);
            }
            DBforumMan.update();
        }
        public Boolean IsAdmin(int userId, int forumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                    return frm.IsAdmin(userId);
            }
            return false;
        }
        public int Register(string username, string password, string mail, int forumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                {
                    int id = frm.Register(username, password, mail);
                    DBforumMan.update();
                    return id;
                }
            }
            throw new ArgumentException(error_forumID + forumId);
        }
        public void UnRegister(int userId, int forumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                {
                    frm.UnRegister(userId);
                    DBforumMan.update();
                    return;
                }
            }
            throw new ArgumentException(error_forumID + forumId);
        }
        public int Login(string username, string password, int forumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                {
                    int id = frm.Login(username, password);
                    DBforumMan.update();
                    return id;
                }
            }
            throw new ArgumentException(error_forumID + forumId);

        }
        public Boolean Logout(int userId, int forumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
                if (frm.ForumID == forumId)
                {
                    bool id = frm.Logout(userId);
                    DBforumMan.update();
                    return id;
                }
            throw new ArgumentException(error_forumID + forumId);
        }
        public void SetPolicy(int userRequesterId, int numOfModerators, string degreeOfEnsuring,
                       Boolean uppercase, Boolean lowercase, Boolean numbers,
                       Boolean symbols, int minLength, int forumId)
        {

            foreach (Forum frm in DBforumMan.getAll())
                if (frm.ForumID == forumId)
                {
                    frm.SetPolicy(userRequesterId, numOfModerators, degreeOfEnsuring, uppercase, lowercase
                        , numbers, symbols, minLength);
                    DBforumMan.update();
                    break;
                }
        }
        public Boolean IsModerator(int userId, int forumId, int subForumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                    return frm.IsModerator(userId, subForumId);
            }
            throw new ArgumentException(error_forumID + forumId); ;
        }
        public void AddModerator(int userRequesterId, int forumId, int subForumId, int moderatorId)
        {

            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                    frm.AddModerator(userRequesterId, subForumId, moderatorId);
            }
            DBforumMan.update();
        }

        public void RemoveModerator(int userRequesterId, int userId, int forumId, int subForumId)
        {

            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                    frm.RemoveModerator(userRequesterId, userId, subForumId);
            }
            DBforumMan.update();
        }
        public void SetTopic(string topic, int forumId, int subForumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.ForumID == forumId)
                    frm.SetSubTopic(topic, subForumId);
            }
            DBforumMan.update();
        }
        public int GetForumId(string name)
        {
            foreach (Forum frm in DBforumMan.getAll())
            {
                if (frm.CompareName(name) == 0)
                    return frm.ForumID;
            }
            throw new ArgumentException(error_forumID + name);
        }

        public int GetSubForumId(int forumId, string topic)
        {
            foreach (Forum frm in DBforumMan.getAll())
                if (frm.ForumID == forumId)
                    return frm.GetSubForumId(topic);
            throw new ArgumentException(error_forumID + forumId);
        }

        public Boolean IsValid(string password, int forumId)
        {
            foreach (Forum frm in DBforumMan.getAll())
                if (frm.ForumID == forumId)
                    return frm.IsValidPassword(password);
            return false;
        }

        public int NumOfForums()
        {
            return DBforumMan.getAll().Count;
        }

        public List<int> GetForumIds()
        {
            List<int> ans = new List<int>();
            foreach (Forum fr in DBforumMan.getAll())
                ans.Add(fr.ForumID);
            return ans;

        }

        public List<string> GetForumTopics()
        {
            List<string> ans = new List<string>();
            foreach (Forum fr in DBforumMan.getAll())
                ans.Add(fr.forumName);
            return ans;
        }

        public string GetForumName(int forumId)
        {
            int forumIndex = GetForumIndex(forumId);
            return DBforumMan.getAll().ElementAt(forumIndex).forumName;
        }

        public List<int> GetSubForumsIds(int forumId)
        {
            List<int> ans = new List<int>();
            int forumIndex = GetForumIndex(forumId);
            Forum cur = DBforumMan.getAll().ElementAt(forumIndex);
            List<SubForum> subForums = cur.subForums;
            foreach (SubForum sf in subForums)
            {
                ans.Add(sf.SubForumId);
            }
            return ans;

        }

        public List<string> GetSubForumsTopics(int forumId)
        {
            List<string> ans = new List<string>();
            int forumIndex = GetForumIndex(forumId);
            Forum cur = DBforumMan.getAll().ElementAt(forumIndex);
            List<SubForum> subForums = cur.subForums;
            foreach (SubForum sf in subForums)
            {
                ans.Add(sf.Topic);
            }
            return ans;
        }


        public Boolean isRegisteredUser(int forumId, int userId)
        {
            List<int> ans = new List<int>();
            int forumIndex = GetForumIndex(forumId);
            Forum cur = DBforumMan.getAll().ElementAt(forumIndex);
            return cur.isUserRegistered(userId);
        }

        public Boolean isLoggedUser(int forumId, int userId)
        {
            List<int> ans = new List<int>();
            int forumIndex = GetForumIndex(forumId);
            Forum cur = DBforumMan.getAll().ElementAt(forumIndex);
            return cur.isUserLogged(userId);
        }

        public string GetSubForumTopic(int forumId, int subForumId)
        {
            string ans = null;
            int forumIndex = GetForumIndex(forumId);
            Forum fr = DBforumMan.getAll().ElementAt(forumIndex);
            ans = fr.GetSubForumTopic(forumId, subForumId);
            return ans;
        }
        public List<int> GetAllComments(int forumId, int subForumId, int firstMessageId)
        {
            List<int> ans = new List<int>();
            int forumIndex = GetForumIndex(forumId);
            Forum cur = DBforumMan.getAll().ElementAt(forumIndex);

            return ans;
        }

        public string GetUserType(int forumId, int userId)
        {
            if (IsAdmin(userId, forumId))
                return "admin";
            if (isRegisteredUser(forumId, userId))
                return "member";
            return "";
        }

        public string GetUsername(int forumId, int userId)
        {
            Forum fr = GetForum(forumId);
            string ans = fr.GetUserName(userId);
            return ans;
        }

        public List<string> GetSessionHistory(int requesterId, int forumId, int userIdSession)
        {
            if (IsAdmin(requesterId, forumId))
            {
                Forum fr = GetForum(forumId);
                List<string> ans = fr.GetSessionHistory(userIdSession);
                return ans;
            }
            else
                throw new UnauthorizedAccessException(error_accessDenied);

        }

        public void ComplainModerator(int userRequesterId, int moderator, int forumId, int subForumId)
        {

            Forum fr = GetForum(forumId);
            SubForum sf = fr.GetSubForum(subForumId);
            List<int> moderators = sf.GetModeratorsIds();
            if (!moderators.Contains(moderator))
                throw new ArgumentException(error_noSuchUser + ": " + moderator);
            string complainer = GetUsername(forumId, userRequesterId);
            List<int> adminsIds = GetForumAdmins(forumId);          
            string complainOnUser = GetUsername(forumId, moderator);

            foreach(int admin in adminsIds)
            {
                string complainToUser = GetUsername(forumId, admin);
                string complainToMail = GetUserMail(forumId, admin);
                Notification.Notification.SendComplaintNotification(complainer, complainToUser, complainOnUser, complainToMail);
            }
            
        }

        public List<int> GetModeratorIds(int forumId, int subForumId)
        {
            Forum fr = GetForum(forumId);
            SubForum sf = fr.GetSubForum(subForumId);
            List<int> moderatorsIds = sf.GetModeratorsIds();
            return moderatorsIds;
        }

        public List<string> GetModeratorNames(int forumId, int subForumId)
        {
            List<int> moderatorsIds = GetModeratorIds(forumId, subForumId);
            List<string> ans = new List<string>();
            foreach (int md in moderatorsIds)
            {
                string moderatorName = GetUsername(forumId, md);
                ans.Add(moderatorName);
            }
            return ans;
        }


        public List<int> GetMembersNoAdminIds(int forumId)
        {
            Forum fr = GetForum(forumId);
            List<int> ans = fr.GetMembersNoAdminIds();
            return ans;
        }

        public List<string> GetMembersNoAdminNames(int forumId)
        {
            List<int> ids = GetMembersNoAdminIds(forumId);
            List<string> ans = new List<string>();
            foreach (int id in ids)
            {
                ans.Add(GetUsername(forumId, id));
            }
            return ans;
        }


        public List<int> GetMembersNoModeratorIds(int forumId, int subForumId)
        {
            List<int> ans = new List<int>();
            Forum fr = GetForum(forumId);
            ans = fr.GetMembersNoModeratorIds(subForumId);
            return ans;          
        }


        public List<string> GetMembersNoModeratorNames(int forumId, int subForumId)
        {
            List<string> ans = new List<string>();
            List<int> membersNoModeratorIds = GetMembersNoModeratorIds(forumId, subForumId);
            foreach (int id in membersNoModeratorIds)
            {
                ans.Add(GetUsername(forumId, id));
            }
            return ans;
        }


        public void SendFriendRequest(int requesterId, int friendId, int forumId)
        {
            string userNameFrom = GetUsername(forumId, requesterId);
            string userNameTo = GetUsername(forumId, friendId);
            string emailTo = GetUserMail(forumId, friendId);
            Notification.Notification.SendFriendRequestNotification(userNameFrom, userNameTo, emailTo);
        }

        



        private string GetUserMail(int forumId, int userId)
        {
            Forum fr = GetForum(forumId);
            return fr.GetUserMail(userId);
        }

        private List<int> GetForumAdmins(int forumId)
        {
            Forum fr = GetForum(forumId);
            return fr.GetForumAdmins();
        }


        private Forum GetForum(int forumId)
        {
            for (int i = 0; i < DBforumMan.getAll().Count; i++)
            {
                if (DBforumMan.getAll().ElementAt(i).ForumID == forumId)
                    return DBforumMan.getAll().ElementAt(i);
            }
            throw new ArgumentException(error_forumID + forumId);
        }

        private int GetForumIndex(int forumId)
        {
            for (int i = 0; i < DBforumMan.getAll().Count; i++)
            {
                if (DBforumMan.getAll().ElementAt(i).ForumID == forumId)
                    return i;
            }
            throw new ArgumentException(error_forumID + forumId);
        }

        private bool CheckExisting(List<int> list, int find)
        {
            foreach (int obj in list)
            {
                if (obj == find)
                    return true;
            }
            return false;
        }


    }
}