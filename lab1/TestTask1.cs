using Microsoft.Xna.Framework;
using System;

class TestTask1
{
    static void Main()
    {
        Console.WriteLine("=== Vectors Task 1 - Movement Calculator ===");
        Console.WriteLine();
        
        // Test data
        Vector3 source = new Vector3(0, 0, 0);
        Vector3 destination = new Vector3(10, 10, 10);
        float speed = 2.0f;
        
        Console.WriteLine($"Source position: {source}");
        Console.WriteLine($"Destination position: {destination}");
        Console.WriteLine($"Speed: {speed} units per step");
        Console.WriteLine();
        
        // Calculate distance
        float distance = Vector3.Distance(source, destination);
        Console.WriteLine($"Distance: {distance:F2} units");
        
        // Calculate steps
        int steps = (int)Math.Ceiling(distance / speed);
        Console.WriteLine($"Steps needed: {steps}");
        
        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}
