using MongoDB.Bson;
using Realms;

namespace TimeHubDesktop.Database.Models
{
    /// <summary>
    /// Reprezentuje okres pracy w systemie.
    /// Zawiera informacje o czasie rozpoczęcia, zakończenia oraz czasie trwania danego okresu pracy.
    /// </summary>
    public partial class WorkPeriod : RealmObject
    {
        /// <summary>
        /// Unikalny identyfikator okresu pracy.
        /// </summary>
        [PrimaryKey]
        public ObjectId PeriodID { get; set; } = ObjectId.GenerateNewId(); 

        /// <summary>
        /// Czas rozpoczęcia okresu pracy w milisekundach (Unix Timestamp).
        /// </summary>
        public long PeriodStartTime { get; set; } 

        /// <summary>
        /// Czas zakończenia okresu pracy w milisekundach (Unix Timestamp).
        /// </summary>
        public long PeriodEndTime { get; set; } 

        /// <summary>
        /// Czas trwania okresu pracy w milisekundach.
        /// </summary>
        public long PeriodTime { get; set; }

        /// <summary>
        /// Metoda obliczająca czas trwania okresu pracy.
        /// </summary>
        /// <remarks>
        /// Czas trwania obliczany jest jako różnica między <see cref="PeriodEndTime"/> a <see cref="PeriodStartTime"/>.
        /// </remarks>
        public void CalculatePeriodTime()
        {
            PeriodTime = PeriodEndTime - PeriodStartTime;
        }
    }
}