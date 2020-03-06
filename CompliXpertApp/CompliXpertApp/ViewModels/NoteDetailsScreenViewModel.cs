using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class NoteDetailsScreenViewModel: AbstractNotifyPropertyChanged
    {
        private Note note;
        public NoteDetailsScreenViewModel()
        {
            MessagingCenter.Subscribe<NotesListScreenViewModel, Note>(this, Message.NotesLoaded, (sender, note) =>
            {
                Note = note;
            });
        }

        public Note Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
                OnPropertyChanged();
            }
        }
    }
}
