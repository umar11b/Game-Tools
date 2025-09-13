using Microsoft.Xna.Framework;
using System;

namespace lab1.vectors;

public class Task3
{
    public static void Run()
    {
        Console.WriteLine("=== Task 3: Cuboid Constructor ===");
        Console.WriteLine("Create a 3D cuboid from 3 triangle points");
        Console.WriteLine();
        
        // Ask for 3 points of a triangle
        Console.WriteLine("Enter 3 points that form a triangle (half of one side of a cuboid):");
        Vector3 point1 = GetVector3FromUser("Point 1");
        Vector3 point2 = GetVector3FromUser("Point 2");
        Vector3 point3 = GetVector3FromUser("Point 3");
        
        // Set a fixed depth for the cuboid
        float depth = 5.0f;
        Console.WriteLine($"\nUsing fixed depth: {depth}");
        
        // Construct the cuboid
        ConstructCuboid(point1, point2, point3, depth);
        
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }
    
    private static Vector3 GetVector3FromUser(string pointName)
    {
        Console.WriteLine($"\n{pointName}:");
        Console.Write($"Enter {pointName} X coordinate: ");
        string? input = Console.ReadLine();
        float x = float.TryParse(input, out float xVal) ? xVal : 0;
        
        Console.Write($"Enter {pointName} Y coordinate: ");
        input = Console.ReadLine();
        float y = float.TryParse(input, out float yVal) ? yVal : 0;
        
        Console.Write($"Enter {pointName} Z coordinate: ");
        input = Console.ReadLine();
        float z = float.TryParse(input, out float zVal) ? zVal : 0;
        
        return new Vector3(x, y, z);
    }
    
    private static void ConstructCuboid(Vector3 p1, Vector3 p2, Vector3 p3, float depth)
    {
        Console.WriteLine("\n=== Cuboid Construction ===");
        Console.WriteLine($"Triangle points:");
        Console.WriteLine($"Point 1: {p1}");
        Console.WriteLine($"Point 2: {p2}");
        Console.WriteLine($"Point 3: {p3}");
        Console.WriteLine($"Cuboid depth: {depth}");
        
        // Calculate the normal vector of the triangle (using cross product)
        Vector3 edge1 = p2 - p1;
        Vector3 edge2 = p3 - p1;
        Vector3 normal = Vector3.Cross(edge1, edge2);
        normal.Normalize();
        
        Console.WriteLine($"\nTriangle normal: {normal}");
        
        // Create the opposite face by moving the triangle points along the normal
        Vector3 p4 = p1 + (normal * depth);
        Vector3 p5 = p2 + (normal * depth);
        Vector3 p6 = p3 + (normal * depth);
        
        Console.WriteLine($"\nCuboid vertices:");
        Console.WriteLine($"Front face (original triangle):");
        Console.WriteLine($"  A: {p1}");
        Console.WriteLine($"  B: {p2}");
        Console.WriteLine($"  C: {p3}");
        
        Console.WriteLine($"\nBack face (opposite triangle):");
        Console.WriteLine($"  D: {p4}");
        Console.WriteLine($"  E: {p5}");
        Console.WriteLine($"  F: {p6}");
        
        // Calculate some basic properties
        float frontArea = CalculateTriangleArea(p1, p2, p3);
        float volume = frontArea * depth;
        
        Console.WriteLine($"\nCuboid properties:");
        Console.WriteLine($"Front face area: {frontArea:F2}");
        Console.WriteLine($"Volume: {volume:F2}");
        
        // Show the edges
        Console.WriteLine($"\nCuboid edges:");
        Console.WriteLine($"Front edges: AB, BC, CA");
        Console.WriteLine($"Back edges: DE, EF, FD");
        Console.WriteLine($"Connecting edges: AD, BE, CF");
    }
    
    private static float CalculateTriangleArea(Vector3 a, Vector3 b, Vector3 c)
    {
        // Use cross product to calculate triangle area
        Vector3 edge1 = b - a;
        Vector3 edge2 = c - a;
        Vector3 cross = Vector3.Cross(edge1, edge2);
        return cross.Length() / 2.0f;
    }
}
