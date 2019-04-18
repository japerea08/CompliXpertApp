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

[assembly: Dependency(typeof(ExternalStorage_Android))]

namespace CompliXpertApp
{
    public class ExternalStorage_Android : IRWExternalStorage
    {
        static readonly int REQUEST_STORAGE = 1;
        static string[] PERMISSIONS_STORAGE = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };

        public void RequestPermissions()
        {

        }
        public bool isPermissionSet()
        {
            if (ActivityCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.ReadExternalStorage) != (int) Permission.Granted || ActivityCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage) != (int) Permission.Granted)
            {
                return false;
            }
            return true;
        }
        public bool FolderExists()
        {
            if (Directory.Exists(Environment.ExternalStorageDirectory.AbsolutePath + "/CompliXpert") == false)
            {
                return false;
            }

            return true;
        }

        public void CreateFolder()
        {
            Directory.CreateDirectory(Environment.ExternalStorageDirectory.AbsolutePath + "/CompliXpert");
        }

        //returns the text from file as a string
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
    }
}