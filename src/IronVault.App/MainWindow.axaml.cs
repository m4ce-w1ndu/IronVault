using Avalonia.Controls;
using Avalonia.Input;

namespace IronVault.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        TransparencyLevelHint = [WindowTransparencyLevel.Mica, WindowTransparencyLevel.AcrylicBlur];
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}