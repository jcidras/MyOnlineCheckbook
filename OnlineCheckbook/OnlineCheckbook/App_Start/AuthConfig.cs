using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using OnlineCheckbook.DAL;

namespace OnlineCheckbook
{
    public class AuthConfig
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public static void RegisterAuth()
        {
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<BankContext>(null);
                //Database.SetInitializer(new DropCreateDatabaseAlways<BankContext>());
                try
                {
                    using (var context = new BankContext())
                    {
                        if (!context.Database.Exists())
                        {
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
                    //if (!WebSecurity.Initialized)
                    //    WebSecurity.InitializeDatabaseConnection("BankContext", "User", "UserId", "Username", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}