using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Navigation;
using SwagApp2.DataStores;
using SwagApp2.DialogService;
using SwagApp2.Models;

namespace SwagApp2.ViewModels
{
	public class SingleListPageViewModel : ViewModelBase
	{
	    private ToDoList _currentList;
	    private readonly IListStore _listStore;
	    private readonly ICustomDialogService _dialogService;

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

	    public SingleListPageViewModel(INavigationService navigationService, IListStore listStore, ICustomDialogService dialogService) : base(navigationService)
	    {
	        _dialogService = dialogService;
	        _listStore = listStore;
	        SelectedItem = null;
	    }

	    public DelegateCommand AddItemCommand => new DelegateCommand(AddItemCommandOnExecuted);

	    private async void AddItemCommandOnExecuted()
	    {
	        await NavigationService.NavigateAsync(new Uri("NewListItemPageModal", UriKind.Relative));            
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
	            }
            }
	    }

        public DelegateCommand SaveItemCommand => new DelegateCommand(SaveCommandOnExecuted);
	    private async void SaveCommandOnExecuted()
	    {
	        if (await UpdateListAsync())
	        {
	            await _dialogService.ShowInfoDialogAsync("Succes", "Your changes have been saved", "Ok");
	        }
	        else
	        {
	            await _dialogService.ShowErrorDialogAsync("Error", "Could not save changes", "Ok");
	        }
	    }

        #region Navigation

	    public override void OnNavigatingTo(INavigationParameters parameters)
	    {
            if (parameters.ContainsKey("List"))
	        {
	            _currentList = (ToDoList)parameters["List"];
	            Title = _currentList.Name;
	            ListItems = new ObservableCollection<ListItem>(_currentList.ListItems);
	            Created = _currentList.Created.ToShortDateString();
	            Owner = _currentList.Owner;
	        }
	    }

	    public override void OnNavigatedFrom(INavigationParameters parameters)
	    {

	    }

	    public override void OnNavigatedTo(INavigationParameters parameters)
	    {
	        if (parameters.ContainsKey("ListItem"))
	        {
	            if (_currentList.AddItem((ListItem)parameters["ListItem"]))
	            {
	                ListItems.Add((ListItem)parameters["ListItem"]);
	            }
	        }
        }

        #endregion

	    private async Task<bool> UpdateListAsync()
	    {
	        bool ret = false;
	        var result = await _listStore.UpdateListAsync(_currentList.Name, _currentList);
	        if (result != null)
	        {
	            //ListItems = new ObservableCollection<ListItem>(result.ListItems);
	            ret = true;
	        }

	        SelectedItem = null;
            return ret;
	    }

    }
}
