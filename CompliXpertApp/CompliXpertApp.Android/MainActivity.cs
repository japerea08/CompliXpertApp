using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Linq;
using CompliXpertApp.Views;
using System;
using Xamarin.Forms;

namespace CompliXpertApp.Droid
{
    [Activity(Label = "CompliXpertApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            //Code to make window scrollable when keyboard is present
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            //subscribe to get the orientation for the page
            MessagingCenter.Subscribe<AddProspectScreen>(this, Helpers.Message.AllowLandscapePortrait, sender => 
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });
            MessagingCenter.Subscribe<AddProspectScreen>(this, Helpers.Message.PreventLandscape, sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
            MessagingCenter.Subscribe<CreateCallReportScreen>(this, Helpers.Message.AllowLandscapePortrait, sender =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });
            MessagingCenter.Subscribe<CreateCallReportScreen>(this, Helpers.Message.PreventLandscape, sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
            MessagingCenter.Subscribe<CallReportDetailsScreen>(this, Helpers.Message.AllowLandscapePortrait, sender =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });
            MessagingCenter.Subscribe<CallReportDetailsScreen>(this, Helpers.Message.PreventLandscape, sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == 16908332)
            {
                //get the current page
                try
                {
                    var currentpage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
                    if (currentpage.GetType().FullName.Contains("CreateCallReportScreen"))
                    {
                        var newcurrentpage = (CreateCallReportScreen) currentpage;
                        //check to see if we have a backbar action
                        if (newcurrentpage?.CustomNavBackButton != null)
                        {
                            //invoke the action to do
                            newcurrentpage.CustomNavBackButton.Invoke();
                            return false;
                        }
                    }
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine(e.Message);
                }

                return base.OnOptionsItemSelected(item);
            }
            else
                return base.OnOptionsItemSelected(item);
        }
    }
}