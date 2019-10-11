using CompliXpertApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompliXpertAppMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public CompliXpertAppMasterDetailPageMaster()
        {
            InitializeComponent();
            BindingContext = new CompliXpertAppMasterDetailPageMasterViewModel();
        }
    }
}