using System.Threading.Tasks;

namespace CompliXpertApp.Helpers
{
    public interface IRWExternalStorage
    {
        Task<string> ReadFileAsync();
        Task WriteFileAsync(string jsonString);
    }
}
