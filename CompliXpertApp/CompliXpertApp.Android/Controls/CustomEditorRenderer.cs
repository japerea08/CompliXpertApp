using CompliXpertApp.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(CompliXpertApp.CustomViews.CustomEditor), typeof(CompliXpertApp.Droid.Controls.CustomEditorRenderer))]
namespace CompliXpertApp.Droid.Controls
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomEditorRenderer : EditorRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var customEditor = (CustomEditor) Element;

                if (customEditor.BorderEnabled == true)
                {
                    GradientDrawable gradient = new GradientDrawable();
                    gradient.SetCornerRadius((float)customEditor.CornerRadius);
                    gradient.SetStroke(customEditor.StrokeThickness, customEditor.StrokeColor.ToAndroid());
                    Control.Background = gradient;
                }
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}