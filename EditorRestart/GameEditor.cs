/*
 * GameEditor - MonoGame Game Class for Solar System Editor
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace EditorRestart
{
    public class GameEditor : Game
    {
        // Windows API for hiding console
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private GraphicsDeviceManager graphics;
        private IntPtr windowHandle;
        private int windowWidth;
        private int windowHeight;

        // Solar System Objects
        private List<SolarSystemObject> solarSystemObjects;
        private SolarSystemObject? sun;
        private List<SolarSystemObject> planets;
        private List<SolarSystemObject> moons;

        // Models and textures
        private Model? sunModel, planetModel, moonModel;
        private Texture2D? sunTexture, planetTexture, moonTexture;
        private Effect? myShader;

        // Camera matrices
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Random random;

        // Constructor for WinForms integration
        public GameEditor(IntPtr handle, int width, int height)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            windowHandle = handle;
            windowWidth = width;
            windowHeight = height;

            // Configure graphics for WinForms control
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = false;
            graphics.HardwareModeSwitch = false;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            // Initialize collections
            solarSystemObjects = new List<SolarSystemObject>();
            planets = new List<SolarSystemObject>();
            moons = new List<SolarSystemObject>();
            random = new Random();

            // Set window handle for WinForms integration
            SetWindowHandle(handle);
        }

        private void SetWindowHandle(IntPtr handle)
        {
            // Hide console window
            var consoleHandle = GetConsoleWindow();
            ShowWindow(consoleHandle, 0); // 0 = SW_HIDE

            // Set the window handle for rendering to WinForms control
            typeof(Game).GetField("m_handle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .SetValue(this, handle);
        }

        protected override void Initialize()
        {
            // Enable depth testing
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            // Set rasterizer state
            RasterizerState state = new RasterizerState();
            state.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = state;

            base.Initialize();
        }

        protected override void LoadContent()
        {
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
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            // Set up camera at position (0, 0, 300) as required
            var cameraPosition = new Vector3(0, 0, 300);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                1000f
            );

            // Render all solar system objects
            foreach (var obj in solarSystemObjects)
            {
                RenderSolarSystemObject(obj, obj.GetWorldMatrix(), viewMatrix, projectionMatrix);
            }

            base.Draw(gameTime);
        }

        private void RenderSolarSystemObject(SolarSystemObject obj, Matrix world, Matrix view, Matrix projection)
        {
            if (obj.Model == null || obj.Texture == null)
                return;

            // Render with shader if available
            if (myShader != null)
            {
                foreach (var mesh in obj.Model.Meshes)
                {
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = myShader;
                        myShader.Parameters["World"]?.SetValue(mesh.ParentBone.Transform * world);
                        myShader.Parameters["View"]?.SetValue(view);
                        myShader.Parameters["Projection"]?.SetValue(projection);
                        myShader.Parameters["ModelTexture"]?.SetValue(obj.Texture);
                    }
                    mesh.Draw();
                }
            }
            else
            {
                // Fallback to basic rendering
                foreach (var mesh in obj.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = mesh.ParentBone.Transform * world;
                        effect.View = view;
                        effect.Projection = projection;
                        effect.TextureEnabled = true;
                        effect.Texture = obj.Texture;
                        effect.LightingEnabled = true;
                        effect.EnableDefaultLighting();
                    }
                    mesh.Draw();
                }
            }
        }

        public void ResizeViewport(int width, int height)
        {
            windowWidth = width;
            windowHeight = height;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        // Solar System Editor Methods
        public void AddSun()
        {
            if (sun != null)
            {
                Console.WriteLine("Sun already exists!");
                return;
            }

            sun = new SolarSystemObject(SolarSystemObjectType.Sun, sunModel, sunTexture)
            {
                Position = Vector3.Zero,
                Scale = new Vector3(20, 20, 20),
                RotationSpeed = 0.01f
            };

            solarSystemObjects.Add(sun);
            Console.WriteLine("Sun added to solar system");
        }

        public void AddPlanet()
        {
            if (sun == null)
            {
                Console.WriteLine("Please add a sun first!");
                return;
            }

            float orbitRadius = 50 + (planets.Count * 30);
            float angle = random.Next(0, 360) * (float)(Math.PI / 180.0);

            var planet = new SolarSystemObject(SolarSystemObjectType.Planet, planetModel, planetTexture)
            {
                Position = new Vector3(
                    (float)Math.Cos(angle) * orbitRadius,
                    0,
                    (float)Math.Sin(angle) * orbitRadius
                ),
                OriginalPosition = new Vector3(
                    (float)Math.Cos(angle) * orbitRadius,
                    0,
                    (float)Math.Sin(angle) * orbitRadius
                ),
                Scale = new Vector3(8, 8, 8),
                RotationSpeed = 0.02f,
                OrbitSpeed = 0.005f / (1 + planets.Count * 0.5f),
                OrbitAngle = angle,
                Parent = sun
            };

            planets.Add(planet);
            solarSystemObjects.Add(planet);
            Console.WriteLine($"Planet added to solar system at orbit radius {orbitRadius}");
        }

        public void AddMoon()
        {
            if (planets.Count == 0)
            {
                Console.WriteLine("Please add at least one planet first!");
                return;
            }

            // Add moon to the last planet
            var parentPlanet = planets[planets.Count - 1];
            float orbitRadius = 15;
            float angle = random.Next(0, 360) * (float)(Math.PI / 180.0);

            var moon = new SolarSystemObject(SolarSystemObjectType.Moon, moonModel, moonTexture)
            {
                Position = parentPlanet.Position + new Vector3(
                    (float)Math.Cos(angle) * orbitRadius,
                    0,
                    (float)Math.Sin(angle) * orbitRadius
                ),
                OriginalPosition = new Vector3(
                    (float)Math.Cos(angle) * orbitRadius,
                    0,
                    (float)Math.Sin(angle) * orbitRadius
                ),
                Scale = new Vector3(3, 3, 3),
                RotationSpeed = 0.03f,
                OrbitSpeed = 0.02f,
                OrbitAngle = angle,
                Parent = parentPlanet
            };

            moons.Add(moon);
            solarSystemObjects.Add(moon);
            Console.WriteLine($"Moon added to planet");
        }

        // Save/Load functionality
        public void SaveGame()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("solar_system_save.txt"))
                {
                    writer.WriteLine($"Objects:{solarSystemObjects.Count}");
                    foreach (var obj in solarSystemObjects)
                    {
                        writer.WriteLine($"Type:{obj.Type}");
                        writer.WriteLine($"Position:{obj.Position.X},{obj.Position.Y},{obj.Position.Z}");
                        writer.WriteLine($"Scale:{obj.Scale.X},{obj.Scale.Y},{obj.Scale.Z}");
                        writer.WriteLine($"Rotation:{obj.RotationY}");
                    }
                }
                Console.WriteLine("Game saved successfully");
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
                if (!File.Exists("solar_system_save.txt"))
                {
                    Console.WriteLine("No save file found");
                    return;
                }

                // Clear existing objects
                solarSystemObjects.Clear();
                planets.Clear();
                moons.Clear();
                sun = null;

                using (StreamReader reader = new StreamReader("solar_system_save.txt"))
                {
                    string? line = reader.ReadLine();
                    if (line != null && line.StartsWith("Objects:"))
                    {
                        int count = int.Parse(line.Split(':')[1]);
                        for (int i = 0; i < count; i++)
                        {
                            string? typeLine = reader.ReadLine();
                            string? posLine = reader.ReadLine();
                            string? scaleLine = reader.ReadLine();
                            string? rotLine = reader.ReadLine();

                            if (typeLine != null && posLine != null && scaleLine != null && rotLine != null)
                            {
                                var type = (SolarSystemObjectType)Enum.Parse(typeof(SolarSystemObjectType), typeLine.Split(':')[1]);
                                var posValues = posLine.Split(':')[1].Split(',');
                                var scaleValues = scaleLine.Split(':')[1].Split(',');
                                float rotation = float.Parse(rotLine.Split(':')[1]);

                                Model? model = type == SolarSystemObjectType.Sun ? sunModel :
                                               type == SolarSystemObjectType.Planet ? planetModel : moonModel;
                                Texture2D? texture = type == SolarSystemObjectType.Sun ? sunTexture :
                                                    type == SolarSystemObjectType.Planet ? planetTexture : moonTexture;

                                var obj = new SolarSystemObject(type, model, texture)
                                {
                                    Position = new Vector3(float.Parse(posValues[0]), float.Parse(posValues[1]), float.Parse(posValues[2])),
                                    Scale = new Vector3(float.Parse(scaleValues[0]), float.Parse(scaleValues[1]), float.Parse(scaleValues[2])),
                                    RotationY = rotation
                                };

                                solarSystemObjects.Add(obj);
                                if (type == SolarSystemObjectType.Sun) sun = obj;
                                else if (type == SolarSystemObjectType.Planet) planets.Add(obj);
                                else moons.Add(obj);
                            }
                        }
                    }
                }
                Console.WriteLine("Game loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
            }
        }

        public void NewProject()
        {
            solarSystemObjects.Clear();
            planets.Clear();
            moons.Clear();
            sun = null;
            Console.WriteLine("New project created");
        }
    }
}

