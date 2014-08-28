﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using EntityFramework.Extensions;
using Xunit;
using Tracker.SqlServer.Entities;

namespace Tracker.SqlServer.Test
{
    
    public class ExtensionTest
    {
        [Fact]
        public void BeginTransactionObjectContext()
        {
            using (var db = new TrackerEntities())
            using (var tx = db.BeginTransaction())
            {
                string emailDomain = "@test.com";

                int count = db.Users.Update(
                    u => u.EmailAddress.EndsWith(emailDomain),
                    u => new User { IsApproved = false, LastActivityDate = DateTime.Now });

                count = db.Users.Delete(u => u.EmailAddress.EndsWith(emailDomain));

                tx.Commit();
            }
        }

        [Fact]
        public void NoTransactionObjectContext()
        {
            using (var db = new TrackerEntities())
            {
                string emailDomain = "@test.com";

                int count = db.Users.Update(
                    u => u.EmailAddress.EndsWith(emailDomain),
                    u => new User { IsApproved = false, LastActivityDate = DateTime.Now });

                count = db.Users.Delete(u => u.EmailAddress.EndsWith(emailDomain));

            }
        }

        [Fact]
        public void TransactionScopeObjectContext()
        {
            using (var tx = new TransactionScope())
            using (var db = new TrackerEntities())
            {
                string emailDomain = "@test.com";

                int count = db.Users.Update(
                    u => u.EmailAddress.EndsWith(emailDomain),
                    u => new User { IsApproved = false, LastActivityDate = DateTime.Now });

                count = db.Users.Delete(u => u.EmailAddress.EndsWith(emailDomain));

                tx.Complete();
            }
        }

    }
}
