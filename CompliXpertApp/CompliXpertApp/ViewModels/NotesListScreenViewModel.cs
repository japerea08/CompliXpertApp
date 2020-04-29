using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.EntityFrameworkCore;
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
        private bool createdOnMobile;
        private bool notesCreated;
        private int crId;
        private bool callReportCreatedAlready;
        private bool isBusy;
        public NotesListScreenViewModel()
        {
            callReportCreatedAlready = false;
            AddNoteCommand = new Command(async () => await AddNoteAsync());
            Subscribe();
        }
        public ICommand AddNoteCommand { get; set; }
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
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
        public bool CreatedOnMobile
        {
            get
            {
                return createdOnMobile;
            }
            set
            {
                createdOnMobile = value;
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
        private void Subscribe()
        {
            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                crId = callReportId;
                callReportCreatedAlready = true;
            });

            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, async (sender, callReportId) =>
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    IsBusy = true;
                    Notes = await (from notes in context.Notes
                             where notes.CallReportId == callReportId
                             select notes).ToListAsync();
                    //get callreport the notes are asscoiated with
                    CreatedOnMobile = await (from c in context.CallReport
                                             where c.CallReportId == callReportId
                                             select c.CreatedOnMobile).FirstOrDefaultAsync();
                    if (Notes.Count == 0)
                        NotesCreated = false;
                    else
                        NotesCreated = true;
                    IsBusy = false;
                    
                        
                }

            });

            MessagingCenter.Subscribe<NoteDetailsScreenViewModel, List<Note>>(this, Message.NotesLoaded, (sender, notesList) =>
            {
                Notes = notesList;

                if (notesList.Count == 0)
                    NotesCreated = false;
                else
                {
                    NotesCreated = true;
                    CreatedOnMobile = notesList[0].CreatedonMobile;
                }                
                    
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
                if (Notes.Count == 0)
                    NotesCreated = false;
                else
                    NotesCreated = true;
            });
        }

        public void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId);

            MessagingCenter.Unsubscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId);

            MessagingCenter.Unsubscribe<NoteDetailsScreenViewModel, List<Note>>(this, Message.NotesLoaded);

            MessagingCenter.Unsubscribe<AddNoteScreenViewModel, Note>(this, Message.NoteCreated);
        }
    }
}
