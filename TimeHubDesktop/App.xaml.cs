using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace TimeHubDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        // Metoda uruchamiana podczas startu systemu
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Generujemy nowy klucz do bazy danych jeśli nie został stworzonyy
            if(EncryptionKeyManager.KeyExists() == false)
            {
                Debug.WriteLine("Tworzenie klucza dla bazy danych ");
                EncryptionKeyManager.GenerateAndStoreKey();
               
            }

            Debug.WriteLine("Wczytywanie klucza dla bazy danych ");
            byte[] key = EncryptionKeyManager.RetrieveKey();


        }






    }

}
