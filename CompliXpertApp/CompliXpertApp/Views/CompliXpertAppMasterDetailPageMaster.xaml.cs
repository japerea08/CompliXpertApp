using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            ListView = MenuItemsListView;
        }

        class CompliXpertAppMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<CompliXpertAppMasterDetailPageMenuItem> MenuItems { get; set; }

            public CompliXpertAppMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<CompliXpertAppMasterDetailPageMenuItem>(new[]
                {
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 0, Title = "Page 1" },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 1, Title = "Page 2" },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 2, Title = "Page 3" },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 3, Title = "Page 4" },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}