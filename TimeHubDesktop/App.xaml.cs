using System.Windows;

//TODO: Napisać summary

namespace TimeHubDesktop
{

    public partial class App : System.Windows.Application
    {
        
        // Metoda uruchamiana podczas startu systemu
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppInitializer.Initialize();
               
        }






    }

}
