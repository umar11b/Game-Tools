using System;
using lab1.matrices;

namespace TestCustomMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Custom Matrix Test ===");
            
            // Test 2x2 matrices
            CustomMatrix matrix1 = new CustomMatrix(2, 2);
            CustomMatrix matrix2 = new CustomMatrix(2, 2);
            
            // Fill matrix1
            matrix1[0, 0] = 1; matrix1[0, 1] = 2;
            matrix1[1, 0] = 3; matrix1[1, 1] = 4;
            
            // Fill matrix2
            matrix2[0, 0] = 5; matrix2[0, 1] = 6;
            matrix2[1, 0] = 7; matrix2[1, 1] = 8;
            
            Console.WriteLine("Matrix 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine("\nMatrix 2:");
            Console.WriteLine(matrix2);
            
            Console.WriteLine("\nAddition:");
            Console.WriteLine(matrix1 + matrix2);
            
            Console.WriteLine("\nSubtraction:");
            Console.WriteLine(matrix1 - matrix2);
            
            Console.WriteLine("\nMultiplication:");
            Console.WriteLine(matrix1 * matrix2);
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
