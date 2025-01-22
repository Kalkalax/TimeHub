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

        public TimerBar()
        {
            InitializeComponent();

            //Załadowanie projektów do listy wyboru
            LoadProjectsFromDatabaseToComboBox();

            //Tworzymy obiekt WorkTimeTracker
            _workTimeTracker = new WorkTimeTracker();

            //Inicjalizacja timer'a do aktualizacji czasu 
            _refreshTimer = new RefreshTimer(_workTimeTracker, UpdateTimeDisplay);
            _refreshTimer.Start();

            //Tworzymy liste WorkPeriod'ów
            _WorkPeriods = [];

        }

        //Metoda, która będzie wywoływana przez RefreshTimer do zaktualizowania UI
        private void UpdateTimeDisplay(TimeSpan totalWorkTime)
        {
            //TimeLabel.Content = totalWorkTime.ToString(@"hh\:mm\:ss");
            //TODO: zmienić nazwe funkcji do wyswietlacza czasu
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
            if (StartStopTimmer_Button.Content is SvgViewbox svgViewbox)
            {
                svgViewbox.Opacity = 1.0; 
            }

            //DEBUG
            if (ProjectsList_ComboBox.SelectedItem is Project selectedProject)
            {
                Debug.WriteLine($"Wybrano projekt: {selectedProject.ProjectName}, ID: {selectedProject.ProjectID}");
            }
        }







        private void StartStopTimmer_Button_Click(object sender, RoutedEventArgs e)
        {
            //Zablokowanie listy projektów
            ProjectsList_ComboBox.IsEnabled = false;

            //Uruchomienie timera


        }

        private void ReloadTimmer_Button_Click(object sender, RoutedEventArgs e)
        {


        }

        
    }
}
