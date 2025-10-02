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
using System.Collections.Generic;
using System.Linq;

namespace EditorAvalonia
{
    public class GameEditor : Game
    {
        // Member variables (following slide example exactly)
        internal Project? Project { get; set; }
        private GraphicsDeviceManager m_graphics;
        private MainWindow? m_parent;
        
        // Solar System Editor
        private List<SolarSystemObject> solarSystemObjects;
        private SolarSystemObject? sun;
        private List<SolarSystemObject> planets;
        private List<SolarSystemObject> moons;
        private Model sunModel, planetModel, moonModel;
        private Texture2D sunTexture, planetTexture, moonTexture;
        private Effect myShader;
        private Random random;
        
        public GameEditor()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // Initialize solar system
            solarSystemObjects = new List<SolarSystemObject>();
            planets = new List<SolarSystemObject>();
            moons = new List<SolarSystemObject>();
            random = new Random();
        }

        public GameEditor(MainWindow _parent) : this()
        {
            m_parent = _parent;
            // Enable MonoGame graphics for proper integration
            m_graphics.PreferredBackBufferWidth = 800;
            m_graphics.PreferredBackBufferHeight = 600;
            m_graphics.ApplyChanges();
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
            
            // Load solar system models and textures
            try
            {
                sunModel = Content.Load<Model>("Sun");
                planetModel = Content.Load<Model>("World");
                moonModel = Content.Load<Model>("Moon");
                
                sunTexture = Content.Load<Texture2D>("SunDiffuse");
                planetTexture = Content.Load<Texture2D>("WorldDiffuse");
                moonTexture = Content.Load<Texture2D>("MoonDiffuse");
                
                myShader = Content.Load<Effect>("MyShader");
                
                Console.WriteLine("Solar system models and textures loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading solar system content: {ex.Message}");
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // Update all solar system objects
            foreach (var obj in solarSystemObjects)
            {
                obj.Update();
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear with black background as required
            GraphicsDevice.Clear(Color.Black);
            
            // Set up camera at position (0, 0, 300) as required
            var cameraPosition = new Vector3(0, 0, 300);
            var view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), 
                GraphicsDevice.Viewport.AspectRatio, 
                0.1f, 
                1000f
            );
            
            // Render solar system objects
            foreach (var obj in solarSystemObjects)
            {
                RenderSolarSystemObject(obj, view, projection);
            }
            
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
        
        // Solar System Editor Methods
        public void AddSun()
        {
            if (sun == null && sunModel != null && sunTexture != null)
            {
                sun = new SolarSystemObject(SolarSystemObjectType.Sun, sunModel, sunTexture)
                {
                    Position = Vector3.Zero,
                    Scale = new Vector3(2f),
                    RotationSpeed = 0.005f
                };
                solarSystemObjects.Add(sun);
                Console.WriteLine("Sun added to solar system");
            }
        }
        
        public void AddPlanet()
        {
            if (planets.Count < 5 && planetModel != null && planetTexture != null)
            {
                var planet = new SolarSystemObject(SolarSystemObjectType.Planet, planetModel, planetTexture)
                {
                    Position = new Vector3(
                        random.Next(-150, 151),
                        random.Next(-90, 91),
                        0
                    ),
                    Scale = new Vector3(0.75f),
                    RotationSpeed = (float)(random.NextDouble() * 0.01 + 0.02), // 0.02 to 0.03
                    OrbitSpeed = (float)(random.NextDouble() * 0.001 + 0.001), // 0.001 to 0.002
                    Parent = sun
                };
                planet.OriginalPosition = planet.Position;
                planets.Add(planet);
                solarSystemObjects.Add(planet);
                Console.WriteLine($"Planet {planets.Count} added to solar system");
            }
        }
        
        public void AddMoon()
        {
            if (moonModel != null && moonTexture != null)
            {
                foreach (var planet in planets)
                {
                    var moon = new SolarSystemObject(SolarSystemObjectType.Moon, moonModel, moonTexture)
                    {
                        Position = new Vector3(
                            planet.Position.X + 20,
                            planet.Position.Y,
                            planet.Position.Z
                        ),
                        Scale = new Vector3((float)(random.NextDouble() * 0.2 + 0.2)), // 0.2 to 0.4
                        RotationSpeed = (float)(random.NextDouble() * 0.005 + 0.005), // 0.005 to 0.01
                        OrbitSpeed = (float)(random.NextDouble() * 0.01 + 0.01), // 0.01 to 0.02
                        Parent = planet
                    };
                    moon.OriginalPosition = moon.Position;
                    moons.Add(moon);
                    solarSystemObjects.Add(moon);
                }
                Console.WriteLine($"Moons added for all {planets.Count} planets");
            }
        }
        
        private void RenderSolarSystemObject(SolarSystemObject obj, Matrix view, Matrix projection)
        {
            if (obj.Model == null) return;
            
            var world = obj.GetWorldMatrix();
            
            foreach (ModelMesh mesh in obj.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (myShader != null)
                    {
                        myShader.Parameters["World"].SetValue(world);
                        myShader.Parameters["View"].SetValue(view);
                        myShader.Parameters["Projection"].SetValue(projection);
                        myShader.Parameters["Texture"].SetValue(obj.Texture);
                        part.Effect = myShader;
                    }
                }
                mesh.Draw();
            }
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
