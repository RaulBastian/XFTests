using Prism.Ioc;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamTests.ViewModels;
using XamTests.Views;

namespace XamTests
{
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage,MainViewModel>();
            containerRegistry.RegisterForNavigation<ConnectivityPage, ConnectivityViewModel>();
            containerRegistry.RegisterForNavigation<FoldersPage, FoldersViewModel>();
        }

        protected override void OnInitialized()
        {
            this.NavigationService.NavigateAsync("MainPage");
        }
    }
}
