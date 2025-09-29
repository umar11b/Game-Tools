/*
 * Lab 3 - Program.cs
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered ImGui.NET editor with embedded MonoGame teapot
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - ImGui.NET integration with MonoGame
 * - Cross-platform editor application structure
 * - Embedded 3D rendering in editor UI
 */

using System;

namespace EditorImGui
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                using (var game = new SimpleEditorGame())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Editor: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
