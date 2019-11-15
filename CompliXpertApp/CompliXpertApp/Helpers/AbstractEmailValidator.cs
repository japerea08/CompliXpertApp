using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CompliXpertApp.Helpers
{
    class AbstractEmailValidator : INotifyPropertyChanged
    {
        private bool emailValidated;
        private Color emailValidationColor;
        private string emailValidationMessage;
        private bool textEntered;

        public bool EmailValidated
        {
            get
            {
                return emailValidated;
            }
            set
            {
                emailValidated = value;
                //if email is not validated
                if (value == false)
                {
                    EmailValidationMessage = "Email format is not correct";
                    EmailValidationColor = Color.Red;
                }
                else
                {
                    EmailValidationMessage = "Email format looks correct";
                    EmailValidationColor = Color.Green;
                }
                OnPropertyChanged();
            }
        }
        public Color EmailValidationColor
        {
            get
            {
                return emailValidationColor;
            }
            set
            {
                emailValidationColor = value;
                OnPropertyChanged();
            }
        }
        public string EmailValidationMessage
        {
            get
            {
                //if nothing has been entered into the email entry
                return emailValidationMessage;
            }
            set
            {
                emailValidationMessage = value;
                OnPropertyChanged();
            }
        }
        public bool TextEntered
        {
            get
            {
                return textEntered;
            }
            set
            {
                textEntered = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
