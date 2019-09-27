using CompliXpertApp.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Content;

[assembly: ExportRenderer(typeof(CompliXpertApp.CustomViews.CustomEntry), typeof(CompliXpertApp.Droid.Controls.CustomEntryRenderer))]
namespace CompliXpertApp.Droid.Controls
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context):base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var customEntry = (CustomEntry) Element;

                if (customEntry.BorderEnabled == true)
                {
                    GradientDrawable gradient = new GradientDrawable();
                    gradient.SetCornerRadius((float) customEntry.CornerRadius);
                    gradient.SetStroke(customEntry.StrokeThickness, customEntry.StrokeColor.ToAndroid());
                    Control.Background = gradient;
                }
            }
        }
    }
}