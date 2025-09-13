using Microsoft.Xna.Framework;
using System;

namespace lab1.vectors;

public class Task2
{
    public static void Run()
    {
        Console.WriteLine("=== Task 2: Triangle Calculator ===");
        Console.WriteLine("Calculate the perimeter and area of a triangle formed by 3 objects");
        Console.WriteLine();
        
        // Ask for 3 object positions
        Console.WriteLine("Enter the position of 3 objects in the game world:");
        Vector3 object1 = GetVector3FromUser("Object 1");
        Vector3 object2 = GetVector3FromUser("Object 2");
        Vector3 object3 = GetVector3FromUser("Object 3");
        
        // Calculate triangle properties
        CalculateTriangle(object1, object2, object3);
        
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }
    
    private static Vector3 GetVector3FromUser(string objectName)
    {
        Console.WriteLine($"\n{objectName} position:");
        Console.Write($"Enter {objectName} X coordinate: ");
        string? input = Console.ReadLine();
        float x = float.TryParse(input, out float xVal) ? xVal : 0;
        
        Console.Write($"Enter {objectName} Y coordinate: ");
        input = Console.ReadLine();
        float y = float.TryParse(input, out float yVal) ? yVal : 0;
        
        Console.Write($"Enter {objectName} Z coordinate: ");
        input = Console.ReadLine();
        float z = float.TryParse(input, out float zVal) ? zVal : 0;
        
        return new Vector3(x, y, z);
    }
    
    private static void CalculateTriangle(Vector3 obj1, Vector3 obj2, Vector3 obj3)
    {
        Console.WriteLine("\n=== Triangle Calculation ===");
        Console.WriteLine($"Object 1 position: {obj1}");
        Console.WriteLine($"Object 2 position: {obj2}");
        Console.WriteLine($"Object 3 position: {obj3}");
        
        // Calculate the three sides of the triangle
        float side1 = Vector3.Distance(obj1, obj2);
        float side2 = Vector3.Distance(obj2, obj3);
        float side3 = Vector3.Distance(obj3, obj1);
        
        Console.WriteLine($"\nTriangle sides:");
        Console.WriteLine($"Side 1 (Object 1 to Object 2): {side1:F2}");
        Console.WriteLine($"Side 2 (Object 2 to Object 3): {side2:F2}");
        Console.WriteLine($"Side 3 (Object 3 to Object 1): {side3:F2}");
        
        // Calculate perimeter
        float perimeter = side1 + side2 + side3;
        Console.WriteLine($"\nPerimeter: {perimeter:F2}");
        
        // Calculate area using Heron's formula
        float s = perimeter / 2; // Semi-perimeter
        float area = (float)Math.Sqrt(s * (s - side1) * (s - side2) * (s - side3));
        Console.WriteLine($"Area: {area:F2}");
        
        // Check if it's a valid triangle
        if (side1 + side2 > side3 && side2 + side3 > side1 && side3 + side1 > side2)
        {
            Console.WriteLine("\n✓ This is a valid triangle!");
        }
        else
        {
            Console.WriteLine("\n⚠ Warning: These points do not form a valid triangle!");
        }
    }
}
