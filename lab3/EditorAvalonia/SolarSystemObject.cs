/*
 * Lab 3 - SolarSystemObject.cs
 * Game Development Tools Course
 * 
 * Solar system object for the editor
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EditorAvalonia
{
    public enum SolarSystemObjectType
    {
        Sun,
        Planet,
        Moon
    }

    public class SolarSystemObject
    {
        public SolarSystemObjectType Type { get; set; }
        public Model Model { get; set; }
        public Texture2D Texture { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public float RotationY { get; set; }
        public float RotationSpeed { get; set; }
        public float OrbitSpeed { get; set; }
        public float OrbitAngle { get; set; }
        public SolarSystemObject? Parent { get; set; }
        public Vector3 OriginalPosition { get; set; }

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

        public void Update()
        {
            // Update rotation
            RotationY += RotationSpeed;

            // Update orbital position if orbiting around a parent
            if (Parent != null)
            {
                OrbitAngle += OrbitSpeed;
                float radius = Vector3.Distance(OriginalPosition, Parent.Position);
                Position = Parent.Position + new Vector3(
                    (float)Math.Cos(OrbitAngle) * radius,
                    0,
                    (float)Math.Sin(OrbitAngle) * radius
                );
            }
        }

        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(Scale) *
                   Matrix.CreateRotationY(RotationY) *
                   Matrix.CreateTranslation(Position);
        }
    }
}
