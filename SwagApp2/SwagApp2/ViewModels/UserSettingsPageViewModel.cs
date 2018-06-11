using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Services;
using SwagApp2.DataStores;
using SwagApp2.DialogService;
using SwagApp2.Models;
using Xamarin.Forms;

namespace SwagApp2.ViewModels
{
	public class UserSettingsPageViewModel : ViewModelBase
	{
	    private readonly IApplicationUserService _applicationUserService;
	    private readonly ICustomDialogService _dialogService;
	    public UserSettingsPageViewModel(INavigationService navigationService,
	        IApplicationUserService applicationUserService,
	        ICustomDialogService dialogService) : base(navigationService)
	    {
	        Title = "Settings";
	        _applicationUserService = applicationUserService;
	        _dialogService = dialogService;

	        DisplayName = _applicationUserService.DisplayName;
	        Name = _applicationUserService.Name;

	        Validate();
	    }

	    private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                Validate();
            }
        }

	    private string _displayName;
	    public string DisplayName
	    {
	        get => _displayName;
	        set
	        {
	            SetProperty(ref _displayName, value);
                Validate();
	        }
	    }

	    private bool _isValid;
	    public bool IsValid
	    {
	        get => _isValid;
	        private set => SetProperty(ref _isValid, value);
	    }

	    private string _message;
	    public string Message
	    {
	        get => _message;
	        private set => SetProperty(ref _message, value);
	    }

	    private Color _messageColor;
	    public Color MessageColor
	    {
	        get => _messageColor;
	        private set => SetProperty(ref _messageColor, value);
	    }

        public DelegateCommand SaveCommand => new DelegateCommand(SaveCommandOnExecuted, SaveCommandCanExecute)
            .ObservesProperty(() => IsValid);

	    private async void SaveCommandOnExecuted()
	    {
	        if (!string.IsNullOrEmpty(Name))
	        {
	            _applicationUserService.Name = Name;
	        }

	        _applicationUserService.DisplayName = DisplayName;
	        await _applicationUserService.SaveUserDataAsync();
	        await _dialogService.ShowInfoDialogAsync("Saved", "Your information has been saved!", "Ok");
	        Validate();
        }

	    private bool SaveCommandCanExecute()
	    {
	        return IsValid;
	    }

	    private void Validate()
	    {
	        if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Name))
	        {
	            IsValid = false;
	        }
	        else
	        {
	            IsValid = true;
	        }

	        if (!string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(Name) 
	                                               && DisplayName == _applicationUserService.DisplayName 
	                                               && Name == _applicationUserService.Name)
	        {
	            Message = "Settings have been saved!";
	            MessageColor = Color.FromHex("#89A34E");
	        }
	        else
	        {
	            Message = "Settings have not been saved!";
	            MessageColor = Color.FromHex("#ED6A5A");
	        }	        
        }
    }
}
