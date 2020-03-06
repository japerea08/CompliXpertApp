using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoteDetailsScreen : ContentPage
	{
        private NoteDetailsScreenViewModel noteDetailsScreenViewModel;
		public NoteDetailsScreen ()
		{
            noteDetailsScreenViewModel = new NoteDetailsScreenViewModel();
			InitializeComponent ();
            BindingContext = noteDetailsScreenViewModel;
		}
	}
}