using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
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

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        void SimulateStartup()
        {
            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            //await Task.Delay(8000); // Simulate a bit of startup work.
            // check for permissons
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int) Permission.Granted || ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int) Permission.Granted)
                RequestStoragePermissions();            
            else
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));           
        }

        private  void RequestStoragePermissions()
        {
            ActivityCompat.RequestPermissions(this, PERMISSIONS_STORAGE, REQUEST_STORAGE);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if(requestCode == 1)
            {
                //start the main actib
                //here is when the main activity is started
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
        }
    }
}