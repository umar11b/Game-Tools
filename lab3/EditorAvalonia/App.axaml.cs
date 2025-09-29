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
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Create the Avalonia editor window
                MainWindow editor = new();
                editor.Game = new GameEditor(editor);
                desktop.MainWindow = editor;
                
                // Note: MonoGame graphics disabled for macOS ARM compatibility
                // The UI and save/load functionality work independently
                // For Lab 3 demonstration, focus on the editor UI and file operations
            }

            base.OnFrameworkInitializationCompleted();
        }
}