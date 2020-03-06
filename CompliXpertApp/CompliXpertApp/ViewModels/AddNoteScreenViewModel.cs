using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AddNoteScreenViewModel
    {
        private bool canSave = false;
        private string subject;
        private string description;
        public AddNoteScreenViewModel()
        {
            SaveNoteCommand = new Command(async () => await SaveNoteAsync(), () => canSave);
        }

        //properties
        public ICommand SaveNoteCommand { get; set; }
        public String Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
                if (String.IsNullOrEmpty(subject) == false || String.IsNullOrWhiteSpace(subject) == false)
                    CanSave(true);
                else
                    CanSave(false);
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                if (String.IsNullOrEmpty(description) == false || String.IsNullOrWhiteSpace(description) == false)
                    CanSave(true);
                else
                    CanSave(false);
            }
        }

        void CanSave(bool value)
        {
            canSave = value;
            ((Command) SaveNoteCommand).ChangeCanExecute();
        }

        //methods
        async Task SaveNoteAsync()
        {
            Note note = new Note()
            {
                Subject = subject,
                Description = description,
                CreatedonMobile = true
            };
            //generate a unique NoteId
            await App.Current.MainPage.Navigation.PopModalAsync();
            ////use messaging center to send note back
            MessagingCenter.Send<AddNoteScreenViewModel, Note>(this, Message.NoteCreated, note);
        }
    }
}
