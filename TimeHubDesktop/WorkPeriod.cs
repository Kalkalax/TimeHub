using Realms;
using System;

namespace TimeHubDesktop
{

    public partial class WorkPeriod : RealmObject
    {
        [PrimaryKey]
        
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Unikalny identyfikator

        public DateTimeOffset PeriodStart { get; set; } // Data rozpoczęcia okresu pracy

        public DateTimeOffset PeriodEnd { get; set; } // Data zakończenia okresu pracy (może być null)

        [Ignored]// Nie przechowujemy tego w bazie, obliczamy automatycznie
        public TimeSpan PeriodTime => PeriodEnd - PeriodStart; // Automatyczne obliczanie czasu trwania
    }
}