﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;
using Forum;
using Message;

namespace DB
{
    public class DBmanager
    {
        Context db;

        public DBmanager()
        {
            db = new Context();
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE Members");
        }

        public void updateDB(List<Member> MembersNew)
        {
            var Members = db.Members;
            List<Member> ans = new List<Member>();

            foreach (Member mem in MembersNew)
            {
                db.Members.Add(mem);
            }

            db.SaveChanges();
        }

        public List<Member> getMembersFromDb()
        {
            var Members = db.Members;
            List<Member> ans = new List<Member>();

            foreach (Member mem in Members ?? null)
            {
                ans.Add(mem);
            }
            return ans;
        }
    }
}
