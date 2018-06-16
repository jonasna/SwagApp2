using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SwagApp2.DataStores;
using SwagApp2.DialogService;
using SwagApp2.DialogService.ToDoListDialogs;
using SwagApp2.Models;
using Xamarin.Forms;

namespace SwagApp2.ViewModels
{
    public class ListPageViewModel : ViewModelBase
    {
        private readonly IListStore _listStore;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICustomDialogService _dialogService;
        private readonly IToDoListDialogService _toDoListService;

        private readonly bool _isValid;

        private ToDoList _selectedList;
        public ToDoList SelectedList
        {
            get => _selectedList;
            set => SetProperty(ref _selectedList, value);
        }

        private ObservableCollection<ToDoList> _listCollection;
        public ObservableCollection<ToDoList> ListCollection
        {
            get => _listCollection;
            set => SetProperty(ref _listCollection, value);
        }

        private int _listHeight;
        public int ListHeight
        {
            get => _listHeight;
            set => SetProperty(ref _listHeight, value);
        }

        private int _listRowHeight;
        public int ListRowHeight
        {
            get => _listRowHeight;
            set => SetProperty(ref _listRowHeight, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        
        public ListPageViewModel(INavigationService navigationService,
            IListStore listStore,
            IApplicationUserService applicationUserService,
            ICustomDialogService dialogService,
            IToDoListDialogService toDoListService)
            : base(navigationService)
        {
            _listStore = listStore;
            _applicationUserService = applicationUserService;
            _dialogService = dialogService;
            _toDoListService = toDoListService;

            if (_applicationUserService.DisplayName == null || _applicationUserService.Name == null)
            {
                _isValid = false;
                Message = "Please configure settings";
            }
            else
            {
                _isValid = true;
                Message = $"Here are your lists, {_applicationUserService.Name}";
            }
            
            ListRowHeight = 50;
            SelectedList = null;
            Title = "List Collection";
        }

        public DelegateCommand NewListCommand => new DelegateCommand(NewListCommandExecuted, () => _isValid);

        private async void NewListCommandExecuted()
        {
            var newList = await _toDoListService.ShowCreateToDoListModalAsync();
            if (newList != null)
            {
                newList.Owner = _applicationUserService.DisplayName;
                if (await _listStore.CreateListAsync(newList) == null)
                    await _dialogService.ShowErrorDialogAsync("Error", "Could not create list!", "Ok");
                else
                {
                    ListCollection.Insert(0, newList);
                    ListHeight = ListCollection.Count * ListRowHeight;
                }                                       
            }           
        }

        public DelegateCommand EditListCommand => new DelegateCommand(UpdateListCommandExecuted, () => SelectedList != null && _isValid )
                .ObservesProperty(() => SelectedList);

        private async void UpdateListCommandExecuted()
        {
            await NavigationService.NavigateAsync(
                new Uri("SingleListPage", UriKind.Relative),
                new NavigationParameters { {"List", _selectedList} } );
        }

        public DelegateCommand DeleteListCommand => new DelegateCommand(DeleteListCommandExecuted, () => SelectedList != null && _isValid )
               .ObservesProperty(() => SelectedList);

        private async void DeleteListCommandExecuted()
        {
            await _listStore.DeleteListAsync(_selectedList.Name);
            ListCollection = new ObservableCollection<ToDoList>(await _listStore.GetAllListsAsync());
            ListHeight = ListCollection.Count * ListRowHeight;
            SelectedList = null;
        }

        #region Navigation

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (ListCollection == null)
            {
                ListCollection = new ObservableCollection<ToDoList>(await _listStore.GetAllListsAsync());
                ListHeight = ListCollection.Count * ListRowHeight;
            }
        }

        #endregion

    }
}