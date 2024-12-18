using System.Windows.Threading;

namespace TimeHubDesktop.Core.Timers

{   /// <summary>
    /// Klasa odpowiedzialna za odświeżanie widoku czasu trwania sesji pracy.
    /// </summary>
    public class RefreshTimer
    {
        /// <summary>
        /// Obiekt <see cref="DispatcherTimer"/> odpowiedzialny za cykliczne wywoływanie akcji aktualizacji.
        /// </summary>
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// Instancja <see cref="WorkTimeTracker"/> przechowująca bieżące informacje o całkowitym czasie pracy.
        /// </summary>
        private readonly WorkTimeTracker _workTimeTracker;

        /// <summary>
        /// Delegat <see cref="Action{TimeSpan}"/> definiujący akcję do wykonania podczas aktualizacji interfejsu użytkownika.
        /// </summary>
        private readonly Action<TimeSpan> _updateAction;

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="RefreshTimer"/>.
        /// </summary>
        /// <param name="workTimeTracker">
        /// Obiekt <see cref="WorkTimeTracker"/> zawierający bieżące informacje o czasie pracy.
        /// </param>
        /// <param name="updateAction">
        /// Delegat <see cref="Action{T}"/> odpowiedzialny za aktualizację interfejsu użytkownika 
        /// na podstawie czasu pracy.
        /// </param>
        public RefreshTimer(WorkTimeTracker workTimeTracker, Action<TimeSpan> updateAction)
        {
            _workTimeTracker = workTimeTracker;
            _updateAction = updateAction;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Obsługuje zdarzenie <see cref="DispatcherTimer.Tick"/>.
        /// Wywołuje delegat do aktualizacji interfejsu użytkownika na podstawie bieżącego czasu pracy.
        /// </summary>
        /// <param name="sender">Źródło zdarzenia.</param>
        /// <param name="e">Dodatkowe dane zdarzenia.</param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            _updateAction(_workTimeTracker.TotalWorkTime);
        }

        /// <summary>
        /// Rozpoczyna działanie timera, uruchamiając cykliczne aktualizacje.
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }
    }
}