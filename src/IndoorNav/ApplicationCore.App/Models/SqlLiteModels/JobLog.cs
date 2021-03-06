using System;
using SQLite;

namespace ApplicationCore.App.Models.SqlLiteModels
{
    public class JobLog
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string JobIdentifier { get; set; }
        public string JobType { get; set; }
        public string Error { get; set; }
        public string Parameters { get; set; }
        public bool Started { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
