using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;

namespace EditorAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

        public override void OnFrameworkInitializationCompleted()
        {
            Console.WriteLine("App: OnFrameworkInitializationCompleted started");
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Console.WriteLine("App: Creating MainWindow");
                
                // Create the Avalonia editor window
                MainWindow editor = new();
                desktop.MainWindow = editor;
                
                Console.WriteLine("App: MainWindow created with MonoGame integration enabled");
            }
            else
            {
                Console.WriteLine("App: ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime");
            }

            Console.WriteLine("App: OnFrameworkInitializationCompleted finished");
            base.OnFrameworkInitializationCompleted();
        }
}