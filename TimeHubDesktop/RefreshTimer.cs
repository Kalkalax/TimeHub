using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TimeHubDesktop
{   // Klasa odpowiedzialna za aktualizacje widoku czasu trwania sesji
    public class RefreshTimer
    {
        private readonly DispatcherTimer _timer;
        private readonly WorkTimeTracker _workTimeTracker;
        private readonly Action<TimeSpan> _updateAction;

        // Konstruktor, który przyjmuje WorkTimeTracker i akcję do aktualizacji UI
        public RefreshTimer(WorkTimeTracker workTimeTracker, Action<TimeSpan> updateAction)
        {
            _workTimeTracker = workTimeTracker;
            _updateAction = updateAction;

            // Inicjalizacja timer'a do aktualizacji czasu 
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100) // Co pół sekundy
            };
            _timer.Tick += Timer_Tick; // Subskrybujemy zdarzenie Tick
        }

        // Metoda wywoływana przy każdym ticku timera
        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Wywołanie akcji do zaktualizowania UI z bieżącym czasem
            _updateAction(_workTimeTracker.TotalWorkTime);
        }

        public void Start() 
        {
            _timer.Start();
        }
    }
}