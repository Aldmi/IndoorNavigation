﻿using System.IO;
using ApplicationCore.App.Models;
using Shiny;
using SQLite;

namespace ApplicationCore.App
{
    public class SampleSqliteConnection : SQLiteAsyncConnection
    {
        public SampleSqliteConnection(IPlatform platform) : base(Path.Combine(platform.AppData.FullName, "sample.db"))
        {
            var conn = GetConnection();
            conn.CreateTable<BeaconEvent>();
            conn.CreateTable<JobLog>();
            conn.CreateTable<BleEvent>();
            conn.CreateTable<VersionChange>();
        }
        
        public AsyncTableQuery<BeaconEvent> BeaconEvents => Table<BeaconEvent>();
        public AsyncTableQuery<BleEvent> BleEvents => Table<BleEvent>();
        public AsyncTableQuery<JobLog> JobLogs => Table<JobLog>();
        public AsyncTableQuery<VersionChange> VersionChanges => Table<VersionChange>();
    }
}
