using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using SwagApp2.DialogService;
using SwagApp2.Models;

namespace SwagApp2.ViewModels.CustomDialog
{
    public class InfoDialogViewModel : CustomDialogViewModelBase
    {
        private bool _isErrorDialog;
        public bool IsErrorDialog
        {
            get => _isErrorDialog;
            set => SetProperty(ref _isErrorDialog, value);
        }
    
        public DelegateCommand OkCommand => new DelegateCommand(() =>
        {
            Completion.SetResult(true);
        });

        public InfoDialogViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            IsErrorDialog = (bool) parameters["IsError"];
        }
    }           
}