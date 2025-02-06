using MongoDB.Bson;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using TiCloud.Core.Database;
using TiCloud.Core.Database.Models;
using TiCloud.Core.Timers;
using TiCloud_Desktop.core.data;
using TiCloud_Desktop.viewmodels;
using TiCloud_Desktop.views.content;

//TODO: Tu jest wszystko do zrobienia, wyczyścić kod, dopisać funkcje(minimalizowanie,usuwanie projektów), poprawić komentarze

namespace TiCloud_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotifyIcon _notifyIcon;
        //private readonly WorkTimeTracker _workTimeTracker;
        //private readonly RefreshTimer _refreshTimer;
        //private readonly WorkPeriod _currentWorkPeriod;
        //private readonly List<WorkPeriod> _WorkPeriods;
                                                            

        public MainWindow()
        {
            InitializeComponent();

            // Sprawdzenie listy aktualizacji
            Debug.WriteLine($"Znaleziono {UpdateFolderManager.CountUpdateFiles()} aktualizacji");

            DataContext = new MainWindowViewModel();

            // Ustawienie ikony z zasobu osadzonego
            Icon icon = new(IconManager.TiCloudIconPath);
            this.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            // Inicjalizacja ikony zasobnika
            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon(IconManager.TiCloudIconPath), // Ustaw ikonę aplikacji
                //Icon = SystemIcons.Information,
                //Icon = new System.Drawing.Icon(TrayIconManager.TiCloudIconPath),
                Visible = false,
                Text = "TiCloud Desktop - Kliknij, aby przywrócić"
            };

            // Obsługa zdarzenia kliknięcia w ikonę
            _notifyIcon.Click += NotifyIcon_Click;
           

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Wymiary docelowego obszaru wyświetlania
            double targetWidth = 916;
            double targetHeight = 714;

            // Ustawienie wstępnych wartości
            this.Width = targetWidth;
            this.Height = targetHeight;

            // Pomiar okna
            this.Measure(new System.Windows.Size(Double.PositiveInfinity, Double.PositiveInfinity));
            this.Arrange(new Rect(0, 0, this.DesiredSize.Width, this.DesiredSize.Height));

            // Oblicz różnicę między całkowitym rozmiarem okna a obszarem wyświetlania
            double widthDifference = this.ActualWidth - this.RenderSize.Width;
            double heightDifference = this.ActualHeight - this.RenderSize.Height;

            // Dostosuj wymiary okna
            this.Width = targetWidth + widthDifference;
            this.Height = targetHeight + heightDifference;
        }


        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            _notifyIcon.Visible = false;
        }
    }
}
    



    
