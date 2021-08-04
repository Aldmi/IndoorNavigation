using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Java.IO;
using System.Threading.Tasks;
using Java.Security;
using File = System.IO.File;

namespace Libs.Excel
{
    public class ExcelAnalitic : IExcelAnalitic
    {
        public ExcelAnalitic()
        {
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="csvLines"></param>
        /// <param name="reWriteFile"></param>
        /// <returns></returns>
        /// <exception cref="AccessControlException"></exception>
        public async Task Write2CsvDoc(string header, string[] csvLines, bool reWriteFile)
        {
            var fileName = "TrilaterationAnalitic.txt";
            if (!Android.OS.Environment.IsExternalStorageEmulated)
            {
                throw new AccessControlException("Доступ к внешнему хранилищу '/storage/emulated/0' Запрещен");
            }
            var root = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "Documents");
            var path= Path.Combine(root, fileName);
            
            if (reWriteFile)
            {
                var lines = new List<string> {header};
                lines.AddRange(csvLines);
                await File.WriteAllLinesAsync(path, lines);
            }
            else
            {    
                await File.AppendAllLinesAsync(path, csvLines);
            }
        }
        
        
        
        public async Task SaveFile(string fileName, MemoryStream stream)
        {
            string root = null;
            //Get the root path in Android device.
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "Documents");
            }
            else
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
            
            //bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);
            
            //Create directory and file 
            Java.IO.File myDir = new Java.IO.File(root + "/IndorNav");
            myDir.Mkdir();
            Java.IO.File file = new Java.IO.File(myDir, fileName);
        
            //Remove if the file exists
            if (file.Exists()) file.Delete();
            FileOutputStream outs = new FileOutputStream(file);
            await outs.WriteAsync(stream.ToArray());
            await outs.FlushAsync();
            outs.Close();
        }
        
        
        
        
        
        // public async Task SaveCountAsync(int count)
        // {
        //     var g = "/storage/emulated/0/Documents";
        //     var path = Path.Combine(g, "count.txt");
        //     using (var writer = File.CreateText(path))
        //     {
        //         await writer.WriteLineAsync(count.ToString());
        //     }
        // }
    }
}