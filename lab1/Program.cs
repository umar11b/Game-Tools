using Microsoft.Xna.Framework;
using System;

namespace lab1;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Vector Addition Test");
        Console.WriteLine("===================");
        
        AddVectors();
        
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
}