using CompliXpertApp.Helpers;
using System.Threading.Tasks;
using CompliXpertApp.Models;
using System.Linq;
using CompliXpertApp.Views;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class LoadingScreenViewModel : AbstractNotifyPropertyChanged
    {
        private bool isBusy = false;
        static HttpClient client;
        public  LoadingScreenViewModel()
        {
            
            client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            client.BaseAddress = new Uri("https://complixperlite2019.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            IsBusy = true;
        }

        //properties
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        //methods
        public async Task DBContainsRecords()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                //IsBusy = true;
                if(context.Customer.Any() == true)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage());
                    //App.Current.MainPage = new NavigationPage(new CompliXpertAppMasterDetailPage());
                    IsBusy = false;
                }
                else
                {
                    //make a call to the Api
                    bool result = await PopulateSqliteDb();
                    if (result == true)
                    {
                        //the Customer List screen is set as the root page.
                        await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage());
                        //App.Current.MainPage = new NavigationPage(new CompliXpertAppMasterDetailPage());
                        IsBusy = false;
                    }
                    
                }
            }

        }

        public async Task<bool> PopulateSqliteDb()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                Task<Account[]> accounts = GetAccountsAsync();
                Task<ProductCode[]> productCodes = GetProductCodesAsync();
                Task<AccountClass[]> accountClasses = GetAccountClassesAsync();
                Task<CallReport[]> callReports = GetCallReportsAsync();
                Task<CallReportQuestions[]> callReportQuestions = GetCallReportQuestionsAsync();
                Task<CallReportType[]> callReportTypes = GetCallReportTypesAsync();
                Task<Country[]> countries = GetCountriesAsync();
                Task<Customer[]> customers = GetCustomersAsync();
                Task<IndustryType[]> industryTypes = GetIndustryTypesAsync();
                Task<LinesofBusiness[]> linesofBusinesses = GetLinesofBusinessAsync();
                Task<CallReportResponse[]> responses = GetCallReportResponsesAsync();

                //awaiting each task
                try
                {
                    context.AddRange(await accounts);
                    context.AddRange(await productCodes);
                    context.AddRange(await accountClasses);
                    context.AddRange(await callReports);
                    context.AddRange(await callReportQuestions);
                    context.AddRange(await callReportTypes);
                    context.AddRange(await countries);
                    context.AddRange(await customers);
                    context.AddRange(await industryTypes);
                    context.AddRange(await linesofBusinesses);
                    context.AddRange(await responses);
                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert("Something went wrong.", "It appears you are not connected to the internet.", "OK");
                    await App.Current.MainPage.Navigation.PopAsync();
                    return false;
                }
                try
                {
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    return false;
                }
            }

        }

        public async Task<CallReportResponse[]> GetCallReportResponsesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/CallReportResponses");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<CallReportResponse[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }

        public async Task<ProductCode[]> GetProductCodesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/productcodes");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ProductCode[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
                
        }
        
        public async Task<Account[]> GetAccountsAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/accounts");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Account[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<AccountClass[]> GetAccountClassesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/accountclasses");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<AccountClass[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<CallReport[]> GetCallReportsAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/callreports");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<CallReport[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<CallReportQuestions[]> GetCallReportQuestionsAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/callreportquestions");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<CallReportQuestions[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<CallReportType[]> GetCallReportTypesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/callreporttypes");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<CallReportType[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<Country[]> GetCountriesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/countries");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Country[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<Customer[]> GetCustomersAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/customers");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Customer[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<IndustryType[]> GetIndustryTypesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/industrytypes");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<IndustryType[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
        public async Task<LinesofBusiness[]> GetLinesofBusinessAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/linesofbusinesses");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<LinesofBusiness[]>();
            else
            {
                await App.Current.MainPage.DisplayAlert("Looks like something went wrong.", "The server is not responding.", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                return null;
            }
        }
    }
}
