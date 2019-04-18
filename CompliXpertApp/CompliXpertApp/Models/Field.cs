using System.ComponentModel;

namespace CompliXpertApp.Models
{
    public class Field : INotifyPropertyChanging
    {
        public string Name { get; set; }
        public bool IsNotValid { get; set; }
        public string NotValidMessageError { get; set; }
        public event PropertyChangingEventHandler PropertyChanging;
    }
}
