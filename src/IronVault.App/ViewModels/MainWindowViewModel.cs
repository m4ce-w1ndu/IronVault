using System.Threading.Tasks;
using System.Windows.Input;
using IronVault.Core.Services;

namespace IronVault.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IVaultService _vaultService;
    private ViewModelBase _currentPage;
    private bool _isUnlocked;

    public IVaultService VaultService => _vaultService;

    public bool IsUnlocked
    {
        get => _isUnlocked;
        private set => SetProperty(ref _isUnlocked, value);
    }

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, value);
    }

    public LockViewModel LockPage { get; }
    public VaultViewModel VaultPage { get; }
    public GeneratorViewModel GeneratorPage { get; }
    public SettingsViewModel SettingsPage { get; }

    public ICommand NavigateToVaultCommand { get; }
    public ICommand NavigateToGeneratorCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }

    public MainWindowViewModel(IVaultService vaultService)
    {
        _vaultService = vaultService;

        // Initialize pages
        LockPage = new LockViewModel(this);
        VaultPage = new VaultViewModel(this);
        GeneratorPage = new GeneratorViewModel(this);
        SettingsPage = new SettingsViewModel(this);

        // Navigation commands
        NavigateToVaultCommand = new RelayCommand(() => CurrentPage = VaultPage);
        NavigateToGeneratorCommand = new RelayCommand(() => CurrentPage = GeneratorPage);
        NavigateToSettingsCommand = new RelayCommand(() => CurrentPage = SettingsPage);

        // Start on correct page based on initial lock state
        _isUnlocked = !vaultService.IsLocked;
        if (_isUnlocked)
        {
            _currentPage = VaultPage;
            _ = VaultPage.LoadItemsAsync();
        }
        else
        {
            _currentPage = LockPage;
        }
    }

    public async Task<bool> TryUnlockAsync(string password)
    {
        var success = await _vaultService.UnlockAsync(password);
        if (success)
        {
            IsUnlocked = true;
            // Load vault items
            await VaultPage.LoadItemsAsync();
            CurrentPage = VaultPage;
        }
        return success;
    }

    public async Task LogoutAsync()
    {
        await _vaultService.LockAsync();
        IsUnlocked = false;
        LockPage.Reset();
        CurrentPage = LockPage;
    }
}
