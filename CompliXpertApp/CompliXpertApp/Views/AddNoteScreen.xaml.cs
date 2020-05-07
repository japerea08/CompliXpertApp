using CompliXpertApp.Helpers;
using CompliXpertApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddNoteScreen : ContentPage
	{
        private AddNoteScreenViewModel addNoteScreenViewModel;
        public AddNoteScreen()
        {
            addNoteScreenViewModel = new AddNoteScreenViewModel();
            InitializeComponent();
            BindingContext = addNoteScreenViewModel;
        }
        public AddNoteScreen (int callreportId, bool callReportCreated)
		{
            addNoteScreenViewModel = new AddNoteScreenViewModel();
			InitializeComponent ();
            BindingContext = addNoteScreenViewModel;
            addNoteScreenViewModel.callreportId = callreportId;
            addNoteScreenViewModel.callReportCreatedAlready = callReportCreated;
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            addNoteScreenViewModel.Subscribe();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //unsubscribe to all here
            addNoteScreenViewModel.UnSubscribe();
        }
    }
}