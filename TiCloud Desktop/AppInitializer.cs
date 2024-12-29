using System.Diagnostics;
using System.IO;
using TiCloud.Core.Database;
using TiCloud.Core.Security;

namespace TiCloud
{
    /// <summary>
    /// Klasa odpowiedzialna za inicjalizację aplikacji, międzzy innymi tworzenie wymaganych folderów,
    /// generowanie klucza szyfrowania bazy danych czy inicjalizowanie bazy danych.
    /// </summary>
    public static class AppInitializer
    {
        /// <summary>
        /// Ścieżka do katalogu danych aplikacji w folderze aplikacji użytkownika.
        /// </summary>
        public static readonly string AppDataDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TiCloud");
        //public static readonly string AppDataDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TiCloudTest");

        /// <summary>
        /// Ścieżka do katalogu, w którym przechowywana jest baza danych aplikacji.
        /// </summary>
        public static readonly string DatabaseDirectoryPath = Path.Combine(AppDataDirectoryPath, "Database");

        /// <summary>
        /// Inicjalizuje aplikację poprzez utworzenie wymaganych folderów (jeśli nie istnieją), 
        /// wygenerowanie klucza szyfrowania bazy danych (jeśli brak) oraz zainicjowanie bazy danych.
        /// </summary>
        public static void Initialize()
        {
            InitializeAppFolders();
            InitializeEncryptionKey();
            InitializeDatabase();
        }

        /// <summary>
        /// Sprawdza istnienie wymaganych folderów i tworzy je, jeśli nie zostały jeszcze utworzone.
        /// Foldery obejmują katalog główny aplikacji oraz folder bazy danych.
        /// </summary>
        private static void InitializeAppFolders()
        {
            if (!Directory.Exists(AppDataDirectoryPath))
            {
                Debug.WriteLine($"Tworzenie folderu aplikacji: {AppDataDirectoryPath}");
                Directory.CreateDirectory(AppDataDirectoryPath);
            }
            if (!Directory.Exists(DatabaseDirectoryPath))
            {
                Debug.WriteLine($"Tworzenie foldera bazy danych: {DatabaseDirectoryPath} ");
                Directory.CreateDirectory(DatabaseDirectoryPath);
            }
        }

        /// <summary>
        /// Sprawdza, czy klucz szyfrowania bazy danych już istnieje. Jeśli nie, generuje nowy klucz 
        /// i zapisuje go w odpowiednim pliku w celu zabezpieczenia danych przechowywanych w bazie.
        /// </summary>
        private static void InitializeEncryptionKey()
        {
            // Generujemy nowy klucz do bazy danych jeśli nie został jeszczee stworzonyy
            if (DatabaseEncryptionKeyManager.KeyExists() == false)
            {
                Debug.WriteLine($"Tworzenie klucza bazy danych: {DatabaseEncryptionKeyManager.DatabaseEncryptionKeyFilePath}");
                DatabaseEncryptionKeyManager.GenerateAndStoreKey();
            }
        }

        /// <summary>
        /// Inicjalizuje bazę danych aplikacji. Sprawdza, czy baza danych już istnieje, a jeśli nie, 
        /// tworzy ją i uruchamia wszystkie wymagane operacje inicjalizacyjne.
        /// </summary>
        private static void InitializeDatabase()
        {
            if (DatabaseManager.DatabaseExists() == false)
            {
                Debug.WriteLine($"Tworzenie bazy danych: {DatabaseManager.DatabaseFilePath}");
            }
            DatabaseManager.InitializeDatabase();
        }

        /// <summary>
        /// Metoda do sprawdzenia wersji aplikacji. Aktualnie niezaimplementowana.
        /// </summary>
        private static void CheckAppVersion() { }
    }
}
