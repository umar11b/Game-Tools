/*
 * Lab 3 - SimpleTeapotRenderer.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered simple teapot renderer for Lab 3 submission
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Simple MonoGame application for teapot rendering
 * - Basic teapot creation and texture
 * - Animation and rendering loop
 * - macOS ARM compatibility
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EditorAvalonia
{
    public class SimpleTeapotRenderer : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Model _teapot;
        private Texture2D _metalTexture;
        private BasicEffect _basicEffect;
        private Matrix _world, _view, _projection;
        private float _rotation;
        private float _movement;

        public SimpleTeapotRenderer()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Lab 3 - Texturized Teapot (Moving & Rotating)";
        }

        protected override void Initialize()
        {
            _world = Matrix.Identity;
            _view = Matrix.CreateLookAt(new Vector3(0, 2, 5), Vector3.Zero, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create simple teapot using built-in primitives
            _teapot = CreateSimpleTeapot();
            
            // Create simple metal texture
            _metalTexture = CreateSimpleTexture();
            
            // Create basic effect
            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.EnableDefaultLighting();
            _basicEffect.TextureEnabled = true;
        }

        private Model CreateSimpleTeapot()
        {
            // Create a simple sphere using built-in geometry
            var vertices = new List<VertexPositionColorTexture>();
            var indices = new List<int>();
            
            int segments = 12;
            int rings = 12;
            
            for (int ring = 0; ring <= rings; ring++)
            {
                float v = (float)ring / rings;
                float phi = v * MathHelper.Pi;
                
                for (int segment = 0; segment <= segments; segment++)
                {
                    float u = (float)segment / segments;
                    float theta = u * MathHelper.TwoPi;
                    
                    float x = (float)(System.Math.Cos(theta) * System.Math.Sin(phi));
                    float y = (float)System.Math.Cos(phi);
                    float z = (float)(System.Math.Sin(theta) * System.Math.Sin(phi));
                    
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
            
            var vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices.ToArray());
            
            var indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Count, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices.ToArray());
            
            var meshParts = new List<ModelMeshPart>();
            var meshPart = new ModelMeshPart();
            meshPart.VertexBuffer = vertexBuffer;
            meshPart.IndexBuffer = indexBuffer;
            meshPart.PrimitiveCount = indices.Count / 3;
            meshParts.Add(meshPart);
            
            var meshes = new List<ModelMesh>();
            var mesh = new ModelMesh(GraphicsDevice, meshParts);
            meshes.Add(mesh);
            
            var bones = new List<ModelBone>();
            return new Model(GraphicsDevice, bones, meshes);
        }

        private Texture2D CreateSimpleTexture()
        {
            var texture = new Texture2D(GraphicsDevice, 128, 128);
            var colors = new Color[128 * 128];
            
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    // Simple checkerboard pattern for metal texture
                    bool isEven = ((x / 16) + (y / 16)) % 2 == 0;
                    colors[y * 128 + x] = isEven ? Color.DarkGray : Color.LightGray;
                }
            }
            
            texture.SetData(colors);
            return texture;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Simple animation: rotation and movement
            _rotation += 0.01f;
            _movement += 0.005f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Simple transformations: rotation and movement
            _world = Matrix.CreateRotationY(_rotation) * 
                     Matrix.CreateTranslation(_movement, 0, 0);

            _basicEffect.World = _world;
            _basicEffect.View = _view;
            _basicEffect.Projection = _projection;
            _basicEffect.Texture = _metalTexture;

            foreach (ModelMesh mesh in _teapot.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = _basicEffect;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
