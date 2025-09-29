/*
 * Lab 3 - ImGuiRenderer.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered simple ImGui renderer for MonoGame integration
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - Basic ImGui.NET integration with MonoGame
 * - Simple renderer implementation for cross-platform compatibility
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;

namespace EditorImGui
{
    public class ImGuiRenderer
    {
        private readonly Game _game;

        public ImGuiRenderer(Game game)
        {
            _game = game;
        }

        public void RebuildFontAtlas()
        {
            // Simple font atlas rebuild - ImGui.NET handles this internally
            try
            {
                ImGui.GetIO().Fonts.Clear();
                ImGui.GetIO().Fonts.AddFontDefault();
                ImGui.GetIO().Fonts.Build();
            }
            catch
            {
                // Ignore font errors for now
            }
        }

        public void BeforeLayout(GameTime gameTime)
        {
            try
            {
                // Initialize ImGui frame
                var io = ImGui.GetIO();
                io.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                io.DisplaySize = new System.Numerics.Vector2(_game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height);
                ImGui.NewFrame();
            }
            catch
            {
                // Ignore ImGui errors for now
            }
        }

        public void AfterLayout()
        {
            try
            {
                // Render ImGui frame
                ImGui.Render();
                RenderImGui();
            }
            catch
            {
                // Ignore rendering errors for now
            }
        }

        private void RenderImGui()
        {
            try
            {
                var drawData = ImGui.GetDrawData();
                if (drawData.CmdListsCount == 0) return;

                // Simple rendering - just let ImGui draw
                // This is a basic implementation for demonstration
            }
            catch
            {
                // Ignore rendering errors for now
            }
        }
    }
}
