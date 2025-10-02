using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;

namespace EditorAvalonia;

public partial class MainWindow : Window
{
    public GameEditor? Game { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();
        Title = "Our Cool Editor";
        
        // Initialize the MonoGame control
        GameViewControl.GameInitialized += OnGameViewInitialized;
    }
    
    private void OnGameViewInitialized(object? sender, EventArgs e)
    {
        // Get the game instance from the control
        Game = GameViewControl.Game;
    }
    
    // Event handlers (following slide example)
        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            Game?.SaveGame();
            StatusLabel.Text = "Game Saved";
        }
    
        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            Game?.LoadGame();
            StatusLabel.Text = "Game Loaded";
        }

        private async void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            // Following slide example exactly - Create new project using modern Avalonia API
            var storageProvider = this.StorageProvider;
            var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Create New Project",
                DefaultExtension = "oce",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Our Cool Editor Project")
                    {
                        Patterns = new[] { "*.oce" }
                    }
                }
            });

            if (file != null)
            {
                var fileName = file.Name;
                if (!fileName.EndsWith(".oce"))
                {
                    fileName += ".oce";
                }
                
                Game!.Project = new Project(Game.Content, fileName);
                Title = "Our Cool Editor - " + Game.Project?.Name;
                Game.AdjustAspectRatio();
                StatusLabel.Text = "New Project Created";
            }
        }
    
    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Game?.Exit();
        Close();
    }
    
    // Resize event handlers (following slide example)
    private void Panel1_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        Game?.AdjustAspectRatio();
    }
    
    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        Game?.AdjustAspectRatio();
    }
    
    // Solar System Editor Menu Handlers
    private void AddSun_Click(object sender, RoutedEventArgs e)
    {
        Game?.AddSun();
        StatusLabel.Text = "Sun added to solar system";
    }
    
    private void AddPlanet_Click(object sender, RoutedEventArgs e)
    {
        Game?.AddPlanet();
        StatusLabel.Text = "Planet added to solar system";
    }
    
    private void AddMoon_Click(object sender, RoutedEventArgs e)
    {
        Game?.AddMoon();
        StatusLabel.Text = "Moon added to solar system";
    }
}