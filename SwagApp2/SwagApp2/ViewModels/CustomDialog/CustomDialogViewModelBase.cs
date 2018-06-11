using System.Threading.Tasks;
using Prism.Navigation;

namespace SwagApp2.ViewModels.CustomDialog
{
    public class CustomDialogViewModelBase : ViewModelBase
    {
        public CustomDialogViewModelBase(INavigationService navigationService) : base(navigationService)
        {
        }

        protected TaskCompletionSource<bool> Completion;

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _btnText;
        public string BtnText
        {
            get => _btnText;
            set => SetProperty(ref _btnText, value);
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            Completion = (TaskCompletionSource<bool>)parameters["Completion"];
            Title = (string)parameters["Title"];
            Message = (string)parameters["Message"];
            BtnText = (string)parameters["BtnText"];
        }
    }
}