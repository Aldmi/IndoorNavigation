using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Libs.Excel
{
    public interface IExcelAnalitic
    {
        Task SaveFile(string fileName, MemoryStream stream);

        Task Write2CsvDoc(string header, string[] csvLines, bool reWriteFile);
    }
}