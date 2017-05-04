using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Day3.Models;

namespace Day3.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{
		
		public readonly INavigationService navService;

		public string NewCustomerName { get; set;}
		public string NewCustomerEmail { get; set;}

		public DelegateCommand SaveCustomer { get; set;}

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
		}

		void OnSave()
		{
			
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			
		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{
		}
	}
}