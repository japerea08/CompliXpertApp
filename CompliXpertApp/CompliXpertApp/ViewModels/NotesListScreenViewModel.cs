using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class NotesListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private List<Note> notes;
        private Note selectedNote;
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
        public Note SelectedNote
        {
            get
            {
                return selectedNote;
            }
            set
            {
                selectedNote = value;
                if (selectedNote == null)
                    return;
                GetNoteDetailsScreen(selectedNote);
            }
        }

        async private void GetNoteDetailsScreen(Note note)
        {
            SelectedNote = null;
            await App.Current.MainPage.Navigation.PushModalAsync(new NoteDetailsScreen());
            MessagingCenter.Send<NotesListScreenViewModel, Note> (this, Message.NotesLoaded, note);
        }
    }
}
