using Android.Content;
using Android.Graphics.Drawables;
using CompliXpertApp.Droid;
using CompliXpertApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
//also this custom render needs to be done for iOS as well, like the custom interfaces
[assembly: ExportRenderer(typeof(LoginEntry), typeof (LoginEntryRender))]
namespace CompliXpertApp.Droid
{
    class LoginEntryRender : EntryRenderer
    {
        public LoginEntryRender(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null)
            {
                return;
            }

            UpdateBorders();
        }

        void UpdateBorders()
        {
            GradientDrawable shape = new GradientDrawable();
            shape.SetShape(ShapeType.Rectangle);
            shape.SetCornerRadius(0);

            if (((LoginEntry)this.Element).IsBorderErrorVisible)
            {
                shape.SetStroke(3, ((LoginEntry) this.Element).BorderErrorColor.ToAndroid());
            }
            else
            {
                shape.SetStroke(3, Android.Graphics.Color.LightGray);
                this.Control.SetBackground(shape);
            }
            this.Control.SetBackground(shape);
        }
    }
}