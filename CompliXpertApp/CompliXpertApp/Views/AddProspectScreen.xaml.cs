using Android.Widget;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddProspectScreen : ContentPage
	{
        private AddProspectScreenViewModel addProspectScreenViewModel;
		public AddProspectScreen ()
		{
            addProspectScreenViewModel = new AddProspectScreenViewModel();
            InitializeComponent ();
            BindingContext = addProspectScreenViewModel;
		}
	}
}