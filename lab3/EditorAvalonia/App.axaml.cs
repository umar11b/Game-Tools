using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

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
            // Following slide example exactly - but adapted for Avalonia
            MainWindow editor = new();
            editor.Game = new GameEditor(editor);
            desktop.MainWindow = editor;
            
            // Start MonoGame in background thread
            System.Threading.Thread gameThread = new System.Threading.Thread(() => editor.Game.Run());
            gameThread.IsBackground = true;
            gameThread.Start();
        }

        base.OnFrameworkInitializationCompleted();
    }
}