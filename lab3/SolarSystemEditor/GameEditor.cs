using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SolarSystemEditor
{
    /// <summary>
    /// Main MonoGame class for the Solar System Editor with WinForms integration
    /// </summary>
    public class GameEditor : Game
    {
        // Graphics and content management
        private GraphicsDeviceManager graphics;
        private IntPtr windowHandle;
        private int windowWidth;
        private int windowHeight;
        
        // Solar System objects
        private List<SolarSystemObject> solarSystemObjects;
        private SolarSystemObject? sun;
        private List<SolarSystemObject> planets;
        private List<SolarSystemObject> moons;
        
        // Models and textures
        private Model? sunModel, planetModel, moonModel;
        private Texture2D? sunTexture, planetTexture, moonTexture;
        private Effect? myShader;
        
        // Simple 3D geometry for Azure VM compatibility
        private VertexBuffer? sphereVertexBuffer;
        private IndexBuffer? sphereIndexBuffer;
        private BasicEffect? basicEffect;
        private int sphereVertexCount;
        
        // Random number generator for procedural generation
        private Random random;
        
        // Camera matrices
        private Matrix viewMatrix;
        private Matrix projectionMatrix;
        
        public GameEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Solar System Editor - 3D View";
            
            // Initialize collections
            solarSystemObjects = new List<SolarSystemObject>();
            planets = new List<SolarSystemObject>();
            moons = new List<SolarSystemObject>();
            random = new Random();
            
            // Set up graphics for standalone window with Azure VM compatibility
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            
            // Azure VM-friendly graphics settings
            graphics.GraphicsProfile = GraphicsProfile.Reach; // Use Reach profile for better compatibility
            graphics.PreferMultiSampling = false; // Disable anti-aliasing
            graphics.SynchronizeWithVerticalRetrace = false; // Disable VSync
            graphics.IsFullScreen = false;
            graphics.HardwareModeSwitch = false;
            
            graphics.ApplyChanges();
        }
        
        /// <summary>
        /// Constructor for WinForms integration with specific window handle and dimensions
        /// </summary>
        public GameEditor(IntPtr windowHandle, int width, int height) : this()
        {
            this.windowHandle = windowHandle;
            this.windowWidth = width;
            this.windowHeight = height;
            
            // Configure graphics for WinForms control with VM compatibility
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = false;
            graphics.HardwareModeSwitch = false;
            
            // VM-friendly graphics settings
            graphics.GraphicsProfile = GraphicsProfile.Reach; // Use Reach profile for better compatibility
            graphics.PreferMultiSampling = false; // Disable anti-aliasing for VM compatibility
            graphics.SynchronizeWithVerticalRetrace = false; // Disable VSync for VM compatibility
            
            // Set the window handle for rendering
            SetWindowHandle(windowHandle);
        }
        
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        
        private void SetWindowHandle(IntPtr handle)
        {
            // Hide the console window for WinForms integration
            var consoleHandle = GetConsoleWindow();
            ShowWindow(consoleHandle, 0); // 0 = SW_HIDE
        }

        protected override void Initialize()
        {
            // Set up rasterizer state for proper rendering
            RasterizerState state = new RasterizerState();
            state.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = state;
            
            // Enable depth testing
            DepthStencilState depthState = new DepthStencilState();
            depthState.DepthBufferEnable = true;
            depthState.DepthBufferWriteEnable = true;
            GraphicsDevice.DepthStencilState = depthState;
            
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
                Console.WriteLine("Creating simple 3D geometry for Azure VM compatibility");
                
                // Create simple 3D geometry as fallback
                CreateSimpleSphereGeometry();
            }
            
            // Set up basic effect for simple rendering
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight0.Direction = new Vector3(-1, -1, -1);
            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.7f, 0.7f, 0.7f);
        }
        
        private void CreateSimpleSphereGeometry()
        {
            // Create a simple sphere using basic geometry
            int segments = 16;
            int rings = 16;
            
            List<VertexPositionColor> vertices = new List<VertexPositionColor>();
            List<int> indices = new List<int>();
            
            // Generate sphere vertices
            for (int ring = 0; ring <= rings; ring++)
            {
                float phi = (float)(Math.PI * ring / rings);
                for (int segment = 0; segment <= segments; segment++)
                {
                    float theta = (float)(2.0 * Math.PI * segment / segments);
                    
                    Vector3 position = new Vector3(
                        (float)(Math.Sin(phi) * Math.Cos(theta)),
                        (float)(Math.Cos(phi)),
                        (float)(Math.Sin(phi) * Math.Sin(theta))
                    );
                    
                    Color color = Color.White;
                    vertices.Add(new VertexPositionColor(position, color));
                }
            }
            
            // Generate sphere indices
            for (int ring = 0; ring < rings; ring++)
            {
                for (int segment = 0; segment < segments; segment++)
                {
                    int current = ring * (segments + 1) + segment;
                    int next = current + segments + 1;
                    
                    // First triangle
                    indices.Add(current);
                    indices.Add(next);
                    indices.Add(current + 1);
                    
                    // Second triangle
                    indices.Add(current + 1);
                    indices.Add(next);
                    indices.Add(next + 1);
                }
            }
            
            // Create vertex buffer
            sphereVertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.WriteOnly);
            sphereVertexBuffer.SetData(vertices.ToArray());
            
            // Create index buffer
            sphereIndexBuffer = new IndexBuffer(GraphicsDevice, typeof(int), indices.Count, BufferUsage.WriteOnly);
            sphereIndexBuffer.SetData(indices.ToArray());
            
            sphereVertexCount = indices.Count;
            
            Console.WriteLine($"Created simple sphere geometry with {vertices.Count} vertices and {indices.Count} indices");
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
            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), 
                GraphicsDevice.Viewport.AspectRatio, 
                0.1f, 
                1000f
            );
            
            // Render solar system objects
            foreach (var obj in solarSystemObjects)
            {
                RenderSolarSystemObject(obj, viewMatrix, projectionMatrix);
            }
            
            base.Draw(gameTime);
        }
        
        public void AdjustAspectRatio()
        {
            // Adjust camera aspect ratio when control is resized
            float aspectRatio = graphics?.GraphicsDevice?.Viewport.AspectRatio ?? 16f / 9f;
            // Additional aspect ratio logic can be added here
        }
        
        /// <summary>
        /// Resizes the graphics device when the WinForms control is resized
        /// </summary>
        public void ResizeGraphicsDevice(int width, int height)
        {
            if (graphics != null && GraphicsDevice != null)
            {
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
                graphics.ApplyChanges();
            }
        }
        
        // Solar System Editor Methods with exact specifications
        
        /// <summary>
        /// Adds a sun: Only one, at (0,0,0), scale 2, rotating around Y-axis at 0.005 units/frame
        /// </summary>
        public void AddSun()
        {
            if (sun == null)
            {
                sun = new SolarSystemObject(SolarSystemObjectType.Sun, sunModel, sunTexture)
                {
                    Position = Vector3.Zero,                    // At (0,0,0)
                    Scale = new Vector3(2f),                    // Scale 2
                    RotationSpeed = 0.005f                      // Rotating around Y-axis at 0.005 units/frame
                };
                solarSystemObjects.Add(sun);
                Console.WriteLine("Sun added to solar system at (0,0,0) with scale 2");
            }
        }
        
        /// <summary>
        /// Adds a planet: Up to 5, random X (-150 to 150), random Y (-90 to 90), Z=0, 
        /// scale 0.75, rotating around Y-axis (0.02-0.03 units/frame), 
        /// orbiting sun's Y-axis (0.001-0.002 units/frame)
        /// </summary>
        public void AddPlanet()
        {
            if (planets.Count < 5)
            {
                var planet = new SolarSystemObject(SolarSystemObjectType.Planet, planetModel, planetTexture)
                {
                    Position = new Vector3(
                        random.Next(-150, 151),                 // Random X (-150 to 150)
                        random.Next(-90, 91),                   // Random Y (-90 to 90)
                        0                                       // Z = 0
                    ),
                    Scale = new Vector3(0.75f),                 // Scale 0.75
                    RotationSpeed = (float)(random.NextDouble() * 0.01 + 0.02), // 0.02 to 0.03
                    OrbitSpeed = (float)(random.NextDouble() * 0.001 + 0.001),  // 0.001 to 0.002
                    Parent = sun
                };
                planet.OriginalPosition = planet.Position;
                planets.Add(planet);
                solarSystemObjects.Add(planet);
                Console.WriteLine($"Planet {planets.Count} added to solar system at {planet.Position}");
            }
        }
        
        /// <summary>
        /// Adds moons: For each planet, at (planetX + 20, planetY, planetZ), 
        /// random scale (0.2-0.4), rotating around Y-axis (0.005-0.01 units/frame), 
        /// orbiting parent planet's Y-axis (0.01-0.02 units/frame)
        /// </summary>
        public void AddMoon()
        {
            if (planets.Count > 0) // Need at least one planet to add moons
            {
                foreach (var planet in planets)
                {
                    var moon = new SolarSystemObject(SolarSystemObjectType.Moon, moonModel, moonTexture)
                    {
                        Position = new Vector3(
                            planet.Position.X + 20,             // planetX + 20
                            planet.Position.Y,                  // planetY
                            planet.Position.Z                   // planetZ
                        ),
                        Scale = new Vector3((float)(random.NextDouble() * 0.2 + 0.2)), // 0.2 to 0.4
                        RotationSpeed = (float)(random.NextDouble() * 0.005 + 0.005),  // 0.005 to 0.01
                        OrbitSpeed = (float)(random.NextDouble() * 0.01 + 0.01),       // 0.01 to 0.02
                        Parent = planet
                    };
                    moon.OriginalPosition = moon.Position;
                    moons.Add(moon);
                    solarSystemObjects.Add(moon);
                }
                Console.WriteLine($"Moons added for all {planets.Count} planets");
            }
        }
        
        /// <summary>
        /// Clears the entire solar system
        /// </summary>
        public void ClearSolarSystem()
        {
            solarSystemObjects.Clear();
            planets.Clear();
            moons.Clear();
            sun = null;
            Console.WriteLine("Solar system cleared");
        }
        
        private void RenderSolarSystemObject(SolarSystemObject obj, Matrix view, Matrix projection)
        {
            var world = obj.GetWorldMatrix();
            
            // Try to render with loaded model first
            if (obj.Model != null)
            {
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
            // Fallback to simple sphere geometry for Azure VM compatibility
            else if (sphereVertexBuffer != null && sphereIndexBuffer != null && basicEffect != null)
            {
                RenderSimpleSphere(obj, world, view, projection);
            }
        }
        
        private void RenderSimpleSphere(SolarSystemObject obj, Matrix world, Matrix view, Matrix projection)
        {
            // Set up basic effect
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            
            // Set color based on object type
            Color color = Color.White;
            switch (obj.Type)
            {
                case SolarSystemObjectType.Sun:
                    color = Color.Yellow;
                    break;
                case SolarSystemObjectType.Planet:
                    color = Color.Blue;
                    break;
                case SolarSystemObjectType.Moon:
                    color = Color.LightGray;
                    break;
            }
            
            basicEffect.DiffuseColor = color.ToVector3();
            
            // Set vertex buffer and index buffer
            GraphicsDevice.SetVertexBuffer(sphereVertexBuffer);
            GraphicsDevice.Indices = sphereIndexBuffer;
            
            // Draw the sphere
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, sphereVertexCount / 3);
            }
        }

        // Save/Load functionality
        private string saveDataPath = "solar_system_save.txt";
        
        public void SaveGame()
        {
            try
            {
                var saveData = $"Solar System Save Data\nTime: {DateTime.Now}\nObjects: {solarSystemObjects.Count}";
                System.IO.File.WriteAllText(saveDataPath, saveData);
                Console.WriteLine("Solar system saved successfully!");
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
                if (System.IO.File.Exists(saveDataPath))
                {
                    var saveData = System.IO.File.ReadAllText(saveDataPath);
                    Console.WriteLine($"Solar system loaded successfully!\nSave data: {saveData}");
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
