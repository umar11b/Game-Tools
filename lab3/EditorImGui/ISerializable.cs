/*
 * Lab 3 - ISerializable.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered serialization interface for game objects
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Defining the ISerializable interface structure
 * - Specifying Serialize and Deserialize method signatures
 * - Ensuring compatibility with binary streams and content management
 */

using Microsoft.Xna.Framework.Content;
using System.IO;

namespace EditorImGui
{
    internal interface ISerializable
    {
        public void Serialize(BinaryWriter _stream);
        public void Deserialize(BinaryReader _stream, ContentManager _content);
    }
}
