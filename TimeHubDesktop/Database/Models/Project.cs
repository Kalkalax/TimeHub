using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Opis, usunąć ostrzerzenia i błędy

namespace TimeHubDesktop.Database.Models
{
    public class Project : RealmObject
    {
        [PrimaryKey]
        public ObjectId ProjectID { get; set; } = ObjectId.GenerateNewId();

        public string ProjectName { get; set; } = string.Empty;

        public IList<WorkSession> ProjectWorkSessions { get; }

        public long ProjectTime { get; set; }

        public void CalculateProjectTime()
        {
            ProjectTime = ProjectWorkSessions.Sum(WorkSession => WorkSession.SessionTime);
        }

    }
}
