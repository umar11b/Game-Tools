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
                editor.Game = new GameEditor(editor);
                desktop.MainWindow = editor;
                
                Console.WriteLine("App: MainWindow created - MonoGame disabled for macOS ARM compatibility");
                
                // Note: MonoGame graphics disabled for macOS ARM compatibility
                // The UI and save/load functionality work independently
                // For Lab 3 demonstration, focus on the editor UI and file operations
            }
            else
            {
                Console.WriteLine("App: ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime");
            }

            Console.WriteLine("App: OnFrameworkInitializationCompleted finished");
            base.OnFrameworkInitializationCompleted();
        }
}