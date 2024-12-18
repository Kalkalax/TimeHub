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
using Realms;
using System.Collections;
using TimeHubDesktop.Database.Models;
using TimeHubDesktop.Database;

//TODO: Tu jest wszystko do zrobienia, wyczyścić kod, dopisać funkcje(minimalizowanie,usuwanie projektów), poprawić komentarze

namespace TimeHubDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly WorkTimeTracker _workTimeTracker;
        private readonly RefreshTimer _refreshTimer;
        private WorkPeriod _currentWorkPeriod; // Inicjalizujemy wartość na początku do przechowywania obecnego okresu
                                                //RealmList(Iterable<T> items) => UnmanagedRealmList(items);

        private readonly List<WorkPeriod> _WorkPeriods; // Zmieniamy na nie-nullowalną listę
                                                             //do przechowywania okresów obecnej sesji

        public MainWindow()
        {
            InitializeComponent();

            LoadProjects();

            // Tworzymy obiekt WorkTimeTracker
            _workTimeTracker = new WorkTimeTracker();

            // Inicjalizacja timer'a do aktualizacji czasu 
            _refreshTimer = new RefreshTimer(_workTimeTracker, UpdateTimeDisplay);
            _refreshTimer.Start();

            //_WorkPeriods = new RealmList<WorkPeriod>();
            _WorkPeriods = [];

            ResetButton.IsEnabled = false; // Zablokowanie przycisku resetu na początku
            StartStopButton.IsEnabled = false; // Zablokowanie przycisku startu na początku


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
                    _currentWorkPeriod.PeriodEndTime = DateTime.Now.Ticks / 10000;
                    _currentWorkPeriod.CalculatePeriodTime();
                    _WorkPeriods.Add(_currentWorkPeriod);
                }
               
            }
            // jeśli jeszcze nie działa
            else
            {
                ResetButton.IsEnabled = false;
                ProjectsComboBox.IsEnabled = false;
                AddProjectButton.IsEnabled = false;

                _workTimeTracker.StartTracking();
                StartStopButton.Content = "Stop";
                Debug.WriteLine("Czas start");

                //Tworzymy nową sesje
                _currentWorkPeriod = new WorkPeriod
                {
                    PeriodStartTime = DateTime.Now.Ticks / 10000
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
                    Debug.WriteLine($"Start = {new DateTime(workPeriod.PeriodStartTime):yyyy-MM-dd HH:mm:ss.fff}");
                    Debug.WriteLine($"End = {new DateTime(workPeriod.PeriodEndTime):yyyy-MM-dd HH:mm:ss.fff}");
                    Debug.WriteLine($"Total Time = {new DateTime(workPeriod.PeriodTime):HH:mm:ss.fff}");
                }

                var selectedProject = ProjectsComboBox.SelectedItem as Project;
                DatabaseManager.SaveWorkSession(selectedProject.ProjectID, _WorkPeriods);

          
            }

            // Czyścimy liste 
            _WorkPeriods.Clear();

            //Blokujemy przycisk do momentu nowej sesji
            ResetButton.IsEnabled = false;

            //Odblokowujemy liste do zmiany 
            ProjectsComboBox.IsEnabled = true;
            AddProjectButton.IsEnabled = true;

            UpdateListView();

        }

        private void ProjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Włącz lub wyłącz przycisk na podstawie wybranej wartości
            StartStopButton.IsEnabled = ProjectsComboBox.SelectedItem != null;
            UpdateListView();
        }




        private void LoadProjects()
        {
            // Pobierz listę projektów z bazy danych
            //List<Project> projects = _realm.All<Project>().ToList();

            List<Project> projects = [.. DatabaseManager.GetAllProjects()];

            // Ustaw źródło danych dla ComboBox
            ProjectsComboBox.ItemsSource = projects;
        }

        private void UpdateListView()
        {
            var selectedProject = ProjectsComboBox.SelectedItem as Project;
            Debug.WriteLine($"{selectedProject.ProjectID}");

            // Pobranie wybranego projektu
            selectedProject = DatabaseManager.GetProjectById(selectedProject.ProjectID);

            if (selectedProject != null)
            {
                // Aktualizacja ListView z sesjami pracy
                DataListView.ItemsSource = selectedProject.ProjectWorkSessions.Select(WorkSession => new
                {
                    Date = WorkSession.SessionDate.ToString("d"),
                    Time = TimeSpan.FromMilliseconds(WorkSession.SessionTime).ToString(@"h\:mm\:ss")
                }).ToList();
            }
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            // Tworzymy instancję okna
            NameInputWindow nameInputWindow = new();

            // Wywołujemy okno i sprawdzamy, czy użytkownik kliknął "OK"
            if (nameInputWindow.ShowDialog() == true)
            {
                // Pobieramy wprowadzone dane (nazwę)
                string enteredName = nameInputWindow.EnteredName;
                Debug.WriteLine($"Wprowadzona nazwa: {enteredName}");
                DatabaseManager.AddProject(enteredName);
                LoadProjects();
            }

        }


    }

    
}