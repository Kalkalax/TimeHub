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
    public static class EncryptionKeyManager
    {
        private const string KeyFilePath = "database_key.dat";

        // Metoda odpowiedzialna za generowanie klucza szyfrowania
        public static void GenerateAndStoreKey()
        {
            byte[] key = RandomNumberGenerator.GetBytes(64);
            byte[] protectedKey = ProtectedData.Protect(key, null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes(KeyFilePath, protectedKey);
            Debug.WriteLine($"Key: {Convert.ToBase64String(key)}");
        }

        // Metoda odpowiedzialna za sprawdzenie istnienia klucza szyfrowania 
        public static bool KeyExists()
        {
            return File.Exists(KeyFilePath);
        }

        // Metod odpowiedzialna za odczyt klucza z zaszyfrowanego pliku
        public static byte[] RetrieveKey()
        {
            byte[] protectedKey = File.ReadAllBytes(KeyFilePath);
            byte[] unprotectedKey = ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.CurrentUser);
            Debug.WriteLine($"Key: {Convert.ToBase64String(unprotectedKey)}");
            return unprotectedKey;

        }




    }
}
