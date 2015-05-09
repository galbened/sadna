﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interfaces;
using Message;
using Forum;

namespace Message.UnitTests
{
    [TestClass]
    public class MessageUnitTests
    {
        String[] titels = { "sport", "nature" };
        String[] subTitels = { "football", "basketball", "animals", "plants" };
        String[] topic = { "man u", "juve" };
        String[] body = { "best team in the world" };
        String[] userNames = { "tomer.b", "tomer.s", "gal.b", "gal.p", "osher" };
        String[] emails = { "tomer.b@gmail.com", "tomer.s@gmail.com", "gal.b@gmail.com", "gal.p@gmail.com", "osher@gmail.com" };
        String[] passwords = { "123456", "abcdef" };
        IMessageManager mm = MessageManager.Instance();
        IForumManager fm = ForumManager.getInstance();

        /*testing add thread
         * should succeed when title not empty
         */
        [TestMethod]
        public void addThreadTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId1 = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int threadId2 = mm.addThread(forumId, subForumId, userId, topic[1], body[0]);
            Assert.AreNotEqual(threadId1, threadId2);
            fm.RemoveForum(forumId);
        }

        [TestMethod]
        public void addThreadTitleEmptyTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId1 = mm.addThread(forumId, subForumId, userId, "", body[0]);
            Assert.AreEqual(threadId1, -1);
            fm.RemoveForum(forumId);
        }

        /*testing add comment
         * should succeed when title not empty
         */
        [TestMethod]
        public void addCommentTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int commentID1 = mm.addComment(threadId, userId, topic[1], body[0]);
            int commentID2 = mm.addComment(threadId, userId, topic[1], body[0]);
            Assert.AreNotEqual(commentID1, commentID2);
            fm.RemoveForum(forumId);
        }

        [TestMethod]
        public void addCommentTitleEmptyTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int commentID1 = mm.addComment(threadId, userId, "", body[0]);
            Assert.AreEqual(commentID1, -1);
            fm.RemoveForum(forumId);
        }

        [TestMethod]
        public void addCommentThreadNotExistsTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int commentID1 = mm.addComment(5, userId, topic[1], body[0]);
            Assert.AreEqual(commentID1, -2);
            fm.RemoveForum(forumId);
        }

        /*testing edit message
         * should succeed when title not empty and message ID exists
         */
        [TestMethod]
        public void editMessageTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int commentID1 = mm.addComment(threadId, userId, topic[1], body[0]);
            Assert.IsTrue(mm.editMessage(commentID1, topic[0], body[0]));
            fm.RemoveForum(forumId);
        }

        [TestMethod]
        public void editMessageTitleEmptyTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int commentID1 = mm.addComment(threadId, userId, topic[1], body[0]);
            Assert.IsFalse(mm.editMessage(commentID1, "", body[0]));
            fm.RemoveForum(forumId);
        }

        [TestMethod]
        public void editCommentMessageNotExistsTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int commentID1 = mm.addComment(threadId, userId, topic[1], body[0]);
            Assert.IsFalse(mm.editMessage(-200, topic[0], body[0]));
            fm.RemoveForum(forumId);
        }

        /*testing delete message
         * should succeed when message ID exists
         */
        [TestMethod]
        public void deleteMessageTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int commentID1 = mm.addComment(threadId, userId, topic[1], body[0]);
            Assert.IsTrue(mm.deleteMessage(commentID1));
            fm.RemoveForum(forumId);
        }

        [TestMethod]
        public void deleteCommentMessageNotExistsTest()
        {
            int forumId = fm.CreateForum(titels[0]);
            int userId = fm.Register(userNames[0], passwords[0], emails[0], forumId);
            fm.AddAdmin(userId, forumId);
            int subForumId = fm.CreateSubForum(subTitels[0], forumId, userId);
            int threadId = mm.addThread(forumId, subForumId, userId, topic[0], body[0]);
            int commentID1 = mm.addComment(threadId, userId, topic[1], body[0]);
            Assert.IsFalse(mm.deleteMessage(-200));
            fm.RemoveForum(forumId);
        }
    }
}