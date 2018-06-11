using Prism.Ioc;
using SwagApp2.ViewModels.CustomDialog;
using SwagApp2.Views.CustomDialog;

namespace SwagApp2.DialogService
{
    public static class RegisterCustomDialogServiceExtensions
    {
        public static void RegisterCustomDialogService(this IContainerRegistry containerRegistry)
        {
            // service
            containerRegistry.RegisterSingleton<ICustomDialogService, CustomDialogService>();

            // dialogs and viewmodels
            containerRegistry.RegisterForNavigation<InfoDialog, InfoDialogViewModel>();
        }
    }
}