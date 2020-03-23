
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonDetailsScreen : ContentPage
	{
        private PersonDetailsScreenViewModel personDetailsScreenViewModel;
		public PersonDetailsScreen ()
		{
            personDetailsScreenViewModel = new PersonDetailsScreenViewModel();
            BindingContext = personDetailsScreenViewModel;
            InitializeComponent ();
		}
    }
}