using SharpVectors.Converters;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using TiCloud.Core.Database;
using TiCloud.Core.Database.Models;
using TiCloud.Core.Timers;

namespace TiCloud_Desktop.views.controls
{
    /// <summary>
    /// Logika interakcji dla klasy TimerBar.xaml
    /// </summary>
    public partial class TimerBar : System.Windows.Controls.UserControl
    {
        private readonly WorkTimeTracker _workTimeTracker;
        private readonly RefreshTimer _refreshTimer;
        private readonly List<WorkPeriod> _WorkPeriods;
        private WorkPeriod _currentWorkPeriod;

        public TimerBar()
        {
            InitializeComponent();

            //Załadowanie projektów do listy wyboru
            LoadProjectsFromDatabaseToComboBox();

            //Tworzymy obiekt WorkTimeTracker
            _workTimeTracker = new WorkTimeTracker();

            //Inicjalizacja timer'a do aktualizacji czasu 
            _refreshTimer = new RefreshTimer(_workTimeTracker, TimeDisplay_Update);
            _refreshTimer.Start();

            //Tworzymy liste WorkPeriod'ów
            //_WorkPeriods = new RealmList<WorkPeriod>();
            _WorkPeriods = [];

        }

        //Metoda, która będzie wywoływana przez RefreshTimer do zaktualizowania UI
        private void TimeDisplay_Update(TimeSpan totalWorkTime)
        {
            TimeDisplay.Content = totalWorkTime.ToString(@"hh\:mm\:ss");
        }

        private void LoadProjectsFromDatabaseToComboBox()
        {
            //Pobranie listy projektów z bazy danych
            List<Project> projects = [.. DatabaseManager.GetAllProjects()];

            //Zablokowanie ComboBox jeśli nie ma projektów
            if (projects.Count == 0)
            {
                ProjectsList_ComboBox.IsEnabled = false;
            }
            
            //Ustawienie żródła dla listy
            ProjectsList_ComboBox.ItemsSource = projects;
        }

        private void ProjectsList_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Włącz lub wyłącz przycisk startu timera na podstawie wybranej wartości
            StartStopTimmer_Button.IsEnabled = ProjectsList_ComboBox.SelectedItem != null;

            //Zmiana przezroczystości ikony przycisku start na 100%
            if (StartStopTimmer_Button.Content is SvgViewbox StartStopTimmersSvgViewbox)
            {
                StartStopTimmersSvgViewbox.Opacity = 1.0; 
            }

            //DEBUG
            if (ProjectsList_ComboBox.SelectedItem is Project selectedProject)
            {
                Debug.WriteLine($"[Debug] Wybrano projekt: {selectedProject.ProjectName}, ID: {selectedProject.ProjectID}");
            }
        }

        private void StartStopTimmer_Button_Click(object sender, RoutedEventArgs e)
        {
            // jeśli działa
            if (_workTimeTracker.IsTracking)
            {
                ReloadTimmer_Button.IsEnabled = true;
                if (ReloadTimmer_Button.Content is SvgViewbox ReloadTimmerSvgViewbox)
                {
                    ReloadTimmerSvgViewbox.Opacity = 1.0;
                }

                _workTimeTracker.StopTracking();
                
                if (StartStopTimmer_Button.Content is SvgViewbox StartStopTimmerSvgViewbox)
                {
                    StartStopTimmerSvgViewbox.Source = new Uri("/resources/images/play_icon.svg", UriKind.Relative);
                }

                Debug.WriteLine("[Debug] Wstrzymano pomiar czasu");

                // Jeśli _currentWorkPeriod nie jest null, aktualizujemy sesję
                if (_currentWorkPeriod != null)
                {
                    _currentWorkPeriod.PeriodEndTime = System.DateTime.Now.Ticks / 10000;
                    _currentWorkPeriod.CalculatePeriodTime();
                    _WorkPeriods.Add(_currentWorkPeriod);
                }
            }
            // jeśli jeszcze nie działa
            else
            {
                ReloadTimmer_Button.IsEnabled = false;
                if (ReloadTimmer_Button.Content is SvgViewbox ReloadTimmerSvgViewbox)
                {
                    ReloadTimmerSvgViewbox.Opacity = 0.6;
                }

                ProjectsList_ComboBox.IsEnabled = false;
  
                _workTimeTracker.StartTracking();

                if (StartStopTimmer_Button.Content is SvgViewbox svgViewbox)
                {
                    svgViewbox.Source = new Uri("/resources/images/pause_icon.svg", UriKind.Relative);
                }

                ProjectsList_ComboBox.Width = 31;


                Debug.WriteLine("[Debug] Rozpoczęto pomiar czasu");

                //Tworzymy nową sesje
                _currentWorkPeriod = new WorkPeriod
                {
                    PeriodStartTime = System.DateTime.Now.Ticks / 10000
                };
            }
        }

        private void ReloadTimmer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_workTimeTracker.IsTracking)
            {
                _workTimeTracker.ResetTracking();

                //Wyświetlamy dane o sesji
                Debug.WriteLine($"[Debug] Zapisano sesje pracy");
                Debug.WriteLine("[Debug] Current Work Periods:");

                foreach (var workPeriod in _WorkPeriods)
                {
                    // Zamieniamy milisekundy na DateTime (start i koniec okresu)
                    var startTime = new DateTime(workPeriod.PeriodStartTime * 10000, DateTimeKind.Utc);
                    var endTime = new DateTime(workPeriod.PeriodEndTime * 10000, DateTimeKind.Utc);

                    // Zamieniamy milisekundy czasu trwania na TimeSpan
                    var totalTime = TimeSpan.FromMilliseconds(workPeriod.PeriodTime);

                    Debug.WriteLine($"[Debug] Start = {startTime:yyyy-MM-dd HH:mm:ss.fff}");
                    Debug.WriteLine($"[Debug] End = {endTime:yyyy-MM-dd HH:mm:ss.fff}");
                    Debug.WriteLine($"[Debug] Total Time = {totalTime:hh\\:mm\\:ss\\.fff}");
                }

                var selectedProject = ProjectsList_ComboBox.SelectedItem as Project;

                DatabaseManager.SaveWorkSession(selectedProject.ProjectID, _WorkPeriods);
            }

            // Czyścimy liste 
            _WorkPeriods.Clear();

            //Blokujemy przycisk do momentu nowej sesji
            ReloadTimmer_Button.IsEnabled = false;

            //Odblokowujemy liste do zmiany 
            ProjectsList_ComboBox.IsEnabled = true;
            ProjectsList_ComboBox.Width = 200;
        }
    }
}
