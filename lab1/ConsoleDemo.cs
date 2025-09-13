using Microsoft.Xna.Framework;
using System;

namespace lab1;

class ConsoleDemo
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Sheridan Game Tools Lab - Console Demo ===");
        Console.WriteLine("All tasks with console output");
        Console.WriteLine();
        
        while (true)
        {
            Console.WriteLine("Choose what to run:");
            Console.WriteLine("1. Vectors - Task 1 (Movement Calculator)");
            Console.WriteLine("2. Vectors - Task 2 (Triangle Calculator)");
            Console.WriteLine("3. Vectors - Task 3 (Cuboid Constructor)");
            Console.WriteLine("4. Matrices - Task 1 (Custom Matrix Operations)");
            Console.WriteLine("5. Matrices - Task 2 (Spinning Teapot)");
            Console.WriteLine("6. Vector Math Demo");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice (0-6): ");
            
            string choice = Console.ReadLine();
            Console.WriteLine();
            
            if (choice == "1")
            {
                vectors.Task1.Run();
            }
            else if (choice == "2")
            {
                vectors.Task2.Run();
            }
            else if (choice == "3")
            {
                vectors.Task3.Run();
            }
            else if (choice == "4")
            {
                matrices.Task1.Run();
            }
            else if (choice == "5")
            {
                Console.WriteLine("Starting 3D teapot...");
                Console.WriteLine("Note: For full 3D teapot demo, run: cd TeapotStandalone && dotnet run");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
            else if (choice == "6")
            {
                RunVectorMathDemo();
            }
            else if (choice == "0")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
            
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            Console.Clear();
        }
    }
    
    static void RunVectorMathDemo()
    {
        Console.WriteLine("Vector Math Test");
        Console.WriteLine("================");
        
        Console.WriteLine("\n--- Vector Addition ---");
        AddVectors();
        
        Console.WriteLine("\n--- Vector Subtraction ---");
        SubtractVectors();
        
        Console.WriteLine("\n--- Vector Movement ---");
        MultiplyVector();
        
        Console.WriteLine("\n--- Distance Calculation ---");
        CalculateDistance();
        
        Console.WriteLine("\n--- Dot Product ---");
        DotProduct();
        
        Console.WriteLine("\n--- Cross Product ---");
        CrossProduct();
    }
    
    static void AddVectors()
    {
        Vector2 a = new Vector2(3, 5);
        Vector2 b = new Vector2(2, -1);
        Vector2 c = a + b;
        Console.WriteLine($"Vector a {a} + b {b} = {c}");
    }
    
    static void SubtractVectors()
    {
        Vector3 a = new Vector3(1, 2, 3);
        Vector3 b = new Vector3(4, 5, 6);
        Vector3 c = a - b;
        Console.WriteLine($"Vector a {a} - b {b} = {c}");
    }
    
    static void MultiplyVector()
    {
        Vector3 pos = new Vector3(10, 10, 10);
        Vector3 dir = new Vector3(1, 0, 0);
        float speed = 5.0f;
        pos += dir * speed;
        Console.WriteLine($"Position after movement: {pos}");
    }
    
    static void CalculateDistance()
    {
        Vector3 point1 = new Vector3(0, 0, 0);
        Vector3 point2 = new Vector3(3, 4, 0);
        float distance = Vector3.Distance(point1, point2);
        Console.WriteLine($"Distance between {point1} and {point2} = {distance:F2}");
    }
    
    static void DotProduct()
    {
        Vector3 a = new Vector3(1, 2, 3);
        Vector3 b = new Vector3(4, 5, 6);
        float dot = Vector3.Dot(a, b);
        Console.WriteLine($"Dot product of {a} and {b} = {dot}");
    }
    
    static void CrossProduct()
    {
        Vector3 a = new Vector3(1, 0, 0);
        Vector3 b = new Vector3(0, 1, 0);
        Vector3 cross = Vector3.Cross(a, b);
        Console.WriteLine($"Cross product of {a} and {b} = {cross}");
    }
}
