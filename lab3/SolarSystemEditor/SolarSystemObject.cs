using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemEditor
{
    /// <summary>
    /// Enumeration for different types of solar system objects
    /// </summary>
    public enum SolarSystemObjectType
    {
        Sun,
        Planet,
        Moon
    }

    /// <summary>
    /// Represents a solar system object with orbital mechanics and rendering capabilities
    /// </summary>
    public class SolarSystemObject
    {
        // Object properties
        public SolarSystemObjectType Type { get; set; }
        public Model Model { get; set; }
        public Texture2D Texture { get; set; }
        
        // Transform properties
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public float RotationY { get; set; }
        
        // Animation properties
        public float RotationSpeed { get; set; }
        public float OrbitSpeed { get; set; }
        public float OrbitAngle { get; set; }
        
        // Hierarchy properties
        public SolarSystemObject? Parent { get; set; }
        public Vector3 OriginalPosition { get; set; }

        /// <summary>
        /// Creates a new solar system object
        /// </summary>
        /// <param name="type">Type of object (Sun, Planet, Moon)</param>
        /// <param name="model">3D model to render</param>
        /// <param name="texture">Texture to apply to the model</param>
        public SolarSystemObject(SolarSystemObjectType type, Model model, Texture2D texture)
        {
            Type = type;
            Model = model;
            Texture = texture;
            Position = Vector3.Zero;
            Scale = Vector3.One;
            RotationY = 0f;
            RotationSpeed = 0f;
            OrbitSpeed = 0f;
            OrbitAngle = 0f;
            Parent = null;
            OriginalPosition = Vector3.Zero;
        }

        /// <summary>
        /// Updates the object's position and rotation based on orbital mechanics
        /// </summary>
        public void Update()
        {
            // Update rotation around own axis
            RotationY += RotationSpeed;

            // Update orbital position if orbiting around a parent
            if (Parent != null)
            {
                OrbitAngle += OrbitSpeed;
                float radius = Vector3.Distance(OriginalPosition, Parent.Position);
                
                // Calculate new orbital position
                Position = Parent.Position + new Vector3(
                    (float)Math.Cos(OrbitAngle) * radius,
                    0,
                    (float)Math.Sin(OrbitAngle) * radius
                );
            }
        }

        /// <summary>
        /// Gets the world transformation matrix for rendering
        /// </summary>
        /// <returns>World matrix combining scale, rotation, and translation</returns>
        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(Scale) *
                   Matrix.CreateRotationY(RotationY) *
                   Matrix.CreateTranslation(Position);
        }

        /// <summary>
        /// Gets a string representation of the object
        /// </summary>
        /// <returns>String describing the object</returns>
        public override string ToString()
        {
            return $"{Type} at {Position} (Scale: {Scale}, Rotation: {RotationY:F2})";
        }
    }
}
