using Microsoft.Xna.Framework;
using System;

namespace lab1.matrices;

public class MatrixDemo
{
    public static void Run()
    {
        Console.WriteLine("=== Matrix Operations Demo ===");
        Console.WriteLine("Basic matrix operations and calculations");
        Console.WriteLine();
        
        // Matrix addition, subtraction, and multiplication
        MatrixAddition();
        MatrixSubtraction();
        MatrixMultiplication();
        MatrixIdentity();
        MatrixTranslation();
        MatrixScaling();
        MatrixRotation();
        
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }
    
    private static void MatrixAddition()
    {
        Console.WriteLine("--- Matrix Addition ---");
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
    }
    
    private static void MatrixSubtraction()
    {
        Console.WriteLine("\n--- Matrix Subtraction ---");
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
        Console.WriteLine($"Subtracted = {matrix1 - matrix2}");
    }
    
    private static void MatrixMultiplication()
    {
        Console.WriteLine("\n--- Matrix Multiplication ---");
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
        Console.WriteLine($"1 * 2 = {matrix1 * matrix2}");
        Console.WriteLine($"2 * 1 = {matrix2 * matrix1}");
    }
    
    private static void MatrixIdentity()
    {
        Console.WriteLine("\n--- Matrix Identity ---");
        Matrix matrix1 = Matrix.Identity;
        Matrix matrix2 = new Matrix(
            new Vector4(5, 5, 5, 5), 
            new Vector4(6, 6, 6, 6),
            new Vector4(7, 7, 7, 7), 
            new Vector4(8, 8, 8, 8)
        );
        
        Console.WriteLine($"Matrix1 = {matrix1}");
        Console.WriteLine($"Matrix2 = {matrix2}");
        Console.WriteLine($"1 * 2 = {matrix1 * matrix2}");
        Console.WriteLine($"2 * 1 = {matrix2 * matrix1}");
    }
    
    private static void MatrixTranslation()
    {
        Console.WriteLine("\n--- Matrix Translation ---");
        Vector3 pos1 = new Vector3(0, 0, 0);
        Vector3 pos2 = new Vector3(1, 2, 3);
        Matrix objectPos1 = Matrix.CreateTranslation(pos1);
        Matrix objectPos2 = Matrix.CreateTranslation(pos2);
        
        Console.WriteLine($"ObjectPos1 = {objectPos1}");
        Console.WriteLine($"ObjectPos2 = {objectPos2}");
    }
    
    private static void MatrixScaling()
    {
        Console.WriteLine("\n--- Matrix Scaling ---");
        Vector3 scale = new Vector3(2, 2, 2);
        Matrix objectScale = Matrix.CreateScale(scale);
        
        Console.WriteLine($"ObjectScale = {objectScale}");
    }
    
    private static void MatrixRotation()
    {
        Console.WriteLine("\n--- Matrix Rotation ---");
        Vector3 rotate = new Vector3(0, 0.1f, 0);
        Matrix objectRotate = Matrix.CreateRotationY(rotate.Y);
        
        Console.WriteLine($"ObjectRotate = {objectRotate}");
    }
}
