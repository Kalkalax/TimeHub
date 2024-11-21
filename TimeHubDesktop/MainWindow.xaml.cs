using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace TimeHubDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly WorkTimeTracker _workTimeTracker;
        private readonly RefreshTimer _refreshTimer;

        public MainWindow()
        {
            InitializeComponent();

            // Tworzymy obiekt WorkTimeTracker
            _workTimeTracker = new WorkTimeTracker();

            // Inicjalizacja timer'a do aktualizacji czasu 
            _refreshTimer = new RefreshTimer(_workTimeTracker, UpdateTimeDisplay);
            _refreshTimer.Start();
          
        }

        // Metoda, która będzie wywoływana przez RefreshTimer do zaktualizowania UI
        private void UpdateTimeDisplay(TimeSpan totalWorkTime)
        {
            TimeLabel.Content = totalWorkTime.ToString(@"hh\:mm\:ss");
        }


        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            // jeśli działa
            if (_workTimeTracker.IsTracking)
            {
                ResetButton.IsEnabled = true;
                _workTimeTracker.StopTracking();
                StartStopButton.Content = "Start";
                Debug.WriteLine("Czas stop");
            }
            // jeśli jeszcze nie działa
            else
            {
                ResetButton.IsEnabled = false;
                _workTimeTracker.StartTracking();
                StartStopButton.Content = "Stop";
                Debug.WriteLine("Czas start");
            }

        }

        // Metoda, która obsługuje naciśnięcie przycisku resetu (zakończenia sesji)
        private void ResetButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_workTimeTracker.IsTracking)
            {
                Debug.WriteLine($"Reset, czas: {_workTimeTracker.TotalWorkTime}");
                _workTimeTracker.ResetTracking();
            }
        }




    }
}