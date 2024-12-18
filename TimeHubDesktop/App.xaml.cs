using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

//TODO: Napisać summary

namespace TimeHubDesktop
{
  
    public partial class App : Application
    {
        
        // Metoda uruchamiana podczas startu systemu
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppInitializer.Initialize();
               
        }






    }

}
