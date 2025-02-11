using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiCloud;

namespace TiCloud_Desktop.core.data
{
    public static class UpdateInfoManager
    {

        public static readonly string UpdatesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "updates");


        public static bool FolderExists()
        {
            return Directory.Exists(UpdatesFolderPath);
        }

        public static int CountUpdateFiles()
        {
            // Sprawdzenie, czy folder istnieje
            if (!FolderExists())
            {
                Debug.WriteLine("Folder z aktualizacjami nie istnieje.");
                return 0;
            }

            // Pobranie wszystkich plików z folderu, które zaczynają się na "update" i mają rozszerzenie .json
            string[] updateFiles = Directory.GetFiles(UpdatesFolderPath, "update_*.json");
            

            // Zwrócenie liczby tych plików
            return updateFiles.Length;
        }

    }

}
