using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System.Collections.ObjectModel;

namespace CompliXpertApp.ViewModels
{
    class CompliXpertAppMasterDetailPageMasterViewModel: AbstractNotifyPropertyChanged
    {
        public ObservableCollection<CompliXpertAppMasterDetailPageMenuItem> MenuItems { get; set; }

        public CompliXpertAppMasterDetailPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<CompliXpertAppMasterDetailPageMenuItem>(new[]
            {
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 0, Title = "Customer List" },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 1, Title = "Create Call Report" },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 2, Title = "Calendar" },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 3, Title = "History" }
                });
        }
    }
}
