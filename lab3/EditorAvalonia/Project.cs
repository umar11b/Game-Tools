/*
 * Lab 3 - Project.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered project management and content loading
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Project structure and level management
 * - Content loading and asset management
 * - Scene rendering coordination and serialization
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace EditorAvalonia
{
    internal class Project : ISerializable
    {
        // Accessors (following slide example)
        public Level CurrentLevel { get; private set; } = null!;
        public List<Level> Levels { get; private set; } = new();
        public string Folder { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;

        // Constructor (following slide example)
        public Project()
        {
        }

        public Project(ContentManager _content, string _name)
        {
            Folder = Path.GetDirectoryName(_name) ?? string.Empty;
            Name = Path.GetFileName(_name);
            
            if (!Name.ToLower().EndsWith(".oce"))
            {
                Name += ".oce";
            }
            
            // Add a default level
            AddLevel(_content);
        }

        public void AddLevel(ContentManager _content)
        {
            CurrentLevel = new();
            CurrentLevel.LoadContent(_content);
            Levels.Add(CurrentLevel);
        }

        public void Render()
        {
            CurrentLevel.Render();
        }

        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(Levels.Count);
            int clIndex = Levels.IndexOf(CurrentLevel);
            foreach (var level in Levels)
            {
                level.Serialize(_stream);
            }
            _stream.Write(clIndex);
            _stream.Write(Folder);
            _stream.Write(Name);
        }

        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            int levelCount = _stream.ReadInt32();
            for (int count = 0; count < levelCount; count++)
            {
                Level l = new();
                l.Deserialize(_stream, _content);
                Levels.Add(l);
            }
            int clIndex = _stream.ReadInt32();
            CurrentLevel = Levels[clIndex];
            Folder = _stream.ReadString();
            Name = _stream.ReadString();
        }
    }
}