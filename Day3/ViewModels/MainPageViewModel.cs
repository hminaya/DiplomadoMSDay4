using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Day3.Models;
using Day3.Services;
using System.Threading.Tasks;

namespace Day3.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{
		
		public readonly INavigationService navService;

		public string NewCustomerName { get; set;}
		public string NewCustomerEmail { get; set;}

		public DelegateCommand SaveCustomer { get; set;}
		public DelegateCommand SyncCustomers { get; set;}

		public ObservableCollection<Customer> Customers { get; set;}

		public MainPageViewModel(){
			
			NewCustomerName = "Leandro Lo";
			NewCustomerEmail = "leandro@ibjjf.com";

			Customers = new ObservableCollection<Customer>() { 
				new Customer() { name = "Andres Galvao", email= "andres@ibjjf.com"},
				new Customer() { name = "Keenan Cornelius", email = "andres@ibjjf.com"},
				new Customer() { name = "Marcelo Garcia", email = "marcelo@ibjjf.com"}
			};
		}

		public MainPageViewModel(INavigationService navigationService)
		{
			navService = navigationService;

			SaveCustomer = new DelegateCommand(OnSave);
			SyncCustomers = new DelegateCommand(OnSync);
		}


		async void OnSync()
		{

		}

		async void OnSave()
		{

		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{
		}
	}
}