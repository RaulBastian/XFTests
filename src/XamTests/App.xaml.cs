using Prism.Ioc;
using Prism.Unity;
using Serilog;
using System;
using System.IO;
using Xamarin.Essentials;
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

            if(Directory.Exists(Path.Combine(FileSystem.CacheDirectory, "logs")))
            {
                Directory.Delete(Path.Combine(FileSystem.CacheDirectory, "logs"),true);
            }

            Log.Logger = new LoggerConfiguration()
                 .WriteTo.File(path: Path.Combine(FileSystem.CacheDirectory, "logs", "log.txt"), rollingInterval: RollingInterval.Day)
                 .CreateLogger();


            Log.Information("Starting...");
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
            containerRegistry.RegisterForNavigation<LogsPage, LogsViewModel>();

            containerRegistry.Register<IService1, RepositoryService1>();
            containerRegistry.Register<IService2, RepositoryService2>();

            containerRegistry.RegisterForNavigation<DependencyInjectionTestPage,DependencyInjectionTestViewModel>();
        }

        protected override void OnInitialized()
        {
            this.NavigationService.NavigateAsync("MainPage");
        }
    }
}
