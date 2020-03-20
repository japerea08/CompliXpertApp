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
           
        }
    }
}