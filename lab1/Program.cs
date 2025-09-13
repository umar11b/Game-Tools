using Microsoft.Xna.Framework;
using System;

namespace lab1;

class Program
{
    static Vector3 pos = new Vector3(10, 10, 10);
    
    static void Main(string[] args)
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
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
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
}