using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Api.Forms.Infrastructure;
using Libs.Excel;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Xamarin.Forms.PlatformConfiguration;

namespace Api.Forms.Pages.Logging
{
    public class TestLogViewModel : ViewModel
    {
        private readonly IExcelAnalitic _excelAnalitic;

        public TestLogViewModel(IDialogs dialogs, ILogger<TestLogViewModel> logger, IExcelAnalitic excelAnalitic)
        {
            _excelAnalitic = excelAnalitic;
            this.Test = ReactiveCommand.Create<string>(async args =>
            {
                switch (args)
                {
                    case "critical":
                        //DEBUG-------------------
                      
                            UnicodeEncoding uniEncoding = new UnicodeEncoding();
                            String message = "Message NEW";

                        await using(MemoryStream ms = new MemoryStream())
                        {
                            var sw = new StreamWriter(ms, uniEncoding);
                            try
                            {
                                sw.Write(message);
                                sw.Flush(); //otherwise you are risking empty stream
                                ms.Seek(0, SeekOrigin.Begin);

                                //await _excelAnalitic.SaveFile("test.txt", ms);

                                await _excelAnalitic.Write2CsvDoc(
                                    "TrilaterationAnalitic.txt",
                                    "Id;Rssi;Distance;",
                                    new[] {
                                        "111;-77;12,5;",
                                        "125;-76;13,9;"
                                      }, 
                                    true);
                            }
                            catch (Exception e)
                            {
                                    
                            }
                            finally
                            {
                                sw.Dispose();
                            }
                        }
      
                        //DEBUG---------------------------
                        logger.LogCritical("This is a critical test");
                        break;

                    case "error":
                        logger.LogError("This is an error test");
                        break;

                    case "warning":
                        logger.LogWarning("This is an error test");
                        break;

                    case "info":
                        logger.LogInformation("This is an info test");
                        break;

                    case "debug":
                        logger.LogDebug("This is a debug test");
                        break;
                }
                dialogs.Snackbar("Sent log");
            });
        }
        
        
        public ICommand Test { get; }
    }
}
