using System;
using SQLite;

namespace ApplicationCore.App.Models.SqlLiteModels
{
    public class BleEvent
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
