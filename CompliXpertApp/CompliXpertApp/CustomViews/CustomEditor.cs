using Xamarin.Forms;

namespace CompliXpertApp.CustomViews
{
    public class CustomEditor : Editor
    {
        public static readonly BindableProperty CornerRadiusProperty = 
            BindableProperty.Create<CustomEditor, double>(p => p.CornerRadius, 0);

        public static readonly BindableProperty StrokeColorProperty =
            BindableProperty.Create<CustomEditor, Color>(p => p.StrokeColor, Color.Transparent);

        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create<CustomEditor, int>(p => p.StrokeThickness, 0);

        public static readonly BindableProperty BorderEnabledProperty =
            BindableProperty.Create<CustomEditor, bool>(p => p.BorderEnabled, false);


        //properties
        public bool BorderEnabled
        {
            get
            {
                return (bool) GetValue(BorderEnabledProperty);
            }
            set
            {
                SetValue(BorderEnabledProperty, value);
            }
        }
        public double CornerRadius
        {
            get
            {
                return (double) base.GetValue(CornerRadiusProperty);
            }
            set
            {
                base.SetValue(CornerRadiusProperty, value);
            }
        }
        public Color StrokeColor
        {
            get
            {
                return (Color) base.GetValue(StrokeColorProperty);
            }
            set
            {
                SetValue(StrokeColorProperty, value);
            }
        }
        public int StrokeThickness
        {
            get
            {
                return (int) base.GetValue(StrokeThicknessProperty);
            }
            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }
    }
}
