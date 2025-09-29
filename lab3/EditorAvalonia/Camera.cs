/*
 * Lab 3 - Camera.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered 3D camera implementation for MonoGame
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - 3D camera mathematics and matrix calculations
 * - MonoGame camera implementation patterns
 * - View and projection matrix setup
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace EditorAvalonia
{
    internal class Camera : ISerializable
    {
        // Accessors (following slide example)
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        public Matrix View { get; set; } = Matrix.Identity;
        public Matrix Projection { get; set; } = Matrix.Identity;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 1000f;
        public float AspectRatio { get; set; } = 16 / 9;

        // Constructor (following slide example)
        public Camera()
        {
        }

        public Camera(Vector3 _position, float _aspectRatio)
        {
            Update(_position, _aspectRatio);
        }

        // Update method (following slide example)
        public void Update(Vector3 _position, float _aspectRatio)
        {
            Position = _position;
            AspectRatio = _aspectRatio;
            
            // Create look-at view matrix
            View = Matrix.CreateLookAt(Position, new Vector3(0, 0, 0), Vector3.Up);
            
            // Create perspective projection matrix
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), 
                AspectRatio, 
                NearPlane, 
                FarPlane);
        }

        // Serialization methods (following slide example)
        public void Serialize(BinaryWriter _stream)
        {
            HelpSerialize.Vec3(_stream, Position);
            _stream.Write(NearPlane);
            _stream.Write(FarPlane);
            _stream.Write(AspectRatio);
        }

        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            Position = HelpDeserialize.Vec3(_stream);
            NearPlane = _stream.ReadSingle();
            FarPlane = _stream.ReadSingle();
            AspectRatio = _stream.ReadSingle();
            Update(Position, AspectRatio);
        }
    }
}
