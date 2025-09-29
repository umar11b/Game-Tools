/*
 * Lab 3 - Helpers.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered serialization helpers for MonoGame Vector3
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Static helper class structure for serialization
 * - Vector3 binary read/write logic
 * - Stream management for primitive types
 */

using Microsoft.Xna.Framework;
using System.IO;

namespace EditorImGui
{
    internal class HelpSerialize
    {
        public static void Vec3(BinaryWriter _stream, Vector3 _vector)
        {
            _stream.Write(_vector.X);
            _stream.Write(_vector.Y);
            _stream.Write(_vector.Z);
        }
    }

    internal class HelpDeserialize
    {
        public static Vector3 Vec3(BinaryReader _stream)
        {
            Vector3 v = Vector3.Zero;
            v.X = _stream.ReadSingle();
            v.Y = _stream.ReadSingle();
            v.Z = _stream.ReadSingle();
            return v;
        }
    }
}
