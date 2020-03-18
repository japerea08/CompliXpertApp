using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotesListScreen : ContentPage
	{
        private NotesListScreenViewModel notesListScreenViewModel;
		public NotesListScreen ()
		{
            notesListScreenViewModel = new NotesListScreenViewModel();
            BindingContext = notesListScreenViewModel;
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //call any DB actions here
            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    notesListScreenViewModel.Notes = (from notes in context.Notes
                                                      where notes.CallReportId == callReportId
                                                      select notes).ToList();
                    if (notesListScreenViewModel.Notes.Count == 0)
                        notesListScreenViewModel.NotesCreated = false;
                    else
                        notesListScreenViewModel.NotesCreated = true;
                }
                    
            });

            MessagingCenter.Subscribe<NoteDetailsScreenViewModel, List<Note>>(this, Message.NotesLoaded, (sender, notesList) =>
            {
               notesListScreenViewModel.Notes = notesList;

                if (notesList.Count == 0)
                    notesListScreenViewModel.NotesCreated = false;
                else
                    notesListScreenViewModel.NotesCreated = true;
            });
        }
    }
}