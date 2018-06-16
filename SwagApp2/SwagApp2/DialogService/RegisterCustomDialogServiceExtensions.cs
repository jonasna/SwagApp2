using Prism.Ioc;
using SwagApp2.DialogService.ToDoListDialogs;
using SwagApp2.ViewModels.CustomDialog;
using SwagApp2.ViewModels.ToDoListModals;
using SwagApp2.Views.CustomDialog;
using SwagApp2.Views.ToDoListModals;

namespace SwagApp2.DialogService
{
    public static class RegisterCustomDialogServiceExtensions
    {
        public static void RegisterCustomDialogService(this IContainerRegistry containerRegistry)
        {
            // services
            containerRegistry.RegisterSingleton<ICustomDialogService, CustomDialogService>();
            containerRegistry.Register<IToDoListDialogService, ToDoListDialogService>();

            // dialogs and viewmodels
            containerRegistry.RegisterForNavigation<InfoDialog, InfoDialogViewModel>();

            // custom modals
            containerRegistry.RegisterForNavigation<NewListModal, NewListModalViewModel>();
            containerRegistry.RegisterForNavigation<NewListItemModal, NewListItemModalViewModel>();
        }
    }
}