/*
 * Lab 3 - Level.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered scene management and model rendering for MonoGame
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Scene graph structure and model management
 * - Camera integration for view and projection
 * - Content loading for models, textures, and shaders
 * - Rendering loop implementation
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;

namespace EditorImGui
{
    internal class Level : ISerializable
    {
        // Accessors (following slide example)
        public Camera GetCamera() { return m_camera; }

        // Members (following slide example)
        private List<Models> m_models = new();
        private Camera m_camera = new(new Vector3(0, 2, 2), 16 / 9);

        public Level()
        {
        }

        public void LoadContent(ContentManager _content)
        {
            try
            {
                // Create a procedural teapot since we have placeholder files
                Models teapot = CreateProceduralTeapot(_content);
                AddModel(teapot);
            }
            catch (System.ArgumentNullException)
            {
                // GraphicsDevice not available - skip 3D content creation
                // Level is created but without models (for macOS ARM compatibility)
            }
        }

        private Models CreateProceduralTeapot(ContentManager _content)
        {
            // Create a simple procedural teapot using basic geometry
            var teapot = new Models();
            
            // Create a simple sphere as our "teapot" for now
            var sphere = CreateSphereModel(_content);
            var metalTexture = CreateMetalTexture(_content);
            var basicShader = CreateBasicShader(_content);
            
            teapot.Mesh = sphere;
            teapot.Texture = metalTexture;
            teapot.Shader = basicShader;
            teapot.Position = Vector3.Zero;
            teapot.Scale = 1.0f;
            
            return teapot;
        }

        private Model CreateSphereModel(ContentManager _content)
        {
            // Create a simple sphere model procedurally
            var graphicsDevice = _content.ServiceProvider.GetService(typeof(GraphicsDevice)) as GraphicsDevice;
            
            var vertices = new List<VertexPositionColorTexture>();
            var indices = new List<int>();
            
            // Create a simple sphere
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
            
            // Create indices for triangles
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
            
            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices.ToArray());
            
            var indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Count, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices.ToArray());
            
            // Create a simple model using proper constructors
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

        private Texture2D CreateMetalTexture(ContentManager _content)
        {
            var graphicsDevice = _content.ServiceProvider.GetService(typeof(GraphicsDevice)) as GraphicsDevice;
            var texture = new Texture2D(graphicsDevice, 256, 256);
            
            // Create a simple metal-like texture procedurally
            var colors = new Color[256 * 256];
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    float noise = (float)(Math.Sin(x * 0.1f) * Math.Cos(y * 0.1f) + 
                                         Math.Sin(x * 0.05f) * Math.Cos(y * 0.05f) * 0.5f);
                    float intensity = (noise + 1.0f) * 0.5f;
                    
                    colors[y * 256 + x] = new Color(
                        (int)(128 + intensity * 64),
                        (int)(128 + intensity * 64),
                        (int)(128 + intensity * 64),
                        255
                    );
                }
            }
            
            texture.SetData(colors);
            return texture;
        }

        private Effect CreateBasicShader(ContentManager _content)
        {
            var graphicsDevice = _content.ServiceProvider.GetService(typeof(GraphicsDevice)) as GraphicsDevice;
            
            // Create a simple basic effect
            var effect = new BasicEffect(graphicsDevice);
            effect.EnableDefaultLighting();
            effect.TextureEnabled = true;
            
            return effect;
        }

        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }

        public void Render()
        {
            foreach (Models m in m_models)
            {
                m.Render(m_camera.View, m_camera.Projection);
            }
        }

        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(m_models.Count);
            foreach (var model in m_models)
            {
                model.Serialize(_stream);
            }
            m_camera.Serialize(_stream);
        }

        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            int modelCount = _stream.ReadInt32();
            for (int count = 0; count < modelCount; count++)
            {
                Models m = new();
                m.Deserialize(_stream, _content);
                m_models.Add(m);
            }
            m_camera.Deserialize(_stream, _content);
        }
    }
}
