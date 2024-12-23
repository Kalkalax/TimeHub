using System.Diagnostics;

namespace TiCloud.Core.Timers
{
    /// <summary>
    /// Klasa odpowiedzialna za śledzenie czasu pracy użytkownika.
    /// Umożliwia rozpoczęcie, zatrzymanie oraz resetowanie pomiaru czasu pracy.
    /// Zdarzenie <see cref="TimeUpdated"/> jest wywoływane przy każdej zmianie czasu pracy.
    /// </summary>
    public class WorkTimeTracker
    {
        /// <summary>
        /// Obiekt typu <see cref="Stopwatch"/> odpowiedzialny za pomiar czasu pracy.
        /// </summary>
        private readonly Stopwatch _timer;

        /// <summary>
        /// Zdarzenie wywoływane, gdy zmienia się stan czasu pracy.
        /// </summary>
        public event Action<TimeSpan> TimeUpdated = delegate { };

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="WorkTimeTracker"/> i rozpoczyna pomiar czasu.
        /// </summary>
        public WorkTimeTracker()
        {
            _timer = new Stopwatch();
        }

        /// <summary>
        /// Właściwość informująca, czy czas pracy jest aktualnie mierzony.
        /// </summary>
        public bool IsTracking => _timer.IsRunning;

        /// <summary>
        /// Rozpoczyna pomiar czasu, jeśli nie jest już uruchomiony.
        /// </summary>
        public void StartTracking()
        {
            if (!_timer.IsRunning)
            {
                _timer.Start();
            }
        }

        /// <summary>
        /// Zatrzymuje pomiar czasu, jeśli jest aktywny.
        /// </summary>
        public void StopTracking()
        {
            if (_timer.IsRunning)
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// Resetuje pomiar czasu pracy.
        /// </summary>
        public void ResetTracking()
        {
            _timer.Reset();
        }

        /// <summary>
        /// Zwraca łączny czas pracy od momentu rozpoczęcia pomiaru.
        /// </summary>
        public TimeSpan TotalWorkTime => _timer.Elapsed;
    }
}
