using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Day4.Models;
using Day4.Services;
using System.Threading.Tasks;
using Prism.Services;

namespace Day4.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{

		public readonly INavigationService navService;
		AzureService azureService;
        IPageDialogService _pageDialogService;

		public string NewCustomerName { get; set; }
		public string NewCustomerEmail { get; set; }

		public DelegateCommand SaveCustomer { get; set; }
		public DelegateCommand SyncCustomers { get; set; }

		public ObservableCollection<Customer> Customers { get; set; }

		public MainPageViewModel()
		{

			NewCustomerName = "Andres Galvao";
			NewCustomerEmail = "andres@ijjf.com";

			Customers = new ObservableCollection<Customer>(){

				new Customer(){name = "Jose", email = "jose@com"},
				new Customer() { name = "Marcelo Garcia", email = "jose@com"},
				new Customer() { name = "Keenan Cor", email = "jose@com"}
			};

		}

		public MainPageViewModel(INavigationService navigationService, 
                                 IPageDialogService pageDialogService,
                                 AzureService azService)
		{
			navService = navigationService;
            _pageDialogService = pageDialogService;
            azureService = azService;

			SaveCustomer = new DelegateCommand(OnSave);
			SyncCustomers = new DelegateCommand(OnSync);

			
		}

		async void OnSync()
		{
            
			await azureService.SyncAsync();
			await LoadCustomers();

            await _pageDialogService.DisplayAlertAsync("Sync", "Sync process has finished", "Ok");

		}

		async void OnSave()
		{

			Customer cst = new Customer();

			cst.name = NewCustomerName;
			cst.email = NewCustomerEmail;

			await azureService.InsertCustomer(cst);

			await LoadCustomers();

            NewCustomerName = "";
            NewCustomerEmail = "";

		}

		async Task LoadCustomers()
		{
            
			var dt = await azureService.GetCustomers();

			Customers = new ObservableCollection<Customer>(dt);

		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{

			azureService.InitializeAsync();

			await LoadCustomers();

		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{
		}
	}
}