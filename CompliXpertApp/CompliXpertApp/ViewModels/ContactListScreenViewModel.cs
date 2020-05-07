using CompliXpertApp.Models;
using System.Collections.Generic;
using CompliXpertApp.Helpers;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using CompliXpertApp.Views;

namespace CompliXpertApp.ViewModels
{
    class ContactListScreenViewModel: AbstractNotifyPropertyChanged
    {
        private List<NewContact> contactsList;
        private NewContact contactSelected;
        private bool contactsCreated;
        private bool isBusy;
        public ContactListScreenViewModel()
        {
        }

        //properties
        public bool ContactsCreated
        {
            get
            {
                return contactsCreated;
            }
            set
            {
                contactsCreated = value;
                OnPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public List<NewContact> ContactsList
        {
            get
            {
                return contactsList;
            }
            set
            {
                contactsList = value;
                OnPropertyChanged();
            }
        }
        public NewContact ContactSelected
        {
            get
            {
                return contactSelected;
            }
            set
            {
                contactSelected = value;
                if (contactSelected == null)
                    return;
                GetContactMasterAsync(contactSelected);
                OnPropertyChanged();
            }
        }

        private async void GetContactMasterAsync(NewContact contact)
        {
            ContactSelected = null;
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new NewContactMaster()) });
            MessagingCenter.Send<ContactListScreenViewModel, NewContact>(this, Message.NewContactLoaded, contact);
        }

        public async void InitializeData()
        {
            IsBusy = true;
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                ContactsList = await context.NewContacts.ToListAsync();
                if (contactsList.Count > 0)
                    ContactsCreated = true;
                else
                    ContactsCreated = false;
            }
            IsBusy = false;
        }
    }
}
