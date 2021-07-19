using System;
using SQLite;

namespace ApplicationCore.App.Models
{
    public class VersionChange
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Area { get; set; }
        public string Old { get; set; }
        public string New { get; set; }

        public DateTime Date { get; set; }
    }
}
