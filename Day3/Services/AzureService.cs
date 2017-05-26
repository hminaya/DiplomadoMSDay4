using System;
using System.Linq;
using Day4.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Prism.Services;

namespace Day4.Services
{
	public class AzureService
	{
		
		public MobileServiceClient MobileService = null;
		private IMobileServiceSyncTable<Customer> customerTable;

		// Change this to point to your Azure backend
		// You can test this using "http://dipxamarin.azurewebsites.net/"
		// 
		private const string MobileUrl = "http://dipxamarin.azurewebsites.net/";

        IPageDialogService _pageDialogService { get; set; }

		public void InitializeAsync()
		{
			MobileService = new MobileServiceClient(MobileUrl);

			var store = new MobileServiceSQLiteStore("day4local1.db");
			store.DefineTable<Customer>();

            MobileService.SyncContext.InitializeAsync(store);
			customerTable = MobileService.GetSyncTable<Customer>();

		}

		public async Task InsertCustomer(Customer cust)
		{

			await customerTable.InsertAsync(cust);

		}

		public async Task<List<Customer>> GetCustomers()
		{

			return (await customerTable.ReadAsync()).ToList();
		}

		public async Task SyncAsync()
		{
			ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

			try
			{
				
				await this.MobileService.SyncContext.PushAsync();

				await this.customerTable.PullAsync("customers",this.customerTable.CreateQuery());

			}
			catch (MobileServicePushFailedException exc)
			{
				Debug.WriteLine(exc.ToString());

				if (exc.PushResult != null)
				{
					syncErrors = exc.PushResult.Errors;

                    foreach (var er in syncErrors)
                    {
                        Debug.WriteLine(er.ToString());
                    }
                }else
				{
					Debug.WriteLine(exc.ToString());
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}

			if (syncErrors != null)
			{
				foreach (var error in syncErrors)
				{
					if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
					{
						//Update failed, reverting to server's copy.
						await error.CancelAndUpdateItemAsync(error.Result);
					}
					else
					{
						// Discard local change.
						await error.CancelAndDiscardItemAsync();
					}

					Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
				}
			}
		}

        static AzureService defaultInstance = new AzureService();
		public static AzureService DefaultManager
		{
			get
			{
                var d = defaultInstance;
                d.InitializeAsync();
				return defaultInstance;
			}
			private set
			{
				defaultInstance = value;
			}
		}

		public MobileServiceClient CurrentClient
		{
			get { return MobileService; }
		}



	}
}
