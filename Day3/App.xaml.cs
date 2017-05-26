using Prism.Unity;
using Day4.Views;
using Day4.Services;
using Microsoft.Practices.Unity;

namespace Day4
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

