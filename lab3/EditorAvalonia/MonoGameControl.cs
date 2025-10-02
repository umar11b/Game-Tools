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
                // Create the MonoGame instance (but don't run it yet)
                _game = new GameEditor();
                
                // Fire the initialized event
                GameInitialized?.Invoke(this, EventArgs.Empty);
                
                Console.WriteLine("MonoGame control initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing MonoGame: {ex.Message}");
                
                // Create a fallback game instance for UI purposes
                _game = new GameEditor();
                GameInitialized?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnDetachedFromVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            
            // Clean up MonoGame
            _game?.Exit();
            _game = null;
        }
    }
}
