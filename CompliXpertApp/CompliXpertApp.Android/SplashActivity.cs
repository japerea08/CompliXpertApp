using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using System;
using System.Threading.Tasks;

namespace CompliXpertApp.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        static readonly int REQUEST_STORAGE = 1;
        static string[] PERMISSIONS_STORAGE = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        //requesting runtime permissions here because we want it asked only when the application first starts up
        protected override void OnStart()
        {
            base.OnStart();
            //ask for the runtime permissions here...
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int) Permission.Granted && ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int) Permission.Granted)
            {
                //we have permission, go ahead with the app
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
            else
            {
                // storage permission is not granted. If necessary display rationale & request.
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage) || ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReadExternalStorage))
                {
                    var view = FindViewById(Android.Resource.Id.Content);

                    Snackbar.Make(view, "CompliXpert App Needs To Access File Storage", Snackbar.LengthIndefinite).SetAction("Ok", new Action<View>(delegate(View obj) {
                        ActivityCompat.RequestPermissions(this, PERMISSIONS_STORAGE, REQUEST_STORAGE);
                    })).Show();
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, PERMISSIONS_STORAGE, REQUEST_STORAGE);
                }
            }
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();

        }

        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        //void SimulateStartup()
        //{
        //    Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
        //    //await Task.Delay(8000); // Simulate a bit of startup work.
        //    // check for permissons
        //    if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int) Permission.Granted || ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int) Permission.Granted)
  
        //    else
        //        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            //means the user made a selection 
            if (requestCode == 1)
            {
                //check to see if the permission has been granted
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    //start the activity
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                }
                else
                {
                    var view = FindViewById(Android.Resource.Id.Content);
                    //display a snackbar indicating
                    Snackbar.Make(view, "You Must Allow CompliXpert Access to Files", Snackbar.LengthIndefinite).SetAction("Ok", new Action<View>(delegate (View obj) {
                        var activity = (Activity) this;
                        activity.FinishAffinity();
                        base.OnStop();
                    })).Show();
                }
            }
        }
    }
}