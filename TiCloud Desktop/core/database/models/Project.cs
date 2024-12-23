using MongoDB.Bson;
using Realms;

namespace TiCloud.Core.Database.Models
{
    /// <summary>
    /// Reprezentuje projekt użytkownika w systemie.
    /// Zawiera informacje o jego nazwie,sesjach pracy powiązanych z projektem
    /// oraz całkowitym czasie spędzonym na projekcie.
    /// </summary>
    public class Project : RealmObject
    {
        /// <summary>
        /// Unikalny identyfikator projektu.
        /// </summary>
        [PrimaryKey]
        public ObjectId ProjectID { get; set; } = ObjectId.GenerateNewId();

        /// <summary>
        /// Data i czas utworzenia projektu.
        /// </summary>
        public DateTimeOffset ProjectCreateDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Nazwa projektu.
        /// </summary>
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// Lista sesji pracy realizowanych na potrzebe projektu.
        /// </summary>
        /// <remarks>
        /// Przechowuje obiekty <see cref="WorkSession"/> reprezentujące pojedyncze sesje pracy w ramach projektu.
        /// </remarks>
        #pragma warning disable CS8618
        public IList<WorkSession> ProjectWorkSessions { get; }
        #pragma warning restore CS8618

        /// <summary>
        /// Całkowity czas pracy dla danego projektu, wyrażony w milisekundach (Unix Timestamp).
        /// </summary>
        /// <remarks>
        /// Wartość ta jest obliczana na podstawie wszystkich sesji z listy <see cref="ProjectWorkSessions"/>.
        /// </remarks>
        public long ProjectTime { get; set; }

        /// <summary>
        /// Oblicza całkowity czas spędzony nad projektem, sumując czas wszystkich sesji pracy.
        /// </summary>
        /// <remarks>
        /// Czas projektu obliczany jest jako suma wartości <see cref="WorkSession.SessionTime"/> 
        /// dla wszystkich sesji pracy znajdujących się w <see cref="ProjectWorkSessions"/>.
        /// </remarks>
        public void CalculateProjectTime()
        {
            ProjectTime = ProjectWorkSessions.Sum(WorkSession => WorkSession.SessionTime);
        }

    }
}
