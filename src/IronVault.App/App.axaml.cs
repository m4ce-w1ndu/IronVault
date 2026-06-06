using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace IronVault.App;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vaultService = new IronVault.Core.Services.MockVaultService();
            var vm = new ViewModels.MainWindowViewModel(vaultService);
            var mainWindow = new MainWindow
            {
                DataContext = vm
            };
            
            mainWindow.Closed += (sender, e) => desktop.Shutdown();
            
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}