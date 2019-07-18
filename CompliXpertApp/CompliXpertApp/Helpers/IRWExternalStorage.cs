using System.Threading.Tasks;

namespace CompliXpertApp.Helpers
{
    public interface IRWExternalStorage
    {
        //read CallReportType
        Task<string> GetCallReportTypeAsync();
        //read CallReport Questions
        Task<string> GetCallReportQuestionsAsync();
        //read the customers
        Task<string> ReadFileAsync();
        Task WriteFileAsync(string jsonString);
    }
}
