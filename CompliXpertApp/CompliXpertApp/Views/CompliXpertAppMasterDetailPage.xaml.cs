using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompliXpertAppMasterDetailPage : MasterDetailPage
    {
        
        private CompliXpertAppMasterDetailPageMasterViewModel compliXpertAppMasterDetailPageMasterViewModel;
        public CompliXpertAppMasterDetailPage()
        {
            compliXpertAppMasterDetailPageMasterViewModel = new CompliXpertAppMasterDetailPageMasterViewModel();
            InitializeComponent();
            BindingContext = compliXpertAppMasterDetailPageMasterViewModel;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as CompliXpertAppMasterDetailPageMenuItem;
            if (item == null)
                return;

            var page = (Page) Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            compliXpertAppMasterDetailPageMasterViewModel.InitializeData();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            this.IsPresented = false;
            base.OnDisappearing();
            compliXpertAppMasterDetailPageMasterViewModel.RebuildOriginalMenu();
        }
    }
}