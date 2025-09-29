/*
 * Lab 3 - EditorGame.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered ImGui editor with embedded MonoGame teapot rendering
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - ImGui.NET integration with MonoGame framework
 * - Editor UI layout with game view panel
 * - Embedded 3D teapot rendering in editor
 * - File operations (Create, Save, Load) integration
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using System;
using System.IO;
using System.Collections.Generic;

namespace EditorImGui
{
    public class EditorGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private ImGuiRenderer _imGuiRenderer;
        
        // Editor state
        private Project? _currentProject;
        private bool _showDemoWindow = false;
        private string _statusText = "Ready";
        
        // Teapot rendering
        private Model? _teapot;
        private Texture2D? _metalTexture;
        private BasicEffect? _basicEffect;
        private Matrix _world = Matrix.Identity;
        private Matrix _view = Matrix.CreateLookAt(new Vector3(0, 2, 2), Vector3.Zero, Vector3.Up);
        private Matrix _projection;
        private float _rotation = 0f;
        private float _movement = 0f;

        public EditorGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            Window.Title = "Our Cool Editor - ImGui + MonoGame";
        }

        protected override void Initialize()
        {
            // Initialize ImGui
            try
            {
                _imGuiRenderer = new ImGuiRenderer(this);
                _imGuiRenderer.RebuildFontAtlas();
            }
            catch
            {
                // If ImGui fails to initialize, we'll continue without it
                _imGuiRenderer = null;
            }

            // Initialize projection matrix
            _projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), 
                GraphicsDevice.Viewport.AspectRatio, 
                0.1f, 
                1000f);

            // Set up rasterizer state
            RasterizerState state = new RasterizerState();
            state.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = state;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Load teapot assets
            _teapot = CreateSphereModel(GraphicsDevice);
            _metalTexture = CreateCheckerboardTexture(GraphicsDevice);
            _basicEffect = CreateBasicShader(GraphicsDevice);

            // Apply the basic effect to the model
            if (_teapot != null && _basicEffect != null)
            {
                foreach (var mesh in _teapot.Meshes)
                {
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = _basicEffect;
                    }
                }
            }
        }

        private Model CreateSphereModel(GraphicsDevice graphicsDevice)
        {
            var vertices = new List<Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture>();
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

                    vertices.Add(new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(
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

            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture), vertices.Count, BufferUsage.WriteOnly);
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

            // Update teapot animation
            _rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * 1.0f;
            _movement += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the teapot in the background
            DrawTeapot();

            // Start ImGui frame
            if (_imGuiRenderer != null)
            {
                _imGuiRenderer.BeforeLayout(gameTime);

                // Draw editor UI
                DrawEditorUI();

                // End ImGui frame
                _imGuiRenderer.AfterLayout();
            }
        }

        private void DrawTeapot()
        {
            if (_teapot == null || _basicEffect == null || _metalTexture == null) return;

            // Apply both rotation and movement transformations
            _world = Matrix.CreateRotationY(_rotation) * 
                     Matrix.CreateTranslation(_movement * 0.5f, 0, 0);

            _basicEffect.World = _world;
            _basicEffect.View = _view;
            _basicEffect.Projection = _projection;
            _basicEffect.Texture = _metalTexture;

            foreach (var mesh in _teapot.Meshes)
            {
                mesh.Draw();
            }
        }

        private void DrawEditorUI()
        {
            try
            {
                // Main menu bar
                if (ImGui.BeginMainMenuBar())
                {
                    if (ImGui.BeginMenu("File"))
                    {
                        if (ImGui.BeginMenu("Project"))
                        {
                            if (ImGui.MenuItem("Create"))
                            {
                                CreateProject();
                            }
                            ImGui.Separator();
                            if (ImGui.MenuItem("Save"))
                            {
                                SaveProject();
                            }
                            if (ImGui.MenuItem("Load"))
                            {
                                LoadProject();
                            }
                            ImGui.EndMenu();
                        }
                        ImGui.Separator();
                        if (ImGui.MenuItem("Exit"))
                        {
                            Exit();
                        }
                        ImGui.EndMenu();
                    }
                    ImGui.EndMainMenuBar();
                }
            }
            catch
            {
                // Ignore ImGui errors
            }

            try
            {
                // Main editor window
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 20), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(1200, 760), ImGuiCond.FirstUseEver);
                
                if (ImGui.Begin("Our Cool Editor"))
                {
                // Split the window into Game View and Properties
                var gameViewSize = new System.Numerics.Vector2(ImGui.GetContentRegionAvail().X * 0.7f, ImGui.GetContentRegionAvail().Y);
                var propertiesSize = new System.Numerics.Vector2(ImGui.GetContentRegionAvail().X * 0.3f, ImGui.GetContentRegionAvail().Y);

                // Game View Panel
                if (ImGui.BeginChild("GameView", gameViewSize, true, ImGuiWindowFlags.NoScrollbar))
                {
                    ImGui.Text("Game View");
                    ImGui.Separator();
                    
                    // The teapot renders in the background, so this area shows it
                    var drawList = ImGui.GetWindowDrawList();
                    var min = ImGui.GetWindowContentRegionMin();
                    var max = ImGui.GetWindowContentRegionMax();
                    var windowPos = ImGui.GetWindowPos();
                    
                    // Draw a border to show the game view area
                    drawList.AddRect(
                        windowPos + min,
                        windowPos + max,
                        ImGui.GetColorU32(ImGuiCol.Border),
                        0.0f,
                        0,
                        2.0f);
                        
                    ImGui.Text("Texturized Teapot (Moving & Rotating)");
                    ImGui.Text($"Rotation: {_rotation:F2}");
                    ImGui.Text($"Movement: {_movement:F2}");
                }
                ImGui.EndChild();

                ImGui.SameLine();

                // Properties Panel
                if (ImGui.BeginChild("Properties", propertiesSize, true))
                {
                    ImGui.Text("Properties");
                    ImGui.Separator();
                    ImGui.Text("Inspector Panel");
                    
                    if (_currentProject != null)
                    {
                        ImGui.Text($"Project: {_currentProject.Name}");
                        ImGui.Text($"Folder: {_currentProject.Folder}");
                        ImGui.Text($"Levels: {_currentProject.Levels.Count}");
                    }
                    else
                    {
                        ImGui.Text("No Project Loaded");
                    }
                }
                ImGui.EndChild();
                }
                ImGui.End();

                // Status bar
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, ImGui.GetIO().DisplaySize.Y - 25), ImGuiCond.Always);
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(ImGui.GetIO().DisplaySize.X, 25), ImGuiCond.Always);
                
                if (ImGui.Begin("StatusBar", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar))
                {
                    ImGui.Text(_statusText);
                }
                ImGui.End();
            }
            catch
            {
                // Ignore ImGui errors
            }
        }

        private void CreateProject()
        {
            try
            {
                _currentProject = new Project(Content, "NewProject.oce");
                _statusText = "New Project Created";
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
                _statusText = "No project to save";
                return;
            }

            try
            {
                using (var stream = File.Create("project_save.oce"))
                using (var writer = new BinaryWriter(stream))
                {
                    _currentProject.Serialize(writer);
                }
                _statusText = "Project Saved";
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
                    _statusText = "Project Loaded";
                }
                else
                {
                    _statusText = "No save file found";
                }
            }
            catch (Exception ex)
            {
                _statusText = $"Error loading project: {ex.Message}";
            }
        }
    }
}
