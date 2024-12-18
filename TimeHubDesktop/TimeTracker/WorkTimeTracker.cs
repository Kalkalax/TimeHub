using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Zweryfikować, napisać summary, dodać do UML

namespace TimeHubDesktop
{
    // Klasa odpowiedzialna za śledzenie czasu pracy
    public class WorkTimeTracker
    {

        private readonly Stopwatch _timer; // Pomiar czasu pracy

        public event Action<TimeSpan> TimeUpdated = delegate { }; // Zdarzenie wywoływane gdy zmienia się stan 

        // Konstruktor klasy
        public WorkTimeTracker()
        {
            _timer = new Stopwatch();
        }

        // Właściwość sprawdzająca czy czas jest mierzony
        public bool IsTracking => _timer.IsRunning;

        // Metoda rozpoczynająca pomiar czasu
        public void StartTracking()
        {
            if (!_timer.IsRunning)
            {
                _timer.Start();
            }
        }

        // Metoda zatrzymująca pomiar czasu
        public void StopTracking()
        {
            if (_timer.IsRunning)
            {
                _timer.Stop();
            }
        }

        // Metoda resetująca czas pracy
        public void ResetTracking()
        {
            _timer.Reset();
        }

        // Właściwość zwracająca łączny czas pracy
        public TimeSpan TotalWorkTime => _timer.Elapsed;


    }
}
