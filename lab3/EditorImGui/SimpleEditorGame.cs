/*
 * Lab 3 - SimpleEditorGame.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered simple editor with embedded MonoGame teapot rendering
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Simple editor interface using SpriteBatch text rendering
 * - Embedded 3D teapot rendering in editor
 * - File operations (Create, Save, Load) integration
 * - Cross-platform compatibility for macOS ARM
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;

namespace EditorImGui
{
    public class SimpleEditorGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        
        // Editor state
        private Project? _currentProject;
        private string _statusText = "Ready";
        
        // Menu system
        private bool _showFileMenu = false;
        private bool _showProjectMenu = false;
        private MouseState _previousMouseState;
        private KeyboardState _previousKeyboardState;
        
        // Embedded Editor2025 teapot rendering
        private Model? _teapot;
        private Matrix _world = Matrix.Identity;
        private Matrix _view = Matrix.CreateLookAt(new Vector3(0, 0, 2), new Vector3(0, 0, 0), Vector3.Up);
        private Matrix _projection;

        public SimpleEditorGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            Window.Title = "Our Cool Editor - Simple Version";
        }

        protected override void Initialize()
        {
            // Initialize matrices exactly like Editor2025
            _world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            _view = Matrix.CreateLookAt(new Vector3(0, 0, 2), new Vector3(0, 0, 0), Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), 
                16f / 9f, // Use default aspect ratio instead of GraphicsDevice.Viewport
                0.1f, 
                100f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Load a simple font (we'll create one if needed)
            try
            {
                _font = CreateSimpleFont(GraphicsDevice);
            }
            catch
            {
                // If font creation fails, we'll handle text rendering differently
                _font = null;
            }

            // Load the teapot model exactly like Editor2025
            try
            {
                _teapot = Content.Load<Model>("teapot");
            }
            catch
            {
                // Fallback to procedural sphere if teapot model fails to load
                _teapot = CreateSphereModel(GraphicsDevice);
            }
        }

        private SpriteFont CreateSimpleFont(GraphicsDevice graphicsDevice)
        {
            // Create a simple font using MonoGame's built-in default font
            // This will use the system's default font for text rendering
            try
            {
                // Try to load a default font - if this fails, we'll create a simple one
                return Content.Load<SpriteFont>("DefaultFont");
            }
            catch
            {
                // If no font file exists, create a simple fallback
                // For now, we'll create a basic texture-based font
                var texture = new Texture2D(graphicsDevice, 8, 12);
                var pixels = new Color[8 * 12];
                
                // Create a simple white rectangle for each character
                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = Color.White;
                }
                
                texture.SetData(pixels);
                
                // If we can't create a proper font, return null and use fallback text rendering
                return null;
            }
        }

        private Model CreateSphereModel(GraphicsDevice graphicsDevice)
        {
            var vertices = new List<VertexPositionColorTexture>();
            var indices = new List<int>();

            int segments = 16;
            int rings = 16;

            for (int ring = 0; ring <= rings; ring++)
            {
                float v = (float)ring / rings;
                float phi = v * MathHelper.Pi;

                for (int segment = 0; segment <= segments; segment++)
                {
                    float u = (float)segment / segments;
                    float theta = u * MathHelper.TwoPi;

                    float x = (float)(Math.Cos(theta) * Math.Sin(phi));
                    float y = (float)Math.Cos(phi);
                    float z = (float)(Math.Sin(theta) * Math.Sin(phi));

                    vertices.Add(new VertexPositionColorTexture(
                        new Vector3(x, y, z),
                        Color.Gray,
                        new Vector2(u, v)
                    ));
                }
            }

            for (int ring = 0; ring < rings; ring++)
            {
                for (int segment = 0; segment < segments; segment++)
                {
                    int current = ring * (segments + 1) + segment;
                    int next = current + segments + 1;

                    indices.Add(current);
                    indices.Add(next);
                    indices.Add(current + 1);

                    indices.Add(current + 1);
                    indices.Add(next);
                    indices.Add(next + 1);
                }
            }

            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices.ToArray());

            var indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Count, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices.ToArray());

            var meshParts = new List<ModelMeshPart>();
            var meshPart = new ModelMeshPart();
            meshPart.VertexBuffer = vertexBuffer;
            meshPart.IndexBuffer = indexBuffer;
            meshPart.PrimitiveCount = indices.Count / 3;
            meshPart.StartIndex = 0;
            meshPart.VertexOffset = 0;
            meshParts.Add(meshPart);

            var meshes = new List<ModelMesh>();
            var mesh = new ModelMesh(graphicsDevice, meshParts);
            meshes.Add(mesh);

            var bones = new List<ModelBone>();
            var model = new Model(graphicsDevice, bones, meshes);

            return model;
        }

        private Texture2D CreateCheckerboardTexture(GraphicsDevice graphicsDevice)
        {
            int size = 64;
            Color[] colors = new Color[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    colors[x + y * size] = ((x / 8) % 2 == (y / 8) % 2) ? Color.DarkGray : Color.LightGray;
                }
            }
            Texture2D texture = new Texture2D(graphicsDevice, size, size);
            texture.SetData(colors);
            return texture;
        }

        private BasicEffect CreateBasicShader(GraphicsDevice graphicsDevice)
        {
            var effect = new BasicEffect(graphicsDevice);
            effect.EnableDefaultLighting();
            effect.TextureEnabled = true;
            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
            effect.DirectionalLight0.Direction = new Vector3(1, -1, 0);
            effect.DirectionalLight0.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);
            return effect;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Handle mouse input for menu interactions
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            
            HandleMenuInput(mouseState, keyboardState);

            // Keep keyboard shortcuts as backup
            if (keyboardState.IsKeyDown(Keys.F1))
            {
                CreateProject();
            }
            if (keyboardState.IsKeyDown(Keys.F2))
            {
                SaveProject();
            }
            if (keyboardState.IsKeyDown(Keys.F3))
            {
                LoadProject();
            }

            // Update teapot animation (Editor2025 style - just rotation)

            // Store previous states
            _previousMouseState = mouseState;
            _previousKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the teapot first (in the game view area)
            DrawTeapot();

            // Draw editor UI on top (only borders and text, not backgrounds)
            DrawSimpleEditorUI();

            base.Draw(gameTime);
        }

        private void DrawTeapot()
        {
            if (_teapot == null || _teapot.Meshes == null) return;

            // Define the Game View panel boundaries (left 70% of window, accounting for menu bar)
            var viewport = GraphicsDevice.Viewport;
            var gameViewWidth = (int)(viewport.Width * 0.7f) - 20; // Match UI panel width
            var gameViewHeight = viewport.Height - 105; // Match UI panel height (accounting for menu bar)
            var gameViewX = 10; // Match UI panel X
            var gameViewY = 35; // Match UI panel Y (below menu bar)

            // Set the viewport to only render within the Game View panel
            var originalViewport = GraphicsDevice.Viewport;
            GraphicsDevice.Viewport = new Viewport(gameViewX, gameViewY, gameViewWidth, gameViewHeight);

            // Clear the game view area with dark blue background (more visible than black)
            GraphicsDevice.Clear(Color.DarkBlue);

            // Apply rotation exactly like Editor2025
            _world *= Matrix.CreateRotationY(0.1f);

            // Render the teapot exactly like Editor2025 with proper null checks
            foreach (var mesh in _teapot.Meshes)
            {
                if (mesh == null || mesh.Effects == null) continue;
                
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (effect == null) continue;
                    
                    effect.World = _world;
                    effect.View = _view;
                    effect.Projection = _projection;
                    effect.EnableDefaultLighting(); // Ensure lighting is enabled
                    effect.Alpha = 1.0f; // Ensure alpha is set
                }
                
                try
                {
                    mesh.Draw();
                }
                catch
                {
                    // If mesh drawing fails, continue with next mesh
                    continue;
                }
            }

            // Restore the original viewport for UI rendering
            GraphicsDevice.Viewport = originalViewport;
        }

        private void DrawSimpleEditorUI()
        {
            if (_spriteBatch == null) return;

            _spriteBatch.Begin();

            // Draw editor panels using colored rectangles and text
            var viewport = GraphicsDevice.Viewport;

            // Draw menu bar first
            DrawMenuBar();

            // Game View Panel (left side - 70%) - Make it transparent so teapot shows through
            var gameViewRect = new Rectangle(10, 35, (int)(viewport.Width * 0.7f) - 20, viewport.Height - 105);
            DrawTransparentPanel(_spriteBatch, gameViewRect, "Game View");

            // Properties Panel (right side - 30%) - Make it look more professional
            var propertiesRect = new Rectangle((int)(viewport.Width * 0.7f) + 10, 35, (int)(viewport.Width * 0.3f) - 20, viewport.Height - 105);
            DrawPanel(_spriteBatch, propertiesRect, Color.FromNonPremultiplied(245, 245, 245, 255), "Properties");

            // Status Bar (bottom) - Make it look like Avalonia status bar
            var statusRect = new Rectangle(10, viewport.Height - 60, viewport.Width - 20, 50);
            DrawPanel(_spriteBatch, statusRect, Color.FromNonPremultiplied(240, 240, 240, 255), "");

            // Draw text in the game view panel
            var gameViewText = "Editor2025 Teapot Embedded";
            DrawTextSafe(gameViewText, new Vector2(gameViewRect.X + 20, gameViewRect.Y + 30), Color.White);

            // Draw rotation info in game view
            var rotationText = "Spinning Teapot (Editor2025)";
            DrawTextSafe(rotationText, new Vector2(gameViewRect.X + 20, gameViewRect.Y + 60), Color.White);

            // Draw project info in properties panel
            if (_currentProject != null)
            {
                DrawTextSafe("Inspector Panel", new Vector2(propertiesRect.X + 20, propertiesRect.Y + 30), Color.Black);
                DrawTextSafe($"Project: {_currentProject.Name}", new Vector2(propertiesRect.X + 20, propertiesRect.Y + 60), Color.Black);
                DrawTextSafe($"Folder: {_currentProject.Folder}", new Vector2(propertiesRect.X + 20, propertiesRect.Y + 80), Color.Black);
                DrawTextSafe($"Levels: {_currentProject.Levels.Count}", new Vector2(propertiesRect.X + 20, propertiesRect.Y + 100), Color.Black);
            }
            else
            {
                DrawTextSafe("Inspector Panel", new Vector2(propertiesRect.X + 20, propertiesRect.Y + 30), Color.Black);
                DrawTextSafe("No Project Loaded", new Vector2(propertiesRect.X + 20, propertiesRect.Y + 60), Color.Black);
            }

            // Draw status text
            DrawTextSafe(_statusText, new Vector2(statusRect.X + 10, statusRect.Y + 15), Color.Black);

            _spriteBatch.End();
        }

        private void HandleMenuInput(MouseState mouseState, KeyboardState keyboardState)
        {
            // Handle left mouse button clicks
            if (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                var mousePos = new Point(mouseState.X, mouseState.Y);
                
                // Check if clicking on File menu
                if (mousePos.X >= 5 && mousePos.X <= 50 && mousePos.Y >= 5 && mousePos.Y <= 25)
                {
                    _showFileMenu = !_showFileMenu;
                    _showProjectMenu = false;
                }
                // Check if clicking on Project submenu items
                else if (_showFileMenu && mousePos.X >= 5 && mousePos.X <= 150 && mousePos.Y >= 30 && mousePos.Y <= 50)
                {
                    _showProjectMenu = !_showProjectMenu;
                }
                // Check if clicking on Create
                else if (_showProjectMenu && mousePos.X >= 25 && mousePos.X <= 100 && mousePos.Y >= 55 && mousePos.Y <= 75)
                {
                    CreateProject();
                    _showFileMenu = false;
                    _showProjectMenu = false;
                }
                // Check if clicking on Save
                else if (_showProjectMenu && mousePos.X >= 25 && mousePos.X <= 100 && mousePos.Y >= 80 && mousePos.Y <= 100)
                {
                    SaveProject();
                    _showFileMenu = false;
                    _showProjectMenu = false;
                }
                // Check if clicking on Load
                else if (_showProjectMenu && mousePos.X >= 25 && mousePos.X <= 100 && mousePos.Y >= 105 && mousePos.Y <= 125)
                {
                    LoadProject();
                    _showFileMenu = false;
                    _showProjectMenu = false;
                }
                // Check if clicking on Exit
                else if (_showFileMenu && mousePos.X >= 5 && mousePos.X <= 100 && mousePos.Y >= 130 && mousePos.Y <= 150)
                {
                    Exit();
                }
                // Close menus if clicking elsewhere
                else
                {
                    _showFileMenu = false;
                    _showProjectMenu = false;
                }
            }
        }

        private void DrawMenuBar()
        {
            if (_spriteBatch == null) return;

            var viewport = GraphicsDevice.Viewport;
            
            // Draw menu bar background
            var menuBarRect = new Rectangle(0, 0, viewport.Width, 25);
            var texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.FromNonPremultiplied(240, 240, 240, 255) });
            _spriteBatch.Draw(texture, menuBarRect, Color.White);

            // Draw File menu button
            var fileButtonRect = new Rectangle(5, 5, 40, 20);
            var fileButtonColor = _showFileMenu ? Color.LightBlue : Color.White;
            texture.SetData(new[] { fileButtonColor });
            _spriteBatch.Draw(texture, fileButtonRect, Color.White);
            DrawTextSafe("File", new Vector2(fileButtonRect.X + 5, fileButtonRect.Y + 2), Color.Black);

            // Draw File menu dropdown
            if (_showFileMenu)
            {
                var fileMenuRect = new Rectangle(5, 30, 120, 125);
                texture.SetData(new[] { Color.White });
                _spriteBatch.Draw(texture, fileMenuRect, Color.White);

                // Draw border
                var borderColor = Color.Black;
                texture.SetData(new[] { borderColor });
                _spriteBatch.Draw(texture, new Rectangle(fileMenuRect.X, fileMenuRect.Y, fileMenuRect.Width, 1), borderColor); // Top
                _spriteBatch.Draw(texture, new Rectangle(fileMenuRect.X, fileMenuRect.Y + fileMenuRect.Height - 1, fileMenuRect.Width, 1), borderColor); // Bottom
                _spriteBatch.Draw(texture, new Rectangle(fileMenuRect.X, fileMenuRect.Y, 1, fileMenuRect.Height), borderColor); // Left
                _spriteBatch.Draw(texture, new Rectangle(fileMenuRect.X + fileMenuRect.Width - 1, fileMenuRect.Y, 1, fileMenuRect.Height), borderColor); // Right

                // Draw Project submenu item
                var projectButtonRect = new Rectangle(10, 35, 80, 20);
                var projectButtonColor = _showProjectMenu ? Color.LightBlue : Color.White;
                texture.SetData(new[] { projectButtonColor });
                _spriteBatch.Draw(texture, projectButtonRect, Color.White);
                DrawTextSafe("Project", new Vector2(projectButtonRect.X + 5, projectButtonRect.Y + 2), Color.Black);

                // Draw separator line
                texture.SetData(new[] { Color.Gray });
                _spriteBatch.Draw(texture, new Rectangle(10, 125, 100, 1), Color.Gray);

                // Draw Exit item
                var exitButtonRect = new Rectangle(10, 130, 50, 20);
                texture.SetData(new[] { Color.White });
                _spriteBatch.Draw(texture, exitButtonRect, Color.White);
                DrawTextSafe("Exit", new Vector2(exitButtonRect.X + 5, exitButtonRect.Y + 2), Color.Black);

                // Draw Project submenu if open
                if (_showProjectMenu)
                {
                    var projectMenuRect = new Rectangle(95, 35, 80, 90);
                    texture.SetData(new[] { Color.White });
                    _spriteBatch.Draw(texture, projectMenuRect, Color.White);

                    // Draw border
                    texture.SetData(new[] { borderColor });
                    _spriteBatch.Draw(texture, new Rectangle(projectMenuRect.X, projectMenuRect.Y, projectMenuRect.Width, 1), borderColor); // Top
                    _spriteBatch.Draw(texture, new Rectangle(projectMenuRect.X, projectMenuRect.Y + projectMenuRect.Height - 1, projectMenuRect.Width, 1), borderColor); // Bottom
                    _spriteBatch.Draw(texture, new Rectangle(projectMenuRect.X, projectMenuRect.Y, 1, projectMenuRect.Height), borderColor); // Left
                    _spriteBatch.Draw(texture, new Rectangle(projectMenuRect.X + projectMenuRect.Width - 1, projectMenuRect.Y, 1, projectMenuRect.Height), borderColor); // Right

                    // Draw Create
                    var createButtonRect = new Rectangle(100, 40, 70, 20);
                    texture.SetData(new[] { Color.White });
                    _spriteBatch.Draw(texture, createButtonRect, Color.White);
                    DrawTextSafe("Create", new Vector2(createButtonRect.X + 5, createButtonRect.Y + 2), Color.Black);

                    // Draw Save
                    var saveButtonRect = new Rectangle(100, 65, 70, 20);
                    texture.SetData(new[] { Color.White });
                    _spriteBatch.Draw(texture, saveButtonRect, Color.White);
                    DrawTextSafe("Save", new Vector2(saveButtonRect.X + 5, saveButtonRect.Y + 2), Color.Black);

                    // Draw Load
                    var loadButtonRect = new Rectangle(100, 90, 70, 20);
                    texture.SetData(new[] { Color.White });
                    _spriteBatch.Draw(texture, loadButtonRect, Color.White);
                    DrawTextSafe("Load", new Vector2(loadButtonRect.X + 5, loadButtonRect.Y + 2), Color.Black);
                }
            }
        }

        private void DrawTransparentPanel(SpriteBatch spriteBatch, Rectangle rect, string title)
        {
            var texture = new Texture2D(GraphicsDevice, 1, 1);
            
            // Draw border only (no background) so teapot shows through
            var borderColor = Color.White;
            var borderThickness = 2;
            texture.SetData(new[] { borderColor });
            
            // Top border
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y, rect.Width, borderThickness), borderColor);
            // Bottom border
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y + rect.Height - borderThickness, rect.Width, borderThickness), borderColor);
            // Left border
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y, borderThickness, rect.Height), borderColor);
            // Right border
            spriteBatch.Draw(texture, new Rectangle(rect.X + rect.Width - borderThickness, rect.Y, borderThickness, rect.Height), borderColor);

            // Draw title
            if (!string.IsNullOrEmpty(title))
            {
                DrawTextSafe(title, new Vector2(rect.X + 10, rect.Y + 5), Color.White);
            }
        }

        private void DrawPanel(SpriteBatch spriteBatch, Rectangle rect, Color color, string title)
        {
            var texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { color });
            
            // Draw solid background for other panels
            spriteBatch.Draw(texture, rect, Color.White);

            // Draw border
            var borderColor = Color.White;
            var borderThickness = 2;
            texture.SetData(new[] { borderColor });
            
            // Top border
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y, rect.Width, borderThickness), borderColor);
            // Bottom border
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y + rect.Height - borderThickness, rect.Width, borderThickness), borderColor);
            // Left border
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y, borderThickness, rect.Height), borderColor);
            // Right border
            spriteBatch.Draw(texture, new Rectangle(rect.X + rect.Width - borderThickness, rect.Y, borderThickness, rect.Height), borderColor);

            // Draw title
            if (!string.IsNullOrEmpty(title))
            {
                DrawTextSafe(title, new Vector2(rect.X + 10, rect.Y + 5), Color.White);
            }
        }

        private void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            // Create a simple pixel texture for text rendering
            var texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { color });
            
            // Draw text using simple rectangles - smaller and more readable
            var charWidth = 6;
            var charHeight = 10;
            var spacing = 1;
            
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == ' ') 
                {
                    // Skip spaces
                    continue;
                }
                
                // Create a simple character representation with better spacing
                var charRect = new Rectangle(
                    (int)(position.X + i * (charWidth + spacing)), 
                    (int)position.Y, 
                    charWidth, 
                    charHeight);
                
                spriteBatch.Draw(texture, charRect, color);
            }
        }

        private void CreateProject()
        {
            try
            {
                _currentProject = new Project(Content, "NewProject.oce");
                _statusText = "New Project Created - Press F2: Save, F3: Load, ESC: Exit";
            }
            catch (Exception ex)
            {
                _statusText = $"Error creating project: {ex.Message}";
            }
        }

        private void SaveProject()
        {
            if (_currentProject == null)
            {
                _statusText = "No project to save - Press F1: Create first";
                return;
            }

            try
            {
                using (var stream = File.Create("project_save.oce"))
                using (var writer = new BinaryWriter(stream))
                {
                    _currentProject.Serialize(writer);
                }
                _statusText = "Project Saved - Press F1: Create, F3: Load, ESC: Exit";
            }
            catch (Exception ex)
            {
                _statusText = $"Error saving project: {ex.Message}";
            }
        }

        private void LoadProject()
        {
            try
            {
                if (File.Exists("project_save.oce"))
                {
                    using (var stream = File.OpenRead("project_save.oce"))
                    using (var reader = new BinaryReader(stream))
                    {
                        _currentProject = new Project();
                        _currentProject.Deserialize(reader, Content);
                    }
                    _statusText = "Project Loaded - Press F1: Create, F2: Save, ESC: Exit";
                }
                else
                {
                    _statusText = "No save file found - Press F1: Create, F2: Save, ESC: Exit";
                }
            }
            catch (Exception ex)
            {
                _statusText = $"Error loading project: {ex.Message}";
            }
        }

        private void DrawTextSafe(string text, Vector2 position, Color color)
        {
            if (_font != null)
            {
                _spriteBatch.DrawString(_font, text, position, color);
            }
            else
            {
                DrawText(_spriteBatch, text, position, color);
            }
        }
    }
}
