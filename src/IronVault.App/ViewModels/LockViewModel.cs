using System.Threading.Tasks;
using System.Windows.Input;

namespace IronVault.App.ViewModels;

public class LockViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isBusy;

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                ErrorMessage = string.Empty; // Clear error on edit
                ((AsyncRelayCommand)UnlockCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                ((AsyncRelayCommand)UnlockCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand UnlockCommand { get; }

    public LockViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        UnlockCommand = new AsyncRelayCommand(UnlockAsync, CanUnlock);
    }

    private bool CanUnlock()
    {
        return !IsBusy && !string.IsNullOrWhiteSpace(Password);
    }

    private async Task UnlockAsync()
    {
        ErrorMessage = string.Empty;
        IsBusy = true;

        try
        {
            bool success = await _mainViewModel.TryUnlockAsync(Password);
            if (!success)
            {
                ErrorMessage = "Invalid master password. Please try again.";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void Reset()
    {
        Password = string.Empty;
        ErrorMessage = string.Empty;
        IsBusy = false;
    }
}
