using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Navigation;
using SwagApp2.DataStores;
using SwagApp2.DialogService;
using SwagApp2.DialogService.ToDoListDialogs;
using SwagApp2.Models;

namespace SwagApp2.ViewModels
{
	public class SingleListPageViewModel : ViewModelBase
	{
	    private ToDoList _currentList;
	    private readonly IListStore _listStore;
	    private readonly ICustomDialogService _dialogService;
	    private readonly IToDoListDialogService _toDoListDialogService;

	    private ObservableCollection<ListItem> _listItems;
	    public ObservableCollection<ListItem> ListItems
	    {
	        get => _listItems;
	        set => SetProperty(ref _listItems, value);
	    }

	    private string _created;
	    public string Created
	    {
	        get => _created;
	        set => SetProperty(ref _created, value);
	    }

	    private string _owner;
	    public string Owner
	    {
	        get => _owner;
	        set => SetProperty(ref _owner, value);
	    }

	    private ListItem _selectedItem;
	    public ListItem SelectedItem
	    {
	        get => _selectedItem;
	        set => SetProperty(ref _selectedItem, value);
	    }

	    public SingleListPageViewModel(INavigationService navigationService,
	                                   IListStore listStore,
	                                   ICustomDialogService dialogService,
	                                   IToDoListDialogService toDoListService) : base(navigationService)
	    {
	        _toDoListDialogService = toDoListService;
	        _dialogService = dialogService;
	        _listStore = listStore;
	        SelectedItem = null;
	    }

	    public DelegateCommand AddItemCommand => new DelegateCommand(AddItemCommandOnExecuted);

	    private async void AddItemCommandOnExecuted()
	    {
	        var newItem = await _toDoListDialogService.ShowCreateListItemModalAsync();
	        if (newItem != null)
	        {
                if (_currentList.AddItem(newItem))
	            {
                    ListItems.Add(newItem);
	                SelectedItem = newItem;
	            }
	            else
	            {
	                await _dialogService.ShowErrorDialogAsync("Error", "Could not create item!", "Ok");
	                SelectedItem = null;
	            }
	        }
	    }

        public DelegateCommand DeleteItemCommand => 
            new DelegateCommand(DeleteItemCommandOnExecuted, () => SelectedItem != null)
                .ObservesProperty(() => SelectedItem);

	    private void DeleteItemCommandOnExecuted()
	    {
	        if (_currentList.RemoveItem(_selectedItem.Name))
	        {
	            var index = ListItems.IndexOf(_selectedItem);
	            if (index >= 0)
	            {
	                ListItems.RemoveAt(index);
	                SelectedItem = null;
	            }
            }
	    }

        public DelegateCommand SaveItemCommand => new DelegateCommand(SaveCommandOnExecuted);
	    private async void SaveCommandOnExecuted()
	    {
	        if (await UpdateListAsync())
	            await _dialogService.ShowInfoDialogAsync("Succes", "Your changes have been saved", "Ok");
	        else
	            await _dialogService.ShowErrorDialogAsync("Error", "Could not save changes", "Ok");

	        SelectedItem = null;
	    }

	    private async Task<bool> UpdateListAsync()
	    {
	        var result = await _listStore.UpdateListAsync(_currentList.Name, _currentList);
	        if (result != null)
	            return true;
	        return false;
	    }

	    #region Navigation

	    public override async void OnNavigatingTo(INavigationParameters parameters)
	    {
	        if (parameters.GetNavigationMode() == NavigationMode.New)
	        {
	            await Populate(parameters);
	        }
	    }

	    private Task Populate(INavigationParameters navParams)
	    {
	        _currentList = (ToDoList)navParams["List"];

	        Title = _currentList.Name;
	        ListItems = new ObservableCollection<ListItem>(_currentList.ListItems);
	        Created = _currentList.Created.ToShortDateString();
	        Owner = _currentList.Owner;

	        return Task.CompletedTask;
	    }

	    #endregion
    }
}
