using Avalonia;
using System;
using System.Threading;

namespace EditorAvalonia;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // Start Avalonia UI first
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
