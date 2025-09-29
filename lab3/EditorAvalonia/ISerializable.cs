/*
 * Lab 3 - ISerializable.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered serialization interface for save/load functionality
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Binary serialization interface design
 * - MonoGame content manager integration
 * - Save/load system architecture
 */

using Microsoft.Xna.Framework.Content;
using System.IO;

namespace EditorAvalonia
{
    internal interface ISerializable
    {
        public void Serialize(BinaryWriter _stream);
        public void Deserialize(BinaryReader _stream, ContentManager _content);
    }
}
