using System.Diagnostics;
using Prism;
using Prism.Ioc;
using SwagApp2.ViewModels;
using SwagApp2.Views;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using Prism.Plugin.Popups;
using SwagApp2.DataStores;
using SwagApp2.ViewModels.CustomDialog;
using SwagApp2.DialogService;
using SwagApp2.Views.CustomDialog;
using Unity;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SwagApp2
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        private IUnityContainer _container;

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var aus = _container.Resolve<IApplicationUserService>();

            // For debugging
            if (Debugger.IsAttached)
            {
                aus.DisplayName = "SwagBoy";
                aus.Name = "Jonas";
            }

            if (aus.DisplayName == null)
                await NavigationService.NavigateAsync("MasterDetailPageView/BaseNavigationPageView/UserSettingsPage");
            else
                await NavigationService.NavigateAsync("MasterDetailPageView/BaseNavigationPageView/ListPageView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _container = containerRegistry.GetContainer();

            // Popup init
            containerRegistry.RegisterPopupNavigationService();

            // Services
            containerRegistry.RegisterSingleton<IListStore, FakeListStore>();
            containerRegistry.RegisterSingleton<IApplicationUserService, ApplicationUserService>();
            containerRegistry.RegisterCustomDialogService();

            // Navigation
            containerRegistry.RegisterForNavigation<BaseNavigationPageView, BaseNavigationPageViewModel>();

            // Views and viewmodels
            containerRegistry.RegisterForNavigation<MasterDetailPageView, MasterDetailPageViewModel>(); // Master Detail
            containerRegistry.RegisterForNavigation<ListPageView, ListPageViewModel>(); // List Page    
            containerRegistry.RegisterForNavigation<SingleListPage, SingleListPageViewModel>(); // Single List Edit Page
            containerRegistry.RegisterForNavigation<UserSettingsPage, UserSettingsPageViewModel>(); // Settings
        }
    }
}
