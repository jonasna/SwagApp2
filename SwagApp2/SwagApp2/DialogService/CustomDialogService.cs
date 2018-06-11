using System.Threading.Tasks;
using SwagApp2.Views.CustomDialog;
using Rg.Plugins.Popup.Services;
using Prism.Navigation;
using System;
using Rg.Plugins.Popup.Pages;

namespace SwagApp2.DialogService
{
    public class CustomDialogService : ICustomDialogService
    {
        private TaskCompletionSource<bool> _popUpClosedCompletionSource;
        private readonly INavigationService _navigationService;
        public CustomDialogService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<bool> ShowErrorDialogAsync(string title, string message, string btnText)
        {
            var navParams = new NavigationParameters
            {
                { "Title", title },
                { "Message", message },
                { "BtnText", btnText },
                { "IsError", true}
            };

            // Navigate
            return await Navigate(navParams, "InfoDialog");
        }

        public async Task<bool> ShowInfoDialogAsync(string title, string message, string btnText)
        {
            var navParams = new NavigationParameters
            {
                { "Title", title },
                { "Message", message },
                { "BtnText", btnText },
                { "IsError", false}
            };

            // Navigate
            return await Navigate(navParams, "InfoDialog");
        }

        /// <summary>
        /// Handle popup page Navigation
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Navigate(INavigationParameters navParams, string uri)
        {
            await Reset();
            navParams.Add("Completion", _popUpClosedCompletionSource);

            await _navigationService.NavigateAsync(new Uri(uri, UriKind.Relative), navParams);
            var ret = await _popUpClosedCompletionSource.Task;
            await _navigationService.GoBackAsync();
            return ret;
        }

        private async Task Reset()
        {
            _popUpClosedCompletionSource = new TaskCompletionSource<bool>();
            await _navigationService.ClearPopupStackAsync();
        }
    }
}