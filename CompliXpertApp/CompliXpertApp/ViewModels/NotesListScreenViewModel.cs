using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class NotesListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private List<Note> notes;
        private Note selectedNote;
        private bool notesCreated;
        private int crId;
        private bool callReportCreatedAlready = false;
        public NotesListScreenViewModel()
        {
            AddNoteCommand = new Command(async () => await AddNoteAsync());

            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                crId = callReportId;
                callReportCreatedAlready = true;
            });

            //call any DB actions here
            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    Notes = (from notes in context.Notes
                                                      where notes.CallReportId == callReportId
                                                      select notes).ToList();
                    if (Notes.Count == 0)
                        NotesCreated = false;
                    else
                        NotesCreated = true;
                }

            });

            MessagingCenter.Subscribe<NoteDetailsScreenViewModel, List<Note>>(this, Message.NotesLoaded, (sender, notesList) =>
            {
                Notes = notesList;

                if (notesList.Count == 0)
                    NotesCreated = false;
                else
                    NotesCreated = true;
            });
            MessagingCenter.Subscribe<AddNoteScreenViewModel, Note>(this, Message.NoteCreated, (sender, note) =>
            {
                List<Note> dummyList = new List<Note>();

                foreach (Note n in Notes)
                {
                    dummyList.Add(n);
                }

                dummyList.Add(note);
                Notes = dummyList;
                NotesCreated = true;
            });
        }
        public ICommand AddNoteCommand { get; set; }
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
        public bool NotesCreated
        {
            get
            {
                return notesCreated;
            }
            set
            {
                notesCreated = value;
                OnPropertyChanged();
            }
        }
        async private void GetNoteDetailsScreen(Note note)
        {
            SelectedNote = null;
            await App.Current.MainPage.Navigation.PushModalAsync(new NoteDetailsScreen());
            MessagingCenter.Send<NotesListScreenViewModel, Note> (this, Message.NotesLoaded, note);
        }
        async Task AddNoteAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddNoteScreen(crId, callReportCreatedAlready));
        }
    }
}
