using MongoDB.Bson;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using TiCloud.Core.Database;
using TiCloud.Core.Database.Models;
using TiCloud.Core.Timers;
using TiCloud_Desktop.views.content;

//TODO: Tu jest wszystko do zrobienia, wyczyścić kod, dopisać funkcje(minimalizowanie,usuwanie projektów), poprawić komentarze

namespace TiCloud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;
        private readonly WorkTimeTracker _workTimeTracker;
        private readonly RefreshTimer _refreshTimer;
        private WorkPeriod _currentWorkPeriod;
                                              

        private readonly List<WorkPeriod> _WorkPeriods;
                                                            

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new HomeView();
            
            LoadProjects();

            // Ustawienie ikony z zasobu osadzonego
            Icon icon = new Icon(IconManager.TiCloudIconPath);
            this.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            // Tworzymy obiekt WorkTimeTracker
            _workTimeTracker = new WorkTimeTracker();

            // Inicjalizacja timer'a do aktualizacji czasu 
            _refreshTimer = new RefreshTimer(_workTimeTracker, UpdateTimeDisplay);
            _refreshTimer.Start();

            //_WorkPeriods = new RealmList<WorkPeriod>();
            _WorkPeriods = [];

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
            



            ResetButton.IsEnabled = false; // Zablokowanie przycisku resetu na początku
            StartStopButton.IsEnabled = false; // Zablokowanie przycisku startu na początku
            //DeleteProjectButton.IsEnabled = false;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Wymiary docelowego obszaru wyświetlania
            double targetWidth = 900;
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

        // Obsługa zamykania okna
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            // Chowamy aplikację do zasobnika tylko, jeśli czas jest mierzony
            if (_workTimeTracker.IsTracking)
            {
                e.Cancel = true;
                Hide();
                _notifyIcon.Visible = true;
            }
            else
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }
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
                //AddProjectButton.IsEnabled = false;
                //DeleteProjectButton.IsEnabled = false;

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
            //AddProjectButton.IsEnabled = true;
            //DeleteProjectButton.IsEnabled = true;

            UpdateListView();

        }

        private void ProjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Włącz lub wyłącz przycisk na podstawie wybranej wartości
            StartStopButton.IsEnabled = ProjectsComboBox.SelectedItem != null;
            //DeleteProjectButton.IsEnabled = true;
            UpdateListView();
        }




        private void LoadProjects()
        {
            // Pobierz listę projektów z bazy danych
            //List<Project> projects = _realm.All<Project>().ToList();

            List<Project> projects = [.. DatabaseManager.GetAllProjects()];

            if (projects.Count == 0)
            {
                //DeleteProjectButton.IsEnabled = false;
                ProjectsComboBox.IsEnabled = false;
            }

            // Ustaw źródło danych dla ComboBox
            ProjectsComboBox.ItemsSource = projects;
        }

        private void UpdateListView()
        {
            if (ProjectsComboBox.SelectedItem is Project selectedProject)
            {
                Debug.WriteLine($"{selectedProject.ProjectID}");

                // Pobranie wybranego projektu
                selectedProject = DatabaseManager.GetProjectById(selectedProject.ProjectID);

                if (selectedProject != null)
                {
                    // Aktualizacja ListView z sesjami pracy
                    //DataListView.ItemsSource = selectedProject.ProjectWorkSessions.Select(WorkSession => new
                    //{
                    //    Date = WorkSession.SessionDate.ToString("d"),
                    //    Time = TimeSpan.FromMilliseconds(WorkSession.SessionTime).ToString(@"h\:mm\:ss")
                    //}).ToList();

                    UpdateTotalTimeDisplay(selectedProject.ProjectID);
                }
            }
            else
            {
                //DataListView.ItemsSource = null;
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

                ProjectsComboBox.IsEnabled = true;

                var newProject = DatabaseManager.GetAllProjects().FirstOrDefault(p => p.ProjectName == enteredName);

                // Ustaw nowo dodany projekt jako wybrany w ComboBox
                if (newProject != null)
                {
                    ProjectsComboBox.SelectedItem = newProject;
                }
            }
        }

        private void DeleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie, czy projekt został wybrany
            if (ProjectsComboBox.SelectedItem is not Project selectedProject)
            {
                System.Windows.MessageBox.Show("Wybierz projekt, który chcesz usunąć.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Przerwij, jeśli projekt nie jest wybrany
            }

            // Potwierdzenie przed usunięciem
            var confirmation = System.Windows.MessageBox.Show(
                $"Czy na pewno chcesz usunąć projekt: {selectedProject.ProjectName}?",
                "Potwierdzenie usunięcia",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirmation == MessageBoxResult.Yes)
            {
                try
                {
                    // Usunięcie projektu

                    Debug.WriteLine($"Projekt do usunięcia: {selectedProject.ProjectID}");

                    DatabaseManager.DeleteProject(selectedProject.ProjectID);

                    ProjectsComboBox.SelectedItem = null;
                    

                    // Załaduj ponownie projekty po usunięciu
                    LoadProjects();
                    UpdateListView();
                    //UpdateTotalTimeDisplay();

                    System.Windows.MessageBox.Show("Projekt został pomyślnie usunięty.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Obsługa błędów
                    System.Windows.MessageBox.Show($"Wystąpił błąd podczas usuwania projektu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void UpdateTotalTimeDisplay(ObjectId? projectId = null)
        {
            if (projectId == null)
            {
                // Jeśli brak projectId, wyczyść TextBlock lub ustaw domyślny tekst
                //TotalTimeTextBlock.Text = "Łączny czas: 00:00:00";
                return;
            }
                //obierz łączny czas z bazy danych
                TimeSpan totalTime = DatabaseManager.GetTotalTimeSpentOnProject(projectId.Value);

                // Zaktualizuj tekst w TextBlock
                //TotalTimeTextBlock.Text = $"Łączny czas w projekcie: {totalTime:h\\:mm\\:ss}";
            
        }

        private void DataListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuBar_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
    



    
