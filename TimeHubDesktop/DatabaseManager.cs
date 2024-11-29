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

        public static readonly string DatabaseFilePath = Path.Combine(AppInitializer.DatabaseDirectoryPath, "TimeHubDatabase.realm");

        public static void InitializeDatabase()
        {

            var config = new RealmConfiguration(DatabaseFilePath)
            {
                SchemaVersion = 1,
                EncryptionKey = EncryptionKeyManager.RetrieveKey(),
                IsReadOnly = false
            };

            _realmInstance = Realm.GetInstance(config);
        }

        public static void SaveWorkSession(List<WorkPeriod> workPeriods)
        {
            var config = new RealmConfiguration(DatabaseFilePath)
            {
                SchemaVersion = 1,
                EncryptionKey = EncryptionKeyManager.RetrieveKey(),
                IsReadOnly = false
            };

            _realmInstance = Realm.GetInstance(config);

            _realmInstance.Write(() =>
            {
                var workSession = new WorkSession();
                
                foreach (var period in workPeriods)
                {
                    workSession.WorkPeriods.Add(period);
        
                }


                _realmInstance.Add(workSession);
            });
        }

        public static bool DatabaseExists()
        {
            return File.Exists(DatabaseFilePath);
        }


        public static Realm? GetRealmInstance() => _realmInstance;

    }
}
