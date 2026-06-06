using System.Collections.Generic;
using Avalonia;
using Avalonia.Styling;

namespace IronVault.App.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private string _selectedTheme = "System";
    private int _lockTimeoutMinutes = 15;
    private int _clearClipboardSeconds = 30;

    public List<string> Themes { get; } = ["System", "Light", "Dark"];

    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (SetProperty(ref _selectedTheme, value))
            {
                ApplyTheme(value);
            }
        }
    }

    public List<int> LockTimeoutOptions { get; } = [1, 5, 15, 30, 60, 0]; // 0 is Never

    public int LockTimeoutMinutes
    {
        get => _lockTimeoutMinutes;
        set => SetProperty(ref _lockTimeoutMinutes, value);
    }

    public List<int> ClearClipboardOptions { get; } = [10, 30, 60, 120, 0]; // 0 is Never

    public int ClearClipboardSeconds
    {
        get => _clearClipboardSeconds;
        set => SetProperty(ref _clearClipboardSeconds, value);
    }

    public string AppVersion => "v0.1.0-alpha";
    public string AppAuthor => "IronVault Contributors";

    public SettingsViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    private void ApplyTheme(string theme)
    {
        if (Application.Current == null) return;

        Application.Current.RequestedThemeVariant = theme switch
        {
            "Light" => ThemeVariant.Light,
            "Dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };
    }
}
