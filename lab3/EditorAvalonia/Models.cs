/*
 * Lab 3 - Models.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered 3D model management and rendering for MonoGame
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Model properties (mesh, texture, shader, transformations)
 * - Shader application to model parts
 * - World matrix calculation (scale, rotation, translation)
 * - Model rendering loop and serialization
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace EditorAvalonia
{
    class Models : ISerializable
    {
        // Accessors (following slide example)
        public Model Mesh { get; set; }
        public Texture Texture { get; set; }
        public Effect Shader { get; set; }
        public Vector3 Position { get => m_position; set { m_position = value; } }
        public Vector3 Rotation { get => m_rotation; set { m_rotation = value; } }
        public float Scale { get; set; }

        // Members (following slide example)
        private Vector3 m_position;
        private Vector3 m_rotation;

        // Constructor (following slide example)
        public Models()
        {
        }

        public Models(ContentManager _content, string _model, string _texture, string _effect, Vector3 _position, float _scale)
        {
            Create(_content, _model, _texture, _effect, _position, _scale);
        }

        public void Create(ContentManager _content, string _model, string _texture, string _effect, Vector3 _position, float _scale)
        {
            Mesh = _content.Load<Model>(_model);
            Mesh.Tag = _model;
            Texture = _content.Load<Texture>(_texture);
            Texture.Tag = _texture;
            Shader = _content.Load<Effect>(_effect);
            Shader.Tag = _effect;
            SetShader(Shader);
            m_position = _position;
            Scale = _scale;
        }

        public void SetShader(Effect _effect)
        {
            Shader = _effect;
            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Shader;
                }
            }
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateScale(Scale) *
                   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                   Matrix.CreateTranslation(Position);
        }

        public void Render(Matrix _view, Matrix _projection)
        {
            // Handle BasicEffect vs custom shader
            if (Shader is BasicEffect basicEffect)
            {
                basicEffect.World = GetTransform();
                basicEffect.View = _view;
                basicEffect.Projection = _projection;
                basicEffect.Texture = Texture as Texture2D;
            }
            else
            {
                // Custom shader parameters
                Shader.Parameters["World"]?.SetValue(GetTransform());
                Shader.Parameters["WorldViewProjection"]?.SetValue(GetTransform() * _view * _projection);
                Shader.Parameters["Texture"]?.SetValue(Texture);
            }

            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                mesh.Draw();
            }
        }

        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(Mesh.Tag.ToString());
            _stream.Write(Texture.Tag.ToString());
            _stream.Write(Shader.Tag.ToString());
            HelpSerialize.Vec3(_stream, Position);
            HelpSerialize.Vec3(_stream, Rotation);
            _stream.Write(Scale);
        }

        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            string mesh = _stream.ReadString();
            string texture = _stream.ReadString();
            string shader = _stream.ReadString();
            Position = HelpDeserialize.Vec3(_stream);
            Rotation = HelpDeserialize.Vec3(_stream);
            Scale = _stream.ReadSingle();
            Create(_content, mesh, texture, shader, Position, Scale);
        }
    }
}