using Microsoft.Xna.Framework;
using System;

namespace lab1;

class Demo
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Sheridan Game Tools Lab Demo ===");
        Console.WriteLine("All tasks implemented and ready for 1-minute demo!");
        Console.WriteLine();
        
        Console.WriteLine("VECTORS TASKS:");
        Console.WriteLine("1. Movement Calculator - Calculate steps between two points");
        Console.WriteLine("2. Triangle Calculator - Calculate perimeter and area");
        Console.WriteLine("3. Cuboid Constructor - Build 3D cuboid from triangle points");
        Console.WriteLine();
        
        Console.WriteLine("MATRICES TASKS:");
        Console.WriteLine("4. Custom Matrix Operations - Addition, subtraction, multiplication");
        Console.WriteLine("5. Spinning Teapot - 3D transformations with rotation and scaling");
        Console.WriteLine();
        
        Console.WriteLine("To run individual tasks:");
        Console.WriteLine("- Vectors: cd vectors && dotnet run --project ../VectorMath.csproj");
        Console.WriteLine("- Matrices Task 1: cd matrices && dotnet run --project ../VectorMath.csproj");
        Console.WriteLine("- Matrices Task 2: cd TeapotStandalone && dotnet run");
        Console.WriteLine();
        
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}
