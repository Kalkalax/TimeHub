using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Windows.Input;

namespace TimeHubDesktop
{

    /// <summary>
    /// Klasa odpowiedzialna za dostarczenie szyfrowania bazy danych
    /// </summary>
    /// <returns>A string populated with random choices.</returns>
    /// <exception>
    ///  
    /// </exception>
    public static class EncryptionKeyManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string KeyFilePath = Path.Combine(AppInitializer.AppDataDirectoryPath, "TimeHubDatabase_Key.dat");

        public static bool KeyExists()
        {
            return File.Exists(KeyFilePath);
        }

        /// <summary>
        /// Metoda odpowiedzialna za generowanie klucza szyfrowania
        /// </summary>
        public static void GenerateAndStoreKey()
        {
            byte[] unprotectedKey = RandomNumberGenerator.GetBytes(64);
            byte[] protectedKey = ProtectedData.Protect(unprotectedKey, null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes(KeyFilePath, protectedKey);

        }




        // Metod odpowiedzialna za odczyt klucza z zaszyfrowanego pliku
        public static byte[] RetrieveKey()
        {
            byte[] protectedKey = File.ReadAllBytes(KeyFilePath);
            byte[] unprotectedKey = ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.CurrentUser);

            // Wyświetlanie hex w debug
            string hexString = BitConverter.ToString(unprotectedKey).Replace("-", "").ToLower();
            Debug.WriteLine(hexString);

            return unprotectedKey;

        }




    }
}
