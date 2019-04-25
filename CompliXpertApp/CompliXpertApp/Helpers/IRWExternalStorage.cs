using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompliXpertApp.Helpers
{
    public interface IRWExternalStorage
    {
        Task<string> ReadFileAsync();
        Task<string> WriteFileAsync(string filePath, string jsonString);
    }
}
