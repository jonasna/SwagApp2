using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using SwagApp2.Models;

namespace SwagApp2.ViewModels.ToDoListModals
{
    public class NewListItemModalViewModel : ViewModelBase
    {
        private TaskCompletionSource<ListItem> _completionSource;

        public NewListItemModalViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Add Item";
            OkBtnText = "Ok";
            CancelBtnText = "Cancel";
        }

        private string _itemName;
        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        private string _itemDescription;
        public string ItemDescription
        {
            get => _itemDescription;
            set => SetProperty(ref _itemDescription, value);
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
                _completionSource.SetResult(new ListItem{Name = ItemName, Description = ItemDescription});
            },
            () => !string.IsNullOrEmpty(ItemName)).ObservesProperty(() => ItemName);

        public DelegateCommand CancelCommand => new DelegateCommand(() => _completionSource.SetResult(null));

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            _completionSource = (TaskCompletionSource<ListItem>)parameters["Completion"];
        }
    }
}