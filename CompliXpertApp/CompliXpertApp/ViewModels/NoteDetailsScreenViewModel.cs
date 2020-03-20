using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class NoteDetailsScreenViewModel: AbstractNotifyPropertyChanged
    {
        private Note note;
        private bool canSaveNote;
        public NoteDetailsScreenViewModel()
        {
            //true for now to build in some capacity of checking info entered by the user
            canSaveNote = true;

            MessagingCenter.Subscribe<NotesListScreenViewModel, Note>(this, Message.NotesLoaded, (sender, note) =>
            {
                Note = note;
            });

            SaveNoteCommand = new Command(async () => await SaveNoteAsync(), () => canSaveNote);
            DeleteNoteCommand = new Command(async () => await DeleteNoteAsync());
        }

        public ICommand SaveNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
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

        //methods
        public async Task SaveNoteAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                //retrieving entity by note id
                Note dbNote = await context.Notes.SingleOrDefaultAsync(n => n.NoteId == note.NoteId);

                if(dbNote != null)
                {
                    dbNote.Subject = note.Subject;
                    dbNote.Description = note.Description;
                    context.Notes.Update(dbNote);
                }

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine(e.InnerException);
                }
                await App.Current.MainPage.Navigation.PopModalAsync();
                MessagingCenter.Send<NoteDetailsScreenViewModel, List<Note>> (this, Message.NotesLoaded, context.Notes.ToList());
            }
        }
        //remove Call Report from local DB
        public async Task DeleteNoteAsync()
        {
            if (await App.Current.MainPage.DisplayAlert("Are you sure you want to delete this Note?", "This action cannot be undone.", "Yes", "Cancel"))
            {
                using (var context = new CompliXperAppContext())
                {
                    var entity = await context.Notes.FirstOrDefaultAsync(x => x.NoteId == Note.NoteId);
                    context.Notes.Remove(entity);
                    await context.SaveChangesAsync();
                    await App.Current.MainPage.Navigation.PopModalAsync();
                    MessagingCenter.Send<NoteDetailsScreenViewModel, List<Note>>(this, Message.NotesLoaded, context.Notes.ToList());
                    //send a message to call report details screen to make a new call 
                }
            }
        }

        void CanSaveNote(bool value)
        {
            canSaveNote = value;
            if (canSaveNote == true)
                ((Command) SaveNoteCommand).ChangeCanExecute();
        }
    }
}
