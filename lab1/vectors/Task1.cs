using Microsoft.Xna.Framework;
using System;

namespace lab1.vectors;

public class Task1
{
    public static void Run()
    {
        Console.WriteLine("=== Task 1: Movement Calculator ===");
        Console.WriteLine("Calculate how many steps a person needs to reach their destination");
        Console.WriteLine();
        
        // Ask for source position
        Console.WriteLine("Enter the person's starting position:");
        Vector3 source = GetVector3FromUser("Source");
        
        // Ask for destination position
        Console.WriteLine("\nEnter the person's destination position:");
        Vector3 destination = GetVector3FromUser("Destination");
        
        // Ask for movement speed
        Console.WriteLine("\nEnter how fast the person can move per step:");
        Console.Write("Speed per step: ");
        float speed = float.Parse(Console.ReadLine() ?? "1");
        
        // Calculate the movement
        CalculateMovement(source, destination, speed);
        
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }
    
    private static Vector3 GetVector3FromUser(string positionName)
    {
        Console.Write($"Enter {positionName} X coordinate: ");
        string? input = Console.ReadLine();
        float x = float.TryParse(input, out float xVal) ? xVal : 0;
        
        Console.Write($"Enter {positionName} Y coordinate: ");
        input = Console.ReadLine();
        float y = float.TryParse(input, out float yVal) ? yVal : 0;
        
        Console.Write($"Enter {positionName} Z coordinate: ");
        input = Console.ReadLine();
        float z = float.TryParse(input, out float zVal) ? zVal : 0;
        
        return new Vector3(x, y, z);
    }
    
    private static void CalculateMovement(Vector3 source, Vector3 destination, float speed)
    {
        Console.WriteLine("\n=== Movement Calculation ===");
        Console.WriteLine($"Starting position: {source}");
        Console.WriteLine($"Destination: {destination}");
        Console.WriteLine($"Speed per step: {speed}");
        
        // Calculate the direction vector
        Vector3 direction = destination - source;
        float totalDistance = direction.Length();
        
        Console.WriteLine($"\nDirection vector: {direction}");
        Console.WriteLine($"Total distance to travel: {totalDistance:F2}");
        
        // Calculate number of steps needed
        int stepsNeeded = (int)Math.Ceiling(totalDistance / speed);
        
        Console.WriteLine($"\nNumber of steps needed: {stepsNeeded}");
        
        // Show the normalized direction
        Vector3 normalizedDirection = direction;
        normalizedDirection.Normalize();
        Console.WriteLine($"Direction per step: {normalizedDirection}");
    }
}
