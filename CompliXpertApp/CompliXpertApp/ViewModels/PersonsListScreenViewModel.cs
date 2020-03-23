using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class PersonsListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private List<Person> persons;
        private Person selectedPerson;
        private bool personsCreated;
        private int crId;
        private bool callReportCreatedAlready = false;

        public PersonsListScreenViewModel()
        {
            AddPersonCommand = new Command(async () => await AddPersonAsync());

            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                crId = callReportId;
                callReportCreatedAlready = true;
            });

            //call any DB actions here
            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    Persons = (from persons in context.Persons
                             where persons.CallReportId == callReportId
                             select persons).ToList();
                    if (Persons.Count == 0)
                        PersonsCreated = false;
                    else
                        PersonsCreated = true;
                }

            });

            MessagingCenter.Subscribe<PersonDetailsScreenViewModel, List<Person>>(this, Message.PersonsLoaded, (sender, personsList) =>
            {
                Persons = personsList;

                if (personsList.Count == 0)
                    PersonsCreated = false;
                else
                    PersonsCreated = true;
            });

            MessagingCenter.Subscribe<AddPersonScreenViewModel, Person>(this, Message.PersonCreated, (sender, person) =>
            {
                List<Person> dummyList = new List<Person>();

                foreach (Person p in Persons)
                {
                    dummyList.Add(p);
                }

                dummyList.Add(person);
                Persons = dummyList;
                PersonsCreated = true;
            });
        }
        //properties
        public ICommand AddPersonCommand { get; set; }
        public List<Person> Persons
        {
            get
            {
                return persons;
            }
            set
            {
                persons = value;
                OnPropertyChanged();
            }
        }
        public bool PersonsCreated
        {
            get
            {
                return personsCreated;
            }
            set
            {
                personsCreated = value;
                OnPropertyChanged();
            }
        }
        public Person SelectedPerson
        {
            get
            {
                return selectedPerson;
            }
            set
            {
                selectedPerson = value;
                if (selectedPerson == null)
                    return;
                GetPersonDetailsScreen(selectedPerson);
            }
        }

        //methods
        async private void GetPersonDetailsScreen(Person person)
        {
            SelectedPerson = null;
            await App.Current.MainPage.Navigation.PushModalAsync(new PersonDetailsScreen());
            MessagingCenter.Send<PersonsListScreenViewModel, Person>(this, Message.PersonLoaded, person);
        }
        async Task AddPersonAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddPersonScreen(crId, callReportCreatedAlready));
        }
    }
}
