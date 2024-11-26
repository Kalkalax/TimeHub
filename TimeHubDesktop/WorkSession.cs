using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeHubDesktop
{
    public class WorkSession : RealmObject
    {
        [PrimaryKey] // Identyfikator sesji
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTimeOffset Date { get; set; } // Data sesji

        // Zmieniamy na RealmList<WorkPeriod> dla kompatybilności z Realm
        //public IList<WorkPeriod>? WorkPeriods { get; set; } // Lista okresów pracy w tej sesji

        // Wyliczana wartość czasu pracy na podstawie okresów pracy
        //public TimeSpan WorkTime => WorkPeriods.Aggregate(TimeSpan.Zero, (total, period) => total + (period.PeriodEnd - period.PeriodStart));
    }
}