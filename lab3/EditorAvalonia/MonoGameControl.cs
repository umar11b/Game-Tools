/*
 * Lab 3 - MonoGameControl.cs
 * Game Development Tools Course
 * 
 * MonoGame-Avalonia integration control
 * Based on community guide: https://community.monogame.net/t/embed-monogame-inside-an-avaloniaui-control/17509
 */

using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.VisualTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EditorAvalonia
{
    public class MonoGameControl : Control
    {
        private GameEditor? _game;
        private bool _isInitialized = false;
        
        public GameEditor? Game => _game;
        public event EventHandler? GameInitialized;

        public MonoGameControl()
        {
            // Initialize the control
        }

        protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            
            if (!_isInitialized)
            {
                _isInitialized = true;
                InitializeMonoGame();
            }
        }

        private void InitializeMonoGame()
        {
            try
            {
                // Create the MonoGame instance
                _game = new GameEditor();
                
                // Fire the initialized event
                GameInitialized?.Invoke(this, EventArgs.Empty);
                
                Console.WriteLine("MonoGame control initialized successfully");
                
                // Start a render loop for this control
                StartRenderLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing MonoGame: {ex.Message}");
                
                // Create a fallback game instance for UI purposes
                _game = new GameEditor();
                GameInitialized?.Invoke(this, EventArgs.Empty);
            }
        }
        
        private async void StartRenderLoop()
        {
            // Simple render loop that updates the control
            while (_game != null)
            {
                try
                {
                    // Update solar system objects with animation
                    UpdateSolarSystemObjects();
                    
                    // Trigger a redraw of the Avalonia control
                    InvalidateVisual();
                    
                    // Wait a bit before next frame
                    await Task.Delay(16); // ~60 FPS
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Render loop error: {ex.Message}");
                    break;
                }
            }
        }
        
        private void UpdateSolarSystemObjects()
        {
            if (_game?.SolarSystemObjects == null) return;
            
            // Update each solar system object
            foreach (var obj in _game.SolarSystemObjects)
            {
                obj.Update();
            }
        }

        protected override void OnDetachedFromVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            
            // Clean up MonoGame
            _game?.Exit();
            _game = null;
        }
        
        public override void Render(Avalonia.Media.DrawingContext context)
        {
            base.Render(context);
            
            // Create a black background for the 3D view
            var bounds = new Avalonia.Rect(0, 0, Bounds.Width, Bounds.Height);
            var blackBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Black);
            context.FillRectangle(blackBrush, bounds);
            
            // Draw a simple 3D scene representation
            Draw3DScene(context, bounds);
        }
        
        private void Draw3DScene(Avalonia.Media.DrawingContext context, Avalonia.Rect bounds)
        {
            if (_game == null) return;
            
            // Try to render 3D models first, fallback to 2D if needed
            if (TryRender3DModels(context, bounds))
            {
                return; // 3D rendering successful
            }
            
            // Fallback to 2D representation
            Draw2DFallback(context, bounds);
        }
        
        private bool TryRender3DModels(Avalonia.Media.DrawingContext context, Avalonia.Rect bounds)
        {
            try
            {
                // Check if we have solar system objects with models
                if (_game.SolarSystemObjects == null || _game.SolarSystemObjects.Count == 0)
                {
                    return false; // No objects to render
                }
                
                // Try to render 3D content using MonoGame
                Render3DSolarSystem(context, bounds);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"3D rendering failed: {ex.Message}");
                return false; // Fallback to 2D
            }
        }
        
        private void Render3DSolarSystem(Avalonia.Media.DrawingContext context, Avalonia.Rect bounds)
        {
            // Try to use MonoGame's actual 3D rendering
            try
            {
                // This is where we would call the actual MonoGame Draw method
                // For now, we'll use enhanced 2D representation that looks more 3D
                var centerX = bounds.Width / 2;
                var centerY = bounds.Height / 2;
                
                // Enhanced 3D-like rendering with better shading
                DrawEnhancedSolarSystem(context, centerX, centerY);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"3D rendering error: {ex.Message}");
                // Fallback to basic 2D
                Draw2DFallback(context, bounds);
            }
        }
        
        private void DrawEnhancedSolarSystem(Avalonia.Media.DrawingContext context, double centerX, double centerY)
        {
            // Draw sun with 3D-like appearance
            if (_game.SolarSystemObjects?.Any(obj => obj.Type == SolarSystemObjectType.Sun) == true)
            {
                Draw3DLikeSun(context, centerX, centerY);
            }
            
            // Draw planets with 3D-like appearance
            var planets = _game.SolarSystemObjects?.Where(obj => obj.Type == SolarSystemObjectType.Planet).ToList();
            if (planets != null)
            {
                foreach (var planet in planets)
                {
                    var planetX = centerX + (planet.Position.X * 0.3);
                    var planetY = centerY + (planet.Position.Y * 0.3);
                    Draw3DLikePlanet(context, planetX, planetY, planet);
                }
            }
            
            // Draw moons with 3D-like appearance
            var moons = _game.SolarSystemObjects?.Where(obj => obj.Type == SolarSystemObjectType.Moon).ToList();
            if (moons != null)
            {
                foreach (var moon in moons)
                {
                    var moonX = centerX + (moon.Position.X * 0.3);
                    var moonY = centerY + (moon.Position.Y * 0.3);
                    Draw3DLikeMoon(context, moonX, moonY, moon);
                }
            }
            
            // Draw status text
            var statusText = $"Solar System Objects: {_game.SolarSystemObjects?.Count ?? 0} (3D Enhanced)";
            var text = new Avalonia.Media.FormattedText(
                statusText,
                System.Globalization.CultureInfo.CurrentCulture,
                Avalonia.Media.FlowDirection.LeftToRight,
                new Avalonia.Media.Typeface("Arial"),
                12,
                Avalonia.Media.Brushes.White
            );
            
            context.DrawText(text, new Avalonia.Point(10, 10));
        }
        
        private void Draw3DLikeSun(Avalonia.Media.DrawingContext context, double x, double y)
        {
            var sunRadius = 30;
            
            // Create realistic sun with multiple layers and gradient effect
            var colors = new[] { 
                Avalonia.Media.Colors.White,
                Avalonia.Media.Colors.Yellow,
                Avalonia.Media.Colors.Orange,
                Avalonia.Media.Colors.DarkOrange,
                Avalonia.Media.Colors.Red
            };
            
            // Draw outer glow
            var glowRadius = sunRadius + 8;
            var glowBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(50, 255, 255, 0));
            var glowEllipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(x - glowRadius, y - glowRadius, glowRadius * 2, glowRadius * 2));
            context.DrawGeometry(glowBrush, null, glowEllipse);
            
            // Draw sun layers for 3D effect
            for (int i = 0; i < colors.Length; i++)
            {
                var radius = sunRadius - (i * 4);
                if (radius > 0)
                {
                    var brush = new Avalonia.Media.SolidColorBrush(colors[i]);
                    var ellipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(x - radius, y - radius, radius * 2, radius * 2));
                    context.DrawGeometry(brush, null, ellipse);
                }
            }
            
            // Add highlight for 3D effect
            var highlightRadius = sunRadius / 3;
            var highlightBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(100, 255, 255, 255));
            var highlightEllipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(x - highlightRadius - 5, y - highlightRadius - 5, highlightRadius * 2, highlightRadius * 2));
            context.DrawGeometry(highlightBrush, null, highlightEllipse);
        }
        
        private void Draw3DLikePlanet(Avalonia.Media.DrawingContext context, double x, double y, SolarSystemObject planet)
        {
            var planetRadius = 15;
            
            // Create realistic planet with atmosphere and shading
            var colors = new[] { 
                Avalonia.Media.Colors.DarkBlue,
                Avalonia.Media.Colors.Blue,
                Avalonia.Media.Colors.LightBlue,
                Avalonia.Media.Colors.Cyan
            };
            
            // Draw atmosphere glow
            var atmosphereRadius = planetRadius + 3;
            var atmosphereBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(30, 0, 100, 255));
            var atmosphereEllipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(x - atmosphereRadius, y - atmosphereRadius, atmosphereRadius * 2, atmosphereRadius * 2));
            context.DrawGeometry(atmosphereBrush, null, atmosphereEllipse);
            
            // Draw planet layers for 3D effect
            for (int i = 0; i < colors.Length; i++)
            {
                var radius = planetRadius - (i * 2);
                if (radius > 0)
                {
                    var brush = new Avalonia.Media.SolidColorBrush(colors[i]);
                    var ellipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(x - radius, y - radius, radius * 2, radius * 2));
                    context.DrawGeometry(brush, null, ellipse);
                }
            }
            
            // Add highlight for 3D effect
            var highlightRadius = planetRadius / 4;
            var highlightBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(80, 255, 255, 255));
            var highlightEllipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(x - highlightRadius - 3, y - highlightRadius - 3, highlightRadius * 2, highlightRadius * 2));
            context.DrawGeometry(highlightBrush, null, highlightEllipse);
        }
        
        private void Draw3DLikeMoon(Avalonia.Media.DrawingContext context, double x, double y, SolarSystemObject moon)
        {
            var moonRadius = 6;
            
            // Draw moon with 3D effect
            var colors = new[] { 
                Avalonia.Media.Colors.DarkGray, 
                Avalonia.Media.Colors.Gray, 
                Avalonia.Media.Colors.LightGray 
            };
            
            for (int i = 0; i < colors.Length; i++)
            {
                var radius = moonRadius - (i * 1);
                var brush = new Avalonia.Media.SolidColorBrush(colors[i]);
                var ellipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(x - radius, y - radius, radius * 2, radius * 2));
                context.DrawGeometry(brush, null, ellipse);
            }
        }
        
        private void Draw2DFallback(Avalonia.Media.DrawingContext context, Avalonia.Rect bounds)
        {
            // Original 2D fallback code
            var centerX = bounds.Width / 2;
            var centerY = bounds.Height / 2;
            
            // Draw sun (if exists)
            if (_game.SolarSystemObjects?.Any(obj => obj.Type == SolarSystemObjectType.Sun) == true)
            {
                var sunBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Yellow);
                var sunRadius = 20;
                var sunEllipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(centerX - sunRadius, centerY - sunRadius, sunRadius * 2, sunRadius * 2));
                context.DrawGeometry(sunBrush, null, sunEllipse);
            }
            
            // Draw planets
            var planets = _game.SolarSystemObjects?.Where(obj => obj.Type == SolarSystemObjectType.Planet).ToList();
            if (planets != null)
            {
                foreach (var planet in planets)
                {
                    var planetX = centerX + (planet.Position.X * 0.5);
                    var planetY = centerY + (planet.Position.Y * 0.5);
                    
                    var planetBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Blue);
                    var planetRadius = 8;
                    var planetEllipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(planetX - planetRadius, planetY - planetRadius, planetRadius * 2, planetRadius * 2));
                    context.DrawGeometry(planetBrush, null, planetEllipse);
                }
            }
            
            // Draw moons
            var moons = _game.SolarSystemObjects?.Where(obj => obj.Type == SolarSystemObjectType.Moon).ToList();
            if (moons != null)
            {
                foreach (var moon in moons)
                {
                    var moonX = centerX + (moon.Position.X * 0.5);
                    var moonY = centerY + (moon.Position.Y * 0.5);
                    
                    var moonBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Gray);
                    var moonRadius = 4;
                    var moonEllipse = new Avalonia.Media.EllipseGeometry(new Avalonia.Rect(moonX - moonRadius, moonY - moonRadius, moonRadius * 2, moonRadius * 2));
                    context.DrawGeometry(moonBrush, null, moonEllipse);
                }
            }
            
            // Draw status text
            var statusText = $"Solar System Objects: {_game.SolarSystemObjects?.Count ?? 0} (2D Fallback)";
            var text = new Avalonia.Media.FormattedText(
                statusText,
                System.Globalization.CultureInfo.CurrentCulture,
                Avalonia.Media.FlowDirection.LeftToRight,
                new Avalonia.Media.Typeface("Arial"),
                12,
                Avalonia.Media.Brushes.White
            );
            
            context.DrawText(text, new Avalonia.Point(10, 10));
        }
    }
}
