using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeHubDesktop.Database;
using TimeHubDesktop.Security;

//TODO: Napisać summary

namespace TimeHubDesktop
{
    public static class AppInitializer
    {
        public static readonly string AppDataDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeHub");
       
        public static readonly string DatabaseDirectoryPath = Path.Combine(AppDataDirectoryPath, "Database");


        public static void Initialize()
        {
            InitializeAppFolders();
            InitializeEncryptionKey();
            InitializeDatabase();
        }

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

        private static void InitializeEncryptionKey()
        {

            // Generujemy nowy klucz do bazy danych jeśli nie został jeszczee stworzonyy
            if (DatabaseEncryptionKeyManager.KeyExists() == false)
            {
                Debug.WriteLine($"Tworzenie klucza bazy danych: {DatabaseEncryptionKeyManager.DatabaseEncryptionKeyFilePath}");
                DatabaseEncryptionKeyManager.GenerateAndStoreKey();

            }
        }

        private static void InitializeDatabase()
        {
            if (DatabaseManager.DatabaseExists() == false)
            {
                Debug.WriteLine($"Tworzenie bazy danych: {DatabaseManager.DatabaseFilePath}");
            }
            DatabaseManager.InitializeDatabase();
        }
        
        private static void CheckAppVersion() { }
    }
}
