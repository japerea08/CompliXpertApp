using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonsListScreen : ContentPage
	{
        private PersonsListScreenViewModel personsListScreenViewModel;
		public PersonsListScreen ()
		{
            personsListScreenViewModel = new PersonsListScreenViewModel();
            BindingContext = personsListScreenViewModel;
			InitializeComponent ();
		}
	}
}