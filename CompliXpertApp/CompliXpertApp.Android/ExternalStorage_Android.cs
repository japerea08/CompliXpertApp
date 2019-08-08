using Xamarin.Forms;
using CompliXpertApp.Helpers;
using Android.OS;
using Android.Widget;
using CompliXpertApp;
using System.IO;
using System.Threading.Tasks;

[assembly: Dependency(typeof(ExternalStorage_Android))]

namespace CompliXpertApp
{
    public class ExternalStorage_Android : IRWExternalStorage
    {
        public string ReadFile(string filePath)
        {
            string fullPath = GetExternalPath() + filePath;

            try
            {
                //get JSON
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    string json = reader.ReadToEnd();
                    return json;
                }

            }
            catch (IOException)
            {
                var context = Android.App.Application.Context;
                string message = "File not accessible";
                Toast.MakeText(context, message, ToastLength.Long).Show();
                return null;
            } 
        }
        public string WriteFile(string jsonString)
        {
            string filePath = "/compliXpertCallReports.text";
            var backingFile = Path.Combine(GetExternalPath(), filePath);
            using (var writer = File.CreateText(backingFile))
            {
                writer.WriteLine(jsonString);
            }
            return null;
        }
        private static string GetExternalPath()
        {
            string path = Environment.ExternalStorageDirectory.AbsolutePath + "/CompliXpert";
            //check to see if the folder exists
            if (Directory.Exists(path) == false)
            {
                var compliXpertFolder = Directory.CreateDirectory(path);
                return compliXpertFolder.FullName;
            }
            return path;
        }
        //read CallReportType
        public async Task<string> GetCallReportTypeAsync()
        {
            string filePath = GetExternalPath() + "/callreporttype.txt";
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync(); 
                }
            }
            catch (IOException)
            {
                var context = Android.App.Application.Context;
                string message = " Call Report Type file not accessible";
                Toast.MakeText(context, message, ToastLength.Long).Show();
                return null;
            }
        }
        public async Task<string> GetCountriesAsync()
        {
            string filePath = GetExternalPath() + "/countries.txt";
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (IOException)
            {
                var context = Android.App.Application.Context;
                string message = " Call Report Questions file not accessible";
                Toast.MakeText(context, message, ToastLength.Long).Show();
                return null;
            }
        }
        //read CallReport Questions
        public async Task<string> GetCallReportQuestionsAsync()
        {
            string filePath = GetExternalPath() + "/questionjson.txt";
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (IOException)
            {
                var context = Android.App.Application.Context;
                string message = " Call Report Questions file not accessible";
                Toast.MakeText(context, message, ToastLength.Long).Show();
                return null;
            }
        }
        public async Task<string> ReadFileAsync()
        {
            string filePath = GetExternalPath() + "/compliXpertcustomers.txt";
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (IOException)
            {
                var context = Android.App.Application.Context;
                string message = "File not accessible";
                Toast.MakeText(context, message, ToastLength.Long).Show();
                return null;
            }
        }

        public async Task WriteFileAsync(string jsonString)
        {
            string filePath = "/compliXpertCallReports.text";
            var backingFile = Path.Combine(GetExternalPath(), filePath);
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(jsonString);
            }
        }
    }
}