using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;
using System.IO;
using TimeHubDesktop.Database.Models;

namespace TimeHubDesktop
{
    public class DatabaseManager
    {
        private static Realm? _realmInstance;

        /// <summary>
        /// Zmienna przechowująca lokalizacje bazy danych
        /// </summary>
        public static readonly string DatabaseFilePath = Path.Combine(AppInitializer.DatabaseDirectoryPath, "TimeHubDatabase.realm");

        /// <summary>
        /// Metoda inicjalizująca baze danych Realm
        /// </summary>
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

        /// <summary>
        /// Metoda zapisująca sesje pracy do bazy danych Realm
        /// </summary>
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
                    period.CalculatePeriodTime();   
                    workSession.SessionWorkPeriods.Add(period);
                }
                workSession.CalculateSessionTime();

                _realmInstance.Add(workSession);
            });
        }

        /// <summary>
        /// Metoda sprawdzająca istnienie bazy danych Realm
        /// </summary>
        /// <returns>
        /// <paramref name="true"/> jeśli plik istnieje, w przeciwnym razie false
        /// </returns>
        public static bool DatabaseExists()
        {
            return File.Exists(DatabaseFilePath);
        }

        /// <summary>
        /// Metoda zwracająca aktualną instancje bazy danych Realm
        /// </summary>
        /// <returns>
        /// Zwraca zmienna <paramref name="_realmInstance" > będącą instancją bazy Realm</paramref>
        /// </returns>
        public static Realm? GetRealmInstance() => _realmInstance;

    }
}
