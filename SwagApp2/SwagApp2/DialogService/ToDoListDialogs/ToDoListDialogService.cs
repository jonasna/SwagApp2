using System;
using System.Threading.Tasks;
using Prism.Navigation;
using SwagApp2.Models;

namespace SwagApp2.DialogService.ToDoListDialogs
{
    public class ToDoListDialogService : IToDoListDialogService
    {
        private readonly INavigationService _navigationService;

        public ToDoListDialogService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<ToDoList> ShowCreateToDoListModalAsync()
        {
            return await Navigate<ToDoList>("NewListModal");
        }

        public async Task<ListItem> ShowCreateListItemModalAsync()
        {
            return await Navigate<ListItem>("NewListItemModal");
        }
        
        private async Task<T> Navigate<T>(string uri)
        {
            var navParams = new NavigationParameters();
            var completionSource = new TaskCompletionSource<T>();
            navParams.Add("Completion", completionSource);
            await _navigationService.NavigateAsync(new Uri(uri, UriKind.Relative), navParams);
            var ret = await completionSource.Task;
            await _navigationService.GoBackAsync();
            return ret;
        }
    }
}