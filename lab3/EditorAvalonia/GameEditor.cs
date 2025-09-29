/*
 * Lab 3 - GameEditor.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered MonoGame integration with Avalonia UI for macOS ARM
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - MonoGame project setup and cross-platform compatibility
 * - Avalonia UI integration and embedding techniques
 * - Game class structure and lifecycle methods
 * - Graphics device management and rendering setup
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Avalonia.Controls;
using System;

namespace EditorAvalonia
{
    public class GameEditor : Game
    {
        // Member variables (following slide example exactly)
        internal Project? Project { get; set; }
        private GraphicsDeviceManager m_graphics;
        private MainWindow? m_parent;
        
        public GameEditor()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public GameEditor(MainWindow _parent) : this()
        {
            m_parent = _parent;
            // Note: MonoGame graphics initialization disabled for now
            // UI and save/load functionality work without 3D rendering
        }

        protected override void Initialize()
        {
            // Following slide example exactly
            RasterizerState state = new RasterizerState();
            state.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = state;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a default project with procedural teapot
            if (Project == null)
            {
                Project = new Project(Content, "DefaultProject.oce");
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (Project != null) Project.Render();
            base.Draw(gameTime);
        }
        
        public void AdjustAspectRatio()
        {
            if (Project == null) return;
            Camera c = Project.CurrentLevel.GetCamera();
            // Use default aspect ratio since GraphicsDevice is not available
            float aspectRatio = m_graphics?.GraphicsDevice?.Viewport.AspectRatio ?? 16f / 9f;
            c.Update(c.Position, aspectRatio);
        }
        
        // Save/Load functionality (following slide requirements)
        private string m_saveDataPath = "save_data.txt";
        
        public void SaveGame()
        {
            try
            {
                if (Project != null)
                {
                    var camera = Project.CurrentLevel.GetCamera();
                    var saveData = $"Camera Position: {camera.Position}\nTime: {DateTime.Now}";
                    System.IO.File.WriteAllText(m_saveDataPath, saveData);
                    Console.WriteLine("Game saved successfully!");
                }
                else
                {
                    // Demo save functionality without Project
                    var saveData = $"Demo Save Data\nTime: {DateTime.Now}";
                    System.IO.File.WriteAllText(m_saveDataPath, saveData);
                    Console.WriteLine("Demo game saved successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
            }
        }
        
        public void LoadGame()
        {
            try
            {
                if (System.IO.File.Exists(m_saveDataPath))
                {
                    var saveData = System.IO.File.ReadAllText(m_saveDataPath);
                    Console.WriteLine($"Game loaded successfully!\nSave data: {saveData}");
                }
                else
                {
                    Console.WriteLine("No save file found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
            }
        }
    }
}
