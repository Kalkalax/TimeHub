using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;
using System.IO;

namespace TimeHubDesktop
{
    public class DatabaseManager
    {
        private static Realm? _realmInstance;

        public static void InitializeDatabase(string databasePath)
        {




            var config = new RealmConfiguration(databasePath)
            {
                SchemaVersion = 1
            };

            _realmInstance = Realm.GetInstance(config);
        }



    }
}
