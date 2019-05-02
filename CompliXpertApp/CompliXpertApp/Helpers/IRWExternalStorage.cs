using System.Threading.Tasks;

namespace CompliXpertApp.Helpers
{
    public interface IRWExternalStorage
    {
        Task<string> ReadFileAsync();
        Task<string> WriteFileAsync(string filePath, string jsonString);
    }
}
