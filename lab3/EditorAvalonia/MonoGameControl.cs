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
                    // Update the game (we can't call Update directly, so we'll just trigger a redraw)
                    
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
            
            // For now, draw a placeholder to show the control is working
            var bounds = new Avalonia.Rect(0, 0, Bounds.Width, Bounds.Height);
            var brush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.DarkBlue);
            context.FillRectangle(brush, bounds);
            
            // Draw some text to show it's working
            var text = new Avalonia.Media.FormattedText(
                "MonoGame Control\n(3D rendering will appear here)",
                System.Globalization.CultureInfo.CurrentCulture,
                Avalonia.Media.FlowDirection.LeftToRight,
                new Avalonia.Media.Typeface("Arial"),
                16,
                Avalonia.Media.Brushes.White
            );
            
            var textPosition = new Avalonia.Point(
                (Bounds.Width - 200) / 2, // Approximate text width
                (Bounds.Height - 40) / 2  // Approximate text height
            );
            
            context.DrawText(text, textPosition);
        }
    }
}
