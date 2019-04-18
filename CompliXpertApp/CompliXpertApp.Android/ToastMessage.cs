using CompliXpertApp;
using Xamarin.Forms;
using Android.Widget;
using CompliXpertApp.Helpers;

[assembly: Dependency(typeof(ToastMessage))]

namespace CompliXpertApp
{
    public class ToastMessage: IToast
    {
        public void WriteToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();            
        }
    }
}