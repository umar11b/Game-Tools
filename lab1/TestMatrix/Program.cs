using Microsoft.Xna.Framework;
using System;

namespace TestMatrix;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Matrix Test ===");
        
        // Matrix addition
        Matrix matrix1 = new Matrix(
            new Vector4(1, 1, 1, 1), 
            new Vector4(2, 2, 2, 2),
            new Vector4(3, 3, 3, 3), 
            new Vector4(4, 4, 4, 4)
        );
        
        Matrix matrix2 = new Matrix(
            new Vector4(5, 5, 5, 5), 
            new Vector4(6, 6, 6, 6),
            new Vector4(7, 7, 7, 7), 
            new Vector4(8, 8, 8, 8)
        );
        
        Console.WriteLine($"Matrix1 = {matrix1}");
        Console.WriteLine($"Matrix2 = {matrix2}");
        Console.WriteLine($"Added = {matrix1 + matrix2}");
        Console.WriteLine($"Subtracted = {matrix1 - matrix2}");
        Console.WriteLine($"1 * 2 = {matrix1 * matrix2}");
        Console.WriteLine($"2 * 1 = {matrix2 * matrix1}");
        
        Console.WriteLine("\n--- Matrix Identity ---");
        Matrix identityMatrix = Matrix.Identity;
        Console.WriteLine($"Identity = {identityMatrix}");
        Console.WriteLine($"Identity * Matrix2 = {identityMatrix * matrix2}");
        Console.WriteLine($"Matrix2 * Identity = {matrix2 * identityMatrix}");
        
        Console.WriteLine("\n--- Matrix Translation ---");
        Vector3 pos1 = new Vector3(0, 0, 0);
        Vector3 pos2 = new Vector3(1, 2, 3);
        Matrix objectPos1 = Matrix.CreateTranslation(pos1);
        Matrix objectPos2 = Matrix.CreateTranslation(pos2);
        Console.WriteLine($"ObjectPos1 = {objectPos1}");
        Console.WriteLine($"ObjectPos2 = {objectPos2}");
        
        Console.WriteLine("\n--- Matrix Scaling ---");
        Vector3 scale = new Vector3(2, 2, 2);
        Matrix objectScale = Matrix.CreateScale(scale);
        Console.WriteLine($"ObjectScale = {objectScale}");
        
        Console.WriteLine("\n--- Matrix Rotation ---");
        Vector3 rotate = new Vector3(0, 0.1f, 0);
        Matrix objectRotate = Matrix.CreateRotationY(rotate.Y);
        Console.WriteLine($"ObjectRotate = {objectRotate}");
        
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }
}