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
        private WorkPeriod? _currentWorkPeriod; // Inicjalizujemy wartość na początku do przechowywania obecnego okresu


        private readonly IList<WorkPeriod> _WorkPeriods; //do przechowywania okresów obecnej sesji

        public MainWindow()
        {
            InitializeComponent();

            // Tworzymy obiekt WorkTimeTracker
            _workTimeTracker = new WorkTimeTracker();

            // Inicjalizacja timer'a do aktualizacji czasu 
            _refreshTimer = new RefreshTimer(_workTimeTracker, UpdateTimeDisplay);
            _refreshTimer.Start();

            //Inicjalizacja listy do przechowywania czasu
            _WorkPeriods = [];

            ResetButton.IsEnabled = false; // Zablokowanie przycisku resetu na początku

            
          
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
                
                
                // Jeśli _currentWorkPeriod nie jest null, aktualizujemy sesję
                if (_currentWorkPeriod != null)
                {
                    _currentWorkPeriod.PeriodEnd = DateTimeOffset.Now;
                    _WorkPeriods.Add(_currentWorkPeriod);
                }
               
            }
            // jeśli jeszcze nie działa
            else
            {
                ResetButton.IsEnabled = false;
                _workTimeTracker.StartTracking();
                StartStopButton.Content = "Stop";
                Debug.WriteLine("Czas start");

                //Tworzymy nową sesje
                _currentWorkPeriod = new WorkPeriod
                {
                    PeriodStart = DateTimeOffset.Now
                };

            }

        }

        // Metoda, która obsługuje naciśnięcie przycisku resetu (zakończenia sesji)
        private void ResetButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_workTimeTracker.IsTracking)
            {
                Debug.WriteLine($"Reset, czas: {_workTimeTracker.TotalWorkTime}");
                _workTimeTracker.ResetTracking();

                //Wyświetlamy dane o sesji
                Debug.WriteLine("Current Work Periods:");

                foreach (var workPeriod in _WorkPeriods)
                {
                    Debug.WriteLine($"Start = {workPeriod.PeriodStart}");
                    Debug.WriteLine($"End = {workPeriod.PeriodEnd}");
                    Debug.WriteLine($"Total Time = {workPeriod.PeriodTime}");
                }
            }

            // Czyścimy liste 
            _WorkPeriods.Clear();

            //Blokujemy przycisk do momentu nowej sesji
            ResetButton.IsEnabled = false;

        }




    }
}