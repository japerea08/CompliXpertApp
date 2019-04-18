using System.ComponentModel;

namespace CompliXpertApp.Models
{
    public class User: INotifyPropertyChanged
    {
        public Field UserName { get; set; }
        public Field Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
