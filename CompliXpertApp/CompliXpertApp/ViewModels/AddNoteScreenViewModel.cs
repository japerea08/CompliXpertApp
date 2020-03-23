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
        public int callreportId;
        public bool callReportCreatedAlready = false;
        public AddNoteScreenViewModel()
        {
            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                callreportId = callReportId;
                callReportCreatedAlready = true;
            });

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
                CreatedDate = DateTime.Now,
                CreatedonMobile = true
            };

            if(callReportCreatedAlready == true)
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    note.CallReportId = callreportId;

                    context.Notes.Add(note);

                    try
                    {
                        await context.SaveChangesAsync();
                        await App.Current.MainPage.Navigation.PopModalAsync();
                        //message being set back to the Notes List Screen to update it
                        MessagingCenter.Send<AddNoteScreenViewModel, Note>(this, Message.NoteCreated, note);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                await App.Current.MainPage.Navigation.PopModalAsync();
                ////use messaging center to send note back
                MessagingCenter.Send<AddNoteScreenViewModel, Note>(this, Message.NoteCreated, note);
            }
            

        }
    }
}
