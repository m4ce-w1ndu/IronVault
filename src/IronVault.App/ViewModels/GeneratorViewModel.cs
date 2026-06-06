using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Input.Platform;

namespace IronVault.App.ViewModels;

public class GeneratorViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private int _length = 16;
    private bool _includeUppercase = true;
    private bool _includeLowercase = true;
    private bool _includeNumbers = true;
    private bool _includeSymbols = true;
    private string _generatedPassword = string.Empty;

    public int Length
    {
        get => _length;
        set
        {
            if (SetProperty(ref _length, value))
            {
                GeneratePassword();
            }
        }
    }

    public bool IncludeUppercase
    {
        get => _includeUppercase;
        set
        {
            if (SetProperty(ref _includeUppercase, value))
            {
                GeneratePassword();
            }
        }
    }

    public bool IncludeLowercase
    {
        get => _includeLowercase;
        set
        {
            if (SetProperty(ref _includeLowercase, value))
            {
                GeneratePassword();
            }
        }
    }

    public bool IncludeNumbers
    {
        get => _includeNumbers;
        set
        {
            if (SetProperty(ref _includeNumbers, value))
            {
                GeneratePassword();
            }
        }
    }

    public bool IncludeSymbols
    {
        get => _includeSymbols;
        set
        {
            if (SetProperty(ref _includeSymbols, value))
            {
                GeneratePassword();
            }
        }
    }

    public string GeneratedPassword
    {
        get => _generatedPassword;
        set => SetProperty(ref _generatedPassword, value);
    }

    public ICommand GenerateCommand { get; }
    public ICommand CopyCommand { get; }

    public GeneratorViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        GenerateCommand = new RelayCommand(GeneratePassword);
        CopyCommand = new AsyncRelayCommand(CopyAsync);
        GeneratePassword();
    }

    public void GeneratePassword()
    {
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var charPool = new StringBuilder();
        if (IncludeUppercase) charPool.Append(upper);
        if (IncludeLowercase) charPool.Append(lower);
        if (IncludeNumbers) charPool.Append(digits);
        if (IncludeSymbols) charPool.Append(symbols);

        if (charPool.Length == 0)
        {
            GeneratedPassword = "Select at least one option";
            return;
        }

        var password = new char[Length];
        var pool = charPool.ToString();

        for (int i = 0; i < Length; i++)
        {
            password[i] = pool[RandomNumberGenerator.GetInt32(pool.Length)];
        }

        GeneratedPassword = new string(password);
    }

    private async Task CopyAsync()
    {
        if (string.IsNullOrEmpty(GeneratedPassword) || GeneratedPassword.StartsWith("Select")) return;
        var clipboard = Application.Current?.GetClipboard();
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(GeneratedPassword);
        }
    }
}
