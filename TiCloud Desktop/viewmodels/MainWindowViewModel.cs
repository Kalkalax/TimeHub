using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using TiCloud;
using TiCloud_Desktop.views.content;

namespace TiCloud_Desktop.viewmodels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        

        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        public RelayCommand ShowHomeCommand { get; }
        public RelayCommand ShowDatabaseCommand { get; }




        public MainWindowViewModel()
        {
            // Domyślny widok
            CurrentView = new HomeView();
            //CurrentView = new DatabaseView();

            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowDatabaseCommand = new RelayCommand(ExecuteShowDatabase);
        }

        private void ExecuteShowHome()
        {
            Debug.WriteLine("Home button clicked");
            CurrentView = new HomeView(); // Zmiana widoku na Home
        }

        private void ExecuteShowDatabase()
        {
            Debug.WriteLine("Database button clicked");
            CurrentView = new DatabaseView(); // Zmiana widoku na Database
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
