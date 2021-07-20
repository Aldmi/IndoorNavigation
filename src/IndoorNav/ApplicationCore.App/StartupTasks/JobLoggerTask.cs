using System;
using ApplicationCore.App.Models;
using Shiny;
using Shiny.Infrastructure;
using Shiny.Jobs;

namespace ApplicationCore.App.StartupTasks
{
    public class JobLoggerTask : IShinyStartupTask
    {
        private readonly IJobManager _jobManager;
        private readonly SampleSqliteConnection _conn;
        private readonly ISerializer _serializer;


        public JobLoggerTask(IJobManager jobManager,
                             ISerializer serializer,
                             SampleSqliteConnection conn)
        {
            _jobManager = jobManager;
            _serializer = serializer;
            _conn = conn;
        }


        public void Start()
        {
            _jobManager.JobStarted.SubscribeAsync(args => _conn.InsertAsync(new JobLog
            {
                JobIdentifier = args.Identifier,
                JobType = args.Type.FullName,
                Started = true,
                Timestamp = DateTime.Now,
                Parameters = _serializer.Serialize(args.Parameters)
            }));
            _jobManager.JobFinished.SubscribeAsync(args => _conn.InsertAsync(new JobLog
            {
                JobIdentifier = args.Job.Identifier,
                JobType = args.Job.Type.FullName,
                Error = args.Exception?.ToString(),
                Parameters = _serializer.Serialize(args.Job.Parameters),
                Timestamp = DateTime.Now
            }));
        }
    }
}
