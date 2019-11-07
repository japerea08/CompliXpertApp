using System;
using System.Threading.Tasks;
using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AddNewContactScreenViewModel : AbstractNotifyPropertyChanged
    {
        private bool canAdd = false;
        private string contactName;

        public AddNewContactScreenViewModel()
        {
            NewContact = new NewContact();
            AddNewContactCommand = new Command(async () => await AddNewContactAsync(), () => canAdd);
        }

        //properties
        public NewContact NewContact { get; set; }
        public string ContactName
        {
            get
            {
                return contactName;
            }
            set
            {
                contactName = value;
                if (String.IsNullOrEmpty(ContactName) == false && String.IsNullOrWhiteSpace(ContactName) == false)
                    CanAdd(true);
                else
                    CanAdd(false);
            }
        }
        public Command AddNewContactCommand { get; private set; }
        //methods
        void CanAdd(bool value)
        {
            canAdd = value;
            ((Command) AddNewContactCommand).ChangeCanExecute();
        }
        private async Task AddNewContactAsync()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                if(NewContact != null || ContactName != null)
                {
                    NewContact.Name = contactName;
                    context.Add(NewContact);
                    try
                    {
                        await context.SaveChangesAsync();
                        App.Current.MainPage = new NavigationPage(new CompliXpertAppMasterDetailPage());
                        await App.Current.MainPage.Navigation.PopToRootAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        SqliteException ex = (SqliteException) e.InnerException;
                        Console.WriteLine(ex.SqliteErrorCode);
                        await App.Current.MainPage.Navigation.PopToRootAsync();
                        DependencyService.Get<IToast>().WriteToast(e.InnerException.Message);
                    }
                }
            }
        }

    }
}
