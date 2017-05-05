using System;
using System.Linq;
using Day3.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Day3.Services
{
	public class AzureService
	{
		
		public MobileServiceClient MobileService = null;
		private IMobileServiceSyncTable<Customer> customerTable;

		// Change this to point to your Azure backend
		// You can test this using "https://diplomadoday3test.azurewebsites.net"
		// 
		private const string MobileUrl = "AzureURL";

		public async Task InitializeAsync()
		{
			MobileService = new MobileServiceClient(MobileUrl);

			var store = new MobileServiceSQLiteStore("local.db");
			store.DefineTable<Customer>();

			await MobileService.SyncContext.InitializeAsync(store);
			customerTable = MobileService.GetSyncTable<Customer>();

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

	}
}
