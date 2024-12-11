using MongoDB.Bson;
using Realms;

namespace TimeHubDesktop.Database.Models
{
    /// <summary>
    /// Reprezentuje sesję pracy użytkownika w systemie.
    /// Zawierającą informacje o okresach pracy, dacie oraz sumarycznym czasie pracy.
    /// </summary>
    public class WorkSession : RealmObject
    {
        /// <summary>
        /// Unikalny identyfikator sesji pracy.
        /// </summary>
        [PrimaryKey] 
        public ObjectId SessionID { get; set; } = ObjectId.GenerateNewId();

        /// <summary>
        /// Data i czas utworzenia sesji pracy.
        /// </summary>
        public DateTimeOffset SessionDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Lista okresów pracy składających się na sesję.
        /// </summary>
        /// <remarks>
        /// Przechowuje obiekty <see cref="WorkPeriod"/> reprezentujące pojedyncze okresy pracy w ramach sesji.
        /// </remarks>
        #pragma warning disable CS8618
        public IList<WorkPeriod> SessionWorkPeriods { get; }
        #pragma warning restore CS8618

        /// <summary>
        /// Całkowity czas pracy dla danej sesji, wyrażony w milisekundach (Unix Timestamp).
        /// </summary>
        /// <remarks>
        /// Wartość ta jest obliczana na podstawie okresów pracy z listy <see cref="SessionWorkPeriods"/>.
        /// </remarks>
        public long SessionTime { get; set; }

        /// <summary>
        /// Oblicza całkowity czas pracy dla sesji na podstawie sumy czasów trwania poszczególnych okresów pracy.
        /// </summary>
        public void CalculateSessionTime() 
        { 
            SessionTime = SessionWorkPeriods.Sum(WorkPeriod => WorkPeriod.PeriodTime);
        }
    }
}