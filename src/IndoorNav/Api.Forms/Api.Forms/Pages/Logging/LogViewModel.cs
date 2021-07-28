using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Forms.Infrastructure;
using Prism.Navigation;
using ReactiveUI;
using Shiny.Infrastructure;
using Shiny.Integrations.Sqlite;

namespace Api.Forms.Pages.Logging
{
    public class LogViewModel : AbstractLogViewModel<CommandItem>
    {
        private readonly ShinySqliteConnection conn;
        private readonly ISerializer serializer;
        private readonly INavigationService navigator;


        public LogViewModel(ShinySqliteConnection conn,
                            ISerializer serializer,
                            IDialogs dialogs,
                            INavigationService navigator) : base(dialogs)
        {
            this.conn = conn;
            this.serializer = serializer;
            this.navigator = navigator;
        }


        //protected override Task ClearLogs() => conn.Logs.DeleteAsync(x => x.IsError);
        protected override Task ClearLogs() => conn.Logs.DeleteAsync(x=>true);
        protected override async Task<IEnumerable<CommandItem>> LoadLogs()
        {
            // var results = await conn
            //     .Logs
            //     .Where(x => x.IsError)
            //     .OrderByDescending(x => x.TimestampUtc)
            //     .ToListAsync();
            
            var results = await conn
                .Logs
                //.Where(x => x.IsError)
                .OrderByDescending(x => x.TimestampUtc)
                .ToListAsync();

            return results.Select(x => ToItem(
                x.TimestampUtc,
                x.Description,
                _ = x.Parameters
            ));
        }


        private CommandItem ToItem(DateTime date, string exception, string value)
        {
            var title = date.ToLocalTime().ToString("MMM dd, hh:dd:ss tt");
            return new CommandItem
            {
                Text = title,
                Detail = exception,
                PrimaryCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    var s = $"{title}{Environment.NewLine}{exception}{Environment.NewLine}{value}";
                    //var parameters = getParameters();

                    //if (parameters != null && parameters.Any())
                    //    foreach (var p in parameters)
                    //        s += $"{Environment.NewLine}{p.Key}: {p.Value}";

                    await navigator.ShowBigText(s, title);
                })
            };
        }
    }
}
