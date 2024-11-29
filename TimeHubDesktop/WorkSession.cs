using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TimeHubDesktop
{
    public class WorkSession : RealmObject
    {
        
        [PrimaryKey] // Identyfikator sesji

        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Data sesji
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;


        //public RealmList<WorkPeriod> WorkPeriods { get; }   // używamy RealmList
        public IList<WorkPeriod> WorkPeriods { get; }


        // Wyliczana wartość czasu pracy na podstawie okresów pracy
        public TimeSpan WorkTime => WorkPeriods.Aggregate(TimeSpan.Zero, (total, period) => total + (period.PeriodEnd - period.PeriodStart));
    }
}