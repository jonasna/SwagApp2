using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using SwagApp2.Models;

namespace SwagApp2.ViewModels.ToDoListModals
{
    public class NewListModalViewModel : ViewModelBase
    {
        private TaskCompletionSource<ToDoList> _completionSource;
        public NewListModalViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "New List";
            OkBtnText = "Ok";
            CancelBtnText = "Cancel";
        }

        private string _listName;
        public string ListName
        {
            get => _listName;
            set => SetProperty(ref _listName, value);
        }

        private string _okBtnText;
        public string OkBtnText
        {
            get => _okBtnText;
            set => SetProperty(ref _okBtnText, value);
        }

        private string _cancelBtnText;
        public string CancelBtnText
        {
            get => _cancelBtnText;
            set => SetProperty(ref _cancelBtnText, value);
        }

        public DelegateCommand CreateCommand => new DelegateCommand(
            () =>
            {
                _completionSource.SetResult(new ToDoList(ListName));
            },
            () => !string.IsNullOrEmpty(ListName)).ObservesProperty(() => ListName);

        public DelegateCommand CancelCommand => new DelegateCommand(() => _completionSource.SetResult(null));

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            _completionSource = (TaskCompletionSource<ToDoList>) parameters["Completion"];
        }
    }
}
