using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class NotesListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private List<Note> notes;
        public NotesListScreenViewModel()
        {
            MessagingCenter.Subscribe<CallReportDetailsViewModel, ICollection<Note>>(this, Message.NotesLoaded, (sender, notesList) => 
            {
                Notes = notesList.ToList();
            });
        }
        public List<Note> Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
                OnPropertyChanged();
            }
        }
    }
}
