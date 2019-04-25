using Xamarin.Forms;
using CompliXpertApp.Helpers;
using Android.OS;
using Android.Widget;
using CompliXpertApp;
using System.IO;
using Android.Support.V4.App;
using Android.Content.PM;
using Android;
using Android.App;
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
        public string WriteFile(string filePath, string jsonString)
        {
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

        public async Task<string> ReadFileAsync()
        {
            string filePath = GetExternalPath() + "/compliXpertcustomers.txt";
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    var json = await reader.ReadToEndAsync();
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

        public Task<string> WriteFileAsync(string filePath, string jsonString)
        {
            throw new System.NotImplementedException();
        }
    }
}