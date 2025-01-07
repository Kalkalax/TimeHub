using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TiCloud.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowHomeCommand { get; }
        public RelayCommand ShowSettingsCommand { get; }

        public MainWindowViewModel()
        {
            // Ustawienie początkowego widoku
            CurrentView = new TiCloud.MainWindow();

            ShowHomeCommand = new RelayCommand(() => CurrentView = new TiCloud.MainWindow());
            //ShowSettingsCommand = new RelayCommand(() => CurrentView = new SettingsViewModel());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
