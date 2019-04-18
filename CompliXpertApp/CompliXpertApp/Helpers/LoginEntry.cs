using Xamarin.Forms;

namespace CompliXpertApp.Helpers
{
    public class LoginEntry : Entry
    {
        //bindable properties to for UI
        public static readonly BindableProperty BorderErrorColorProperty = BindableProperty.Create(nameof(BorderErrorColor), typeof(Color), typeof(LoginEntry), Color.Transparent, BindingMode.TwoWay);

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(LoginEntry), string.Empty);

        public static readonly BindableProperty IsBorderErrorVisibleProperty = BindableProperty.Create(nameof(IsBorderErrorVisible), typeof(bool), typeof(LoginEntry), false, BindingMode.TwoWay);

        //properties
        public string ErrorText
        {
            get
            {
                return (string) GetValue(ErrorTextProperty);
            }
            set
            {
                SetValue(ErrorTextProperty, value);
            }
        }
        public Color BorderErrorColor
        {
            get
            {
                return (Color) GetValue(BorderErrorColorProperty);
            }
            set
            {
                SetValue(BorderErrorColorProperty, value);
            }
        }
        public bool IsBorderErrorVisible
        {
            get
            {
                return (bool) GetValue(IsBorderErrorVisibleProperty);
            }
            set
            {
                SetValue(IsBorderErrorVisibleProperty, value);
            }
        }
        
    }
}
