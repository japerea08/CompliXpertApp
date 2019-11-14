
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using System.Windows.Input;

namespace CompliXpertApp.Behaviors
{
    class EmailValidatorBehavior : Behavior<Entry>
    {
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";


       public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EmailValidatorBehavior), null);

        //properties
        public ICommand Command
        {
            get
            {
                return (ICommand) GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            if (bindable.BindingContext != null)
                BindingContext = bindable.BindingContext;

            bindable.BindingContextChanged += Bindable_BindingContextChanged;

            bindable.TextChanged += HandleTextChanged;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();

            if (!(sender is BindableObject bindable))
                return;

            BindingContext = bindable.BindingContext;
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(e.NewTextValue, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)) == false)
            {
                ((Entry) sender).TextColor = Color.Red;

                Command?.Execute(false);
            }
            else
            {
                ((Entry) sender).TextColor = Color.Default;

                Command?.Execute(true);
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
            bindable.TextChanged -= HandleTextChanged;

        }
    }
}
