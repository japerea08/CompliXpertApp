using System;
using System.Collections.Generic;
using System.Text;

namespace CompliXpertApp.Helpers
{
    public interface IRWExternalStorage
    {
        bool FolderExists(string folderPath);
        string ReadFile(string filePath);
        string WriteFile(string filePath, string jsonString);
        void CreateFolder();
    }
}
