using Microsoft.Xna.Framework;
using System;

namespace lab1;

class Program
{
    static Vector3 pos = new Vector3(10, 10, 10);
    
    static void Main(string[] args)
    {
        Console.WriteLine("=== Sheridan Game Tools Lab ===");
        Console.WriteLine("Choose what to run:");
        Console.WriteLine("1. Vector Math Demo");
        Console.WriteLine("2. Vectors - Task 1 (Movement Calculator)");
        Console.WriteLine("3. Vectors - Task 2 (Triangle Calculator)");
               Console.WriteLine("4. Vectors - Task 3 (Cuboid Constructor)");
               Console.WriteLine("5. Matrices - Demo");
               Console.WriteLine("6. Matrices - Task 1 (Custom Matrix Operations)");
               Console.WriteLine("7. Matrices - Task 2 (Teapot Transformations)");
        Console.WriteLine();
        Console.Write("Enter your choice (1-7): ");
        
        string choice = Console.ReadLine();
        
        if (choice == "1")
        {
            RunVectorMathDemo();
        }
        else if (choice == "2")
        {
            vectors.Task1.Run();
        }
        else if (choice == "3")
        {
            vectors.Task2.Run();
        }
        else if (choice == "4")
        {
            vectors.Task3.Run();
        }
               else if (choice == "5")
               {
                   matrices.MatrixDemo.Run();
               }
               else if (choice == "6")
               {
                   matrices.Task1.Run();
               }
               else if (choice == "7")
               {
                   Console.WriteLine("Starting 3D teapot...");
                   Console.WriteLine("Note: For full 3D teapot demo, run: cd TeapotStandalone && dotnet run");
                   Console.WriteLine("Press Enter to continue...");
                   Console.ReadLine();
               }
               else
               {
                   Console.WriteLine("Invalid choice. Running Vector Math Demo...");
                   RunVectorMathDemo();
               }
    }
    
    static void RunVectorMathDemo()
    {
        Console.WriteLine("\nVector Math Test");
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
        
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }
    
    private static void AddVectors()
    {
        Vector2 a = new Vector2(3, 5);
        Vector2 b = new Vector2(2, -1);
        Vector2 c = a + b;
        Console.WriteLine($"Vector a {a} + b {b} = {c}");
    }
    
    private static void SubtractVectors()
    {
        Vector2 a = new Vector2(3, 5);
        Vector2 b = new Vector2(2, -1);
        Vector2 c = b - a;
        Vector2 d = a - b;
        Console.WriteLine($"Vector c mag: {c.Length()} c dir: {c}");
        Console.WriteLine($"Vector d mag: {d.Length()} d dir: {d}");
    }
    
    private static void MultiplyVector()
    {
        Vector3 dest = new Vector3(0, 0, 0);
        Vector3 dir = dest - pos;
        dir.Normalize();
        pos += (dir * 0.1f);
        Console.WriteLine($"Pos = {pos}");
    }
    
    private static void CalculateDistance()
    {
        Vector3 p1 = new Vector3(1, 1, 0);
        Vector3 p2 = new Vector3(2, 1, 2);
        float distance = Vector3.Distance(p1, p2);
        Console.WriteLine($"Distance between {p1} and {p2}: {distance}");
    }
    
    private static void DotProduct()
    {
        Vector3 source = new Vector3(0, 0, 0);
        Vector3 a = new Vector3(0, 2, 0);
        Vector3 b = new Vector3(1, 1, 0);

        Vector3 aVec = a - source;
        Vector3 bVec = b - source;

        aVec.Normalize();
        bVec.Normalize();

        float dot = Vector3.Dot(aVec, bVec);

        Console.WriteLine($"Dot = {dot}");
    }
    
    private static void CrossProduct()
    {
        Vector3 source = new Vector3(0, 0, 0);
        Vector3 a = new Vector3(0, 2, 0);
        Vector3 b = new Vector3(1, 1, 0);

        Vector3 aVec = a - source;
        Vector3 bVec = b - source;

        Vector3 cross = Vector3.Cross(aVec, bVec);
        cross.Normalize();

        Console.WriteLine($"Cross = {cross}");
    }
}