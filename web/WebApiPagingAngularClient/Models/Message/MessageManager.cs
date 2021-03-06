﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Message
{
    public class MessageManager : IMessageManager
    {
        private HashSet<Message> messages;
        private static MessageManager instance = null;
        private int lastMessageID;
        private const string error_emptyTitle = "Cannot add thread without title";
        private const string error_messageIdNotFound = "Message ID doesn't exist";


        public static IMessageManager Instance()
        {
            if (instance == null)
                return new MessageManager();
            return instance;
        }

        private MessageManager()
        {
            messages = new HashSet<Message>();
            lastMessageID = -1;
        }


        public int addThread(int forumId, int subForumId, int publisherId, string title, string body)
        {
            if ((title == null) || (title.Equals("")))
                throw new ArgumentException(error_emptyTitle);
            lastMessageID++;
            int messageId = lastMessageID;
            Thread thread = new Thread(forumId, subForumId, messageId, publisherId, title, body);
            Message ms = thread.getMessage();
            messages.Add(ms);
            return ms.getMessageID();
        }


        public int addComment(int firstMessageID, int publisherID, string title, string body)
        {
            if ((title == null) || (title.Equals("")))
                throw new ArgumentException(error_emptyTitle);
            FirstMessage first = (FirstMessage)findMessage(firstMessageID);
            //checking if firstMessageID exists and really FirstMessage
            if ((first != null) && (first.isFirst()))
            {
                lastMessageID++;
                int messageId = lastMessageID;
                ResponseMessage rm = new ResponseMessage((FirstMessage)first, messageId, publisherID, title, body);
                first.addResponse(rm);
                messages.Add(rm);
                return rm.getMessageID();
            }
            throw new InvalidOperationException(error_messageIdNotFound);
        }


        public bool editMessage(int messageId, string title, string body)
        {
            if ((title == null) || (title.Equals("")))
                throw new ArgumentException(error_emptyTitle);
            Message ms = findMessage(messageId);
            if (ms != null)
            {
                ms.editMessage(title, body);
                return true;
            }
            throw new InvalidOperationException(error_messageIdNotFound);
        }



        public bool deleteMessage(int messageID)
        {
            Message ms = findMessage(messageID);
            if (ms != null)
            {
                // if firstMessage, it should be deleted with all its comments
                if (ms.isFirst())
                {
                    HashSet<ResponseMessage> messagesForDeletion = ((FirstMessage)ms).getResponseMessages();
                    foreach (ResponseMessage rm in messagesForDeletion)
                        messages.Remove(rm);
                }
                //remove the message anyway
                 messages.Remove(ms);
                 return true;
            }
            throw new InvalidOperationException(error_messageIdNotFound);
                

        }

        private Message findMessage(int messageID)
        {
            bool found = false;
            int messagesSearched = 0;
            while ((!found) && (messagesSearched < messages.Count))
            {
                Message cur = messages.ElementAt<Message>(messagesSearched);
                if (cur.getMessageID() == messageID)
                {
                    found = true;
                    return cur;
                }
                messagesSearched++;
            }

            //if messageID is wrong
            return null;
        }

        

        

    }
}
