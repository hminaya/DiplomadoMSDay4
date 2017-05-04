using Prism.Unity;
using Day3.Views;
using Day3.Services;
using Microsoft.Practices.Unity;

namespace Day3
{
	public partial class App : PrismApplication
	{
		public App(IPlatformInitializer initializer = null) : base(initializer) { }

		protected override void OnInitialized()
		{
			InitializeComponent();

			NavigationService.NavigateAsync("MainPage");
		}

		protected override void RegisterTypes()
		{
			Container.RegisterTypeForNavigation<MainPage>();
			Container.RegisterType<AzureService, AzureService>();
		}
	}
}

