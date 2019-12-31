using CompliXpertApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompliXpertAppMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;
        private CompliXpertAppMasterDetailPageMasterViewModel compliXpertAppMasterDetailPageMasterViewModel;
        public CompliXpertAppMasterDetailPageMaster()
        {
            compliXpertAppMasterDetailPageMasterViewModel = new CompliXpertAppMasterDetailPageMasterViewModel();
            InitializeComponent();
            BindingContext = compliXpertAppMasterDetailPageMasterViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            compliXpertAppMasterDetailPageMasterViewModel.InitializeData();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}