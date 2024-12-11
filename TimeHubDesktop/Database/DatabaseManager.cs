using Realms;
using System.IO;
using TimeHubDesktop.Database.Models;

//TODO: Dodać model odpowiedzialny za przetrzymywanie projektów i zaimplementować.

namespace TimeHubDesktop.Database
{
    /// <summary>
    /// Klasa zarządzająca operacjami na bazie danych Realm.
    /// Zapewnia metody inicjalizacji bazy, weryfikacji jej istnienia, 
    /// zarządzania instancją, zapisywania oraz odczytu danych w bazie.
    /// </summary>
    public class DatabaseManager
    {
        /// <summary>
        /// Statyczna instancja bazy danych Realm, używana do wykonywania operacji na bazie.
        /// Jest tworzona podczas inicjalizacji i przechowywana w pamięci dla wielokrotnego użycia.
        /// </summary>
        private static Realm? _realmInstance;

        /// <summary>
        /// Konfiguracja bazy danych Realm, definiująca ścieżkę do pliku, wersję schematu oraz klucz szyfrujący.
        /// Tworzona raz podczas inicjalizacji.
        /// </summary>
        private static RealmConfiguration? _realmConfiguration;

        /// <summary>
        /// Ścieżka do pliku bazy danych Realm.
        /// </summary>
        public static readonly string DatabaseFilePath = Path.Combine(AppInitializer.DatabaseDirectoryPath, "TimeHubDatabase.realm");

        /// <summary>
        /// Sprawdza, czy plik bazy danych istnieje w określonej ścieżce.
        /// </summary>
        /// <returns>
        /// <c>true</c>, jeśli plik bazy danych istnieje; w przeciwnym razie <c>false</c>.
        /// </returns>
        public static bool DatabaseExists()
        {
            return File.Exists(DatabaseFilePath);
        }

        /// <summary>
        /// Inicjalizuje bazę danych Realm i zapisuje konfigurację w pamięci.
        /// </summary>
        public static void InitializeDatabase()
        {
            if (_realmConfiguration == null)
            {
                _realmConfiguration = new RealmConfiguration(DatabaseFilePath)
                {
                    SchemaVersion = 1,
                    EncryptionKey = EncryptionKeyManager.RetrieveKey(),
                    IsReadOnly = false
                };
            }

            _realmInstance = Realm.GetInstance(_realmConfiguration);
        }

        /// <summary>
        /// Zwraca instancję bazy danych Realm.
        /// </summary>
        /// <returns>Instancja bazy danych Realm.</returns>
        /// <exception cref="InvalidOperationException">
        /// Rzucane, jeśli baza danych nie została zainicjalizowana.
        /// </exception>
        public static Realm GetRealmInstance()
        {
            if (_realmInstance == null)
            {
                throw new InvalidOperationException("Database has not been initialized. Call InitializeDatabase() first.");
            }

            return _realmInstance;
        }

        /// <summary>
        /// Zapisuje sesję pracy do bazy danych Realm.
        /// </summary>
        /// <param name="workPeriods">Lista okresów pracy.</param>
        public static void SaveWorkSession(List<WorkPeriod> workPeriods)
        {
            var realmInstance = GetRealmInstance();
            var workSession = new WorkSession();

            realmInstance.Write(() =>
            {
                foreach (var period in workPeriods)
                {
                    period.CalculatePeriodTime();
                    workSession.SessionWorkPeriods.Add(period);
                }

                workSession.CalculateSessionTime();
                realmInstance.Add(workSession);
            });
        }

        /// <summary>
        /// Pobiera wszystkie rekordy z tabeli <see cref="WorkSession"/> w bazie danych Realm.
        /// </summary>
        /// <returns>
        /// Obiekt <see cref="IQueryable{T}"/> zawierający wszystkie rekordy z tabeli <see cref="WorkSession"/>.
        /// </returns>
        /// <remarks>
        /// Metoda umożliwia wykonanie dodatkowych zapytań LINQ na danych, takich jak filtrowanie, sortowanie czy projekcja.
        /// Upewnij się, że baza danych została zainicjalizowana za pomocą <see cref="InitializeDatabase"/> przed wywołaniem tej metody.
        /// </remarks>
        /// <example>
        /// Przykład użycia:
        /// <code>
        /// var workSessions = DatabaseManager.GetAllWorkSessions();
        /// foreach (var session in workSessions)
        /// {
        ///     Console.WriteLine(session.SessionID);
        /// }
        /// </code>
        /// </example>
        public static IQueryable<WorkSession> GetAllWorkSessions()
        {
            var realmInstance = GetRealmInstance();

            return realmInstance.All<WorkSession>();
        }
 
    }
}
