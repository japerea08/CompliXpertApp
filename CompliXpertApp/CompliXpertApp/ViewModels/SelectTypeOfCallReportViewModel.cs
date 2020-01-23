using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class SelectTypeOfCallReportViewModel: SelectAccountforCallReportViewModel
    {
        public SelectTypeOfCallReportViewModel()
        {
            //subscribe to messages from CustomerMasterViewModel
            MessagingCenter.Subscribe<CustomerMasterViewModel, Account>(this, Message.AccountLoaded, (sender, account) =>
            {
                SelectedAccount = account;
            });

        }

        //methods
        public async void InitializeData()
        {
            using (var context = new CompliXperAppContext())
            {
                //get all callreport types
                CallReportTypes = await context.CallReportType.ToListAsync();
            }
        }

        override public async Task CreateCallReportAsync()
        {
            if (CallReportTypeSelected != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CreateCallReportScreen()) });

                MessagingCenter.Send<SelectTypeOfCallReportViewModel, CallReportType>(this, Message.CallReportTypeLoaded, CallReportTypeSelected);

                MessagingCenter.Send<SelectTypeOfCallReportViewModel, Account>(this, Message.AccountLoaded, SelectedAccount);
                SelectedAccount = null;
            }
        }

    }
}
