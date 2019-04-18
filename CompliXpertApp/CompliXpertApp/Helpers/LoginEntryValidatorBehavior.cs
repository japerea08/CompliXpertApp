using Xamarin.Forms;

namespace CompliXpertApp.Helpers
{
    class LoginEntryValidatorBehavior : Behavior<LoginEntry>
    {
        LoginEntry control;
        string _placeHolder;
        Color _placeHolderColor;

        protected override void OnAttachedTo(LoginEntry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            bindable.PropertyChanged += Bindable_PropertyChanged;
            control = bindable;
            _placeHolder = bindable.Placeholder;
            _placeHolderColor = bindable.PlaceholderColor;
        }

        private void Bindable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == LoginEntry.IsBorderErrorVisibleProperty.PropertyName && control != null)
            {
                if (control.IsBorderErrorVisible)
                {
                    control.Placeholder = control.ErrorText;
                    control.PlaceholderColor = control.BorderErrorColor;
                    control.Text = string.Empty;
                }
            }
            else
            {
                control.Placeholder = _placeHolder;
                control.PlaceholderColor = _placeHolderColor;
            }
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue) == false)
                ((LoginEntry) sender).IsBorderErrorVisible = false;
        }

        protected override void OnDetachingFrom(LoginEntry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            bindable.PropertyChanged -= Bindable_PropertyChanged;
        }
    }
}
