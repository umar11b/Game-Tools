using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EditorSkiaSharp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        StatusLabel.Text = "Solar System Editor Ready";
        StatusText.Text = "Solar System Editor - Avalonia PoC";
    }

    // Event handlers
    private void AddSun_Click(object? sender, RoutedEventArgs e)
    {
        SceneView.SunExists = true;
        StatusLabel.Text = "Sun added to solar system";
    }

    private void AddPlanet_Click(object? sender, RoutedEventArgs e)
    {
        SceneView.PlanetExists = true;
        StatusLabel.Text = "Planet added to solar system";
    }

    private void AddMoon_Click(object? sender, RoutedEventArgs e)
    {
        SceneView.MoonExists = true;
        StatusLabel.Text = "Moon added to solar system";
    }

    private void ToggleTeapot_Click(object? sender, RoutedEventArgs e)
    {
        SceneView.ShowTeapot = !SceneView.ShowTeapot;
        StatusLabel.Text = $"Teapot {(SceneView.ShowTeapot ? "shown" : "hidden")}";
    }
}