using MongoDB.Bson;
using Realms;
using System.IO;
using TiCloud.Core.Database.Models;
using TiCloud.Core.Security;

namespace TiCloud.Core.Database
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
        public static readonly string DatabaseFilePath = Path.Combine(AppInitializer.DatabaseDirectoryPath, "TiCloudDatabase.realm");

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
        /// Inicjalizuje bazę danych Realm, zapisuje konfigurację w pamięci oraz
        /// tworzy instancję bazy danych, jeśli nie została wcześniej utworzona.
        /// W przypadku migracji schematu bazy danych (z wersji < 2) dodaje kolumnę
        /// <see cref="Project.ProjectCreateDate"/> z domyślną wartością (data utworzenia projektu).
        /// </summary>
        public static void InitializeDatabase()
        {
            _realmConfiguration ??= new RealmConfiguration(DatabaseFilePath)
            {
                SchemaVersion = 2,
                EncryptionKey = DatabaseEncryptionKeyManager.GetKey(),
                IsReadOnly = false,

                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    if (oldSchemaVersion < 2)
                    {
                        // Przeprowadzamy migrację dla nowych wersji (dodanie kolumny)
                        var projects = migration.NewRealm.All<Project>();
                        foreach (var project in projects)
                        {
                            // Ustawiamy domyślną wartość dla kolumny "ProjectCreateDate"
                            project.ProjectCreateDate = DateTimeOffset.Now;
                        }
                    }
                }
            };
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
            return _realmInstance ?? throw new InvalidOperationException("Database has not been initialized. Call InitializeDatabase() first.");
        }

        /// <summary>
        /// Dodaje nowy projekt do bazy danych Realm.
        /// </summary>
        /// <param name="projectName">Nazwa projektu do dodania.</param>
        public static void AddProject(string projectName)
        {
            var realmInstance = GetRealmInstance();

            realmInstance.Write(() =>
            {
                var project = new Project
                {
                    ProjectName = projectName,
                };
                realmInstance.Add(project);
            });
        }

        /// <summary>
        /// Usuwa projekt oraz wszystkie powiązane sesje pracy i okresy pracy z bazy danych.
        /// Jeśli projekt o podanym identyfikatorze nie zostanie znaleziony, zostanie rzucony wyjątek <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="projectId">Unikalny identyfikator projektu, który ma zostać usunięty.</param>
        /// <exception cref="InvalidOperationException">Jeśli projekt o podanym identyfikatorze nie istnieje.</exception>
        public static void DeleteProject(ObjectId projectId)
        {
            var realmInstance = GetRealmInstance();
            var project = realmInstance.All<Project>().FirstOrDefault(p => p.ProjectID == projectId) 
                ?? throw new InvalidOperationException($"Project with ID {projectId} not found.");
            
            realmInstance.Write(() =>
            {
                // Usuń wszystkie powiązane sesje pracy (WorkSession)
                foreach (var workSession in project.ProjectWorkSessions)
                {
                    // Usuń powiązane okresy pracy (WorkPeriod)
                    foreach (var workPeriod in workSession.SessionWorkPeriods)
                    {
                        realmInstance.Remove(workPeriod);
                    }

                    // Usuń sesję pracy
                    realmInstance.Remove(workSession);
                }

                // Usuń projekt
                realmInstance.Remove(project);
            });

        }


        /// <summary>
        /// Zapisuje sesję pracy do bazy danych Realm w kontekście określonego projektu.
        /// Jeśli projekt o podanym identyfikatorze nie istnieje, zostaje rzucony wyjątek.
        /// </summary>
        /// <param name="projectId">Identyfikator projektu, do którego przypisana jest sesja pracy.</param>
        /// <param name="workPeriods">Lista okresów pracy, które mają zostać zapisane w sesji pracy.</param>
        /// <exception cref="InvalidOperationException">
        /// Rzucane, jeśli projekt o podanym identyfikatorze nie zostanie znaleziony w bazie danych.
        /// </exception>
        public static void SaveWorkSession(ObjectId projectId, List<WorkPeriod> workPeriods)
        {
            var realmInstance = GetRealmInstance();
            var workSession = new WorkSession();
            var project = realmInstance.All<Project>().FirstOrDefault(p => p.ProjectID == projectId) 
                ?? throw new InvalidOperationException($"Project with ID {projectId} not found.");
            
            realmInstance.Write(() =>
            {
                foreach (var period in workPeriods)
                {
                    period.CalculatePeriodTime();
                    workSession.SessionWorkPeriods.Add(period);
                }

                workSession.CalculateSessionTime();
                realmInstance.Add(workSession);

                project.ProjectWorkSessions.Add(workSession);
                project.CalculateProjectTime();
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
        public static IQueryable<WorkSession> GetAllWorkSessions()
        {
            var realmInstance = GetRealmInstance();

            return realmInstance.All<WorkSession>();
        }

        /// <summary>
        /// Pobiera wszystkie projekty zapisane w bazie danych Realm.
        /// </summary>
        /// <returns>
        /// Obiekt <see cref="IQueryable{T}"/> zawierający wszystkie projekty.
        /// </returns>
        public static IQueryable<Project> GetAllProjects()
        {
            var realmInstance = GetRealmInstance();

            return realmInstance.All<Project>();
        }

        /// <summary>
        /// Pobiera projekt na podstawie jego identyfikatora z bazy danych Realm.
        /// </summary>
        /// <param name="projectId">Identyfikator projektu do pobrania.</param>
        /// <returns>
        /// Projekt o podanym identyfikatorze.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Rzucane, jeśli projekt o podanym identyfikatorze nie został znaleziony.
        /// </exception>
        public static Project GetProjectById(ObjectId projectId)
        {
            var realmInstance = GetRealmInstance();
            var project = realmInstance.All<Project>().FirstOrDefault(p => p.ProjectID == projectId);

            return project ?? throw new InvalidOperationException($"Project with ID {projectId} not found.");
        }

        /// <summary>
        /// Zwraca całkowity czas spędzony na projekcie na podstawie jego identyfikatora ProjectID.
        /// Czas jest pobierany z przechowywanej wartości czasu w milisekundach i konwertowany na obiekt TimeSpan.
        /// </summary>
        /// <param name="projectId">Unikalny identyfikator projektu.</param>
        /// <returns>Obiekt <see cref="TimeSpan"/> reprezentujący całkowity czas spędzony na projekcie. 
        /// Zwraca <see cref="TimeSpan.Zero"/> jeśli projekt nie został znaleziony.</returns>
        public static TimeSpan GetTotalTimeSpentOnProject(ObjectId projectId)
        {
            var realmInstance = GetRealmInstance();
            var project = realmInstance.All<Project>().FirstOrDefault(p => p.ProjectID == projectId);
            
            if (project == null)
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromMilliseconds(project.ProjectTime); // Zakładamy, że ProjectTime jest w ms
        }
    }
}
