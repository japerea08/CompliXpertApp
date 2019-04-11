using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;

namespace CompliXpertApp.Droid
{
    [Activity(Label = "CompliXpertApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly int REQUEST_STORAGE = 1;
        static string[] PERMISSIONS_STORAGE = {Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage};
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //check for permissons
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted || ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int) Permission.Granted)
            {
                RequestStoragePermissions();
            }

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        private void RequestStoragePermissions()
        {
            ActivityCompat.RequestPermissions(this, PERMISSIONS_STORAGE, REQUEST_STORAGE);
        }
    }
}