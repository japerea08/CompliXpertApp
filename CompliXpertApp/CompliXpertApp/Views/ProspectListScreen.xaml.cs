using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CompliXpertApp.ViewModels;
namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProspectListScreen : ContentPage
	{
        private ProspectListScreenViewModel prospectListScreenViewModel;
		public ProspectListScreen ()
		{
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            prospectListScreenViewModel = new ProspectListScreenViewModel();
            BindingContext = prospectListScreenViewModel;
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            prospectListScreenViewModel.InitializeData();
            base.OnAppearing();
        }
    }
}