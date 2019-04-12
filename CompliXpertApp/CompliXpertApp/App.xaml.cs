using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CompliXpertApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //constructor sets the MainPage property to a MainPage object type
            MainPage = new NavigationPage (new SplashScreen());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //check to see whether the file directory exists
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
