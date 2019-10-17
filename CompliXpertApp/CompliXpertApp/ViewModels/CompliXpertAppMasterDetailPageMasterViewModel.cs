using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CompliXpertAppMasterDetailPageMasterViewModel: AbstractNotifyPropertyChanged
    {
        private CompliXpertAppMasterDetailPageMenuItem _menuItem; 
        
        public CompliXpertAppMasterDetailPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<CompliXpertAppMasterDetailPageMenuItem>(new[]
            {
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 0, Title = "Customer List", ImageSource = "white_home.png", TargetType = typeof(CustomerListScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 1, Title = "Create Call Report", ImageSource = null, TargetType = typeof(CreateCallReportScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 2, Title = "Calendar", ImageSource = null },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 3, Title = "History", ImageSource = null }
                });
        }

        //properties
        public ObservableCollection<CompliXpertAppMasterDetailPageMenuItem> MenuItems { get; set; }
        public CompliXpertAppMasterDetailPageMenuItem MenuItem
        {
            get
            {
                return _menuItem;
            }
            set
            {
                _menuItem = value;
                if (MenuItem == null)
                    return;
                CallScreen(MenuItem);
                OnPropertyChanged();
            }
        }

        private async void CallScreen(CompliXpertAppMasterDetailPageMenuItem menuItem)
        {
            var page = (Page) Activator.CreateInstance(menuItem.TargetType);
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(page) });
        }
    }
}
