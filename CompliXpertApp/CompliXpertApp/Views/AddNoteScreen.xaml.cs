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
            MessagingCenter.Subscribe<CreateCallReportViewModel, int>(this, Message.CallReportLoaded, (sender,callReportId) =>
            {
                addNoteScreenViewModel.callreportId = callReportId;
            });
            //MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            //{
            //    addNoteScreenViewModel.callreportId = callReportId;
            //    addNoteScreenViewModel.callReportCreatedAlready = true;
            //});
        }
    }
}