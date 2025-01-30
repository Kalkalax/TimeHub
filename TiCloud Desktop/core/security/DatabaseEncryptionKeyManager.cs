using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace TiCloud.Core.Security
{
    /// <summary>
    /// Klasa zapewniająca funkcjonalność bezpiecznego generowania, przechowywania 
    /// oraz odczytywania klucza szyfrowania bazy danych.
    /// </summary>
    public static class DatabaseEncryptionKeyManager
    {
        /// <summary>
        /// Ścieżka do pliku, w którym przechowywany jest zaszyfrowany klucz bazy danych.
        /// </summary>
        public static readonly string DatabaseEncryptionKeyFilePath = Path.Combine(AppInitializer.AppDataDirectoryPath, "TiCloudDatabase_Key.dat");

        /// <summary>
        /// Sprawdza, czy plik zawierający zaszyfrowany klucz bazy danych już istnieje.
        /// </summary>
        /// <returns>
        /// Zwraca <c>true</c>, jeśli plik klucza istnieje; w przeciwnym razie <c>false</c>.
        /// </returns>
        public static bool KeyExists()
        {
            return File.Exists(DatabaseEncryptionKeyFilePath);
        }

        /// <summary>
        /// Generuje nowy losowy klucz szyfrowania, zabezpiecza go przy użyciu 
        /// mechanizmu ochrony danych użytkownika i zapisuje do pliku.
        /// </summary>
        /// <remarks>
        /// Klucz ma długość 64 bajtów i jest szyfrowany za pomocą 
        /// <see cref="ProtectedData"/> z wykorzystaniem <see cref="DataProtectionScope.CurrentUser"/>.
        /// </remarks>
        public static void GenerateAndStoreKey()
        {
            byte[] unprotectedKey = RandomNumberGenerator.GetBytes(64);
            byte[] protectedKey = ProtectedData.Protect(unprotectedKey, null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes(DatabaseEncryptionKeyFilePath, protectedKey);
        }

        /// <summary>
        /// Odczytuje i deszyfruje klucz szyfrowania bazy danych z pliku.
        /// </summary>
        /// <returns>
        /// Zwraca odszyfrowany klucz jako tablicę bajtów.
        /// </returns>
        /// <remarks>
        /// Zaszyfrowany klucz jest deszyfrowany przy użyciu ochrony danych użytkownika.
        /// Na potrzeby debugowania, klucz w formacie szesnastkowym jest wyświetlany w oknie debugowania.
        /// </remarks>
        public static byte[] GetKey()
        {
            byte[] protectedKey = File.ReadAllBytes(DatabaseEncryptionKeyFilePath);
            byte[] unprotectedKey = ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.CurrentUser);

            // Wyświetlanie hex w debug
            Debug.WriteLine(BitConverter.ToString(unprotectedKey).Replace("-", "").ToLower());

            return unprotectedKey;
        }
    }
}
