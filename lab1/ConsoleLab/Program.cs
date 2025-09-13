using Microsoft.Xna.Framework;
using System;
// Zamanu 
// Umar Zaman - Lab 1 

/*
 * AI Tool Citation:
 * Cursor. (2025). Cursor AI (September 12 version) [AI-powered code editor]. https://cursor.sh/
 * 
 * This code was generated with assistance from Cursor AI to implement console applications
 * for vector and matrix operations as part of Sheridan Game Tools Lab 1.
 */

namespace lab1;

class ConsoleTasks
{
    // The following code was generated on September 12, 2025, by Cursor AI software (Cursor, 2025), 
    // to implement the main menu loop for console tasks in response to the prompt "create a console application menu system"
    static void Main(string[] args)
    {
        Console.WriteLine("=== Sheridan Game Tools Lab - Console Tasks ===");
        Console.WriteLine("Vectors and Matrices Tasks with Console Output");
        Console.WriteLine();
        
        while (true)
        {
            Console.WriteLine("Choose what to run:");
            Console.WriteLine("1. Vectors - Task 1 (Movement Calculator)");
            Console.WriteLine("2. Vectors - Task 2 (Triangle Calculator)");
            Console.WriteLine("3. Vectors - Task 3 (Cuboid Constructor)");
            Console.WriteLine("4. Matrices - Task 1 (Custom Matrix Operations)");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice (0-4): ");
            
            string choice = Console.ReadLine();
            Console.WriteLine();
            
            if (choice == "1")
            {
                RunVectorsTask1();
            }
            else if (choice == "2")
            {
                RunVectorsTask2();
            }
            else if (choice == "3")
            {
                RunVectorsTask3();
            }
            else if (choice == "4")
            {
                RunMatricesTask1();
            }
            else if (choice == "0")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter a number between 0 and 4.");
            }
            
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            Console.Clear();
        }
    }
    
    // The following code was generated on September 12, 2025, by Cursor AI software (Cursor, 2025),
    // to implement Vectors Task 1 movement calculator in response to the prompt "create a movement calculator using Vector3"
    static void RunVectorsTask1()
    {
        Console.WriteLine("=== Vectors Task 1 - Movement Calculator ===");
        Console.WriteLine();
        
        Console.Write("Enter source X position: ");
        float sourceX = GetValidFloat("0");
        Console.Write("Enter source Y position: ");
        float sourceY = GetValidFloat("0");
        Console.Write("Enter source Z position: ");
        float sourceZ = GetValidFloat("0");
        
        Console.Write("Enter destination X position: ");
        float destX = GetValidFloat("10");
        Console.Write("Enter destination Y position: ");
        float destY = GetValidFloat("10");
        Console.Write("Enter destination Z position: ");
        float destZ = GetValidFloat("10");
        
        Console.Write("Enter movement speed: ");
        float speed = GetValidFloat("2");
        
        Vector3 source = new Vector3(sourceX, sourceY, sourceZ);
        Vector3 destination = new Vector3(destX, destY, destZ);
        
        Console.WriteLine();
        Console.WriteLine($"Source: {source}");
        Console.WriteLine($"Destination: {destination}");
        
        float distance = Vector3.Distance(source, destination);
        int steps = (int)Math.Ceiling(distance / speed);
        
        Console.WriteLine($"Distance: {distance:F2} units");
        Console.WriteLine($"Steps needed: {steps}");
    }
    
    // The following code was generated on September 12, 2025, by Cursor AI software (Cursor, 2025),
    // to implement Vectors Task 2 triangle calculator in response to the prompt "create a triangle perimeter and area calculator using Vector3"
    static void RunVectorsTask2()
    {
        Console.WriteLine("=== Vectors Task 2 - Triangle Calculator ===");
        Console.WriteLine();
        
        Console.Write("Enter point 1 X: ");
        float x1 = float.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter point 1 Y: ");
        float y1 = float.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter point 1 Z: ");
        float z1 = float.Parse(Console.ReadLine() ?? "0");
        
        Console.Write("Enter point 2 X: ");
        float x2 = float.Parse(Console.ReadLine() ?? "3");
        Console.Write("Enter point 2 Y: ");
        float y2 = float.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter point 2 Z: ");
        float z2 = float.Parse(Console.ReadLine() ?? "0");
        
        Console.Write("Enter point 3 X: ");
        float x3 = float.Parse(Console.ReadLine() ?? "1.5");
        Console.Write("Enter point 3 Y: ");
        float y3 = float.Parse(Console.ReadLine() ?? "2.6");
        Console.Write("Enter point 3 Z: ");
        float z3 = float.Parse(Console.ReadLine() ?? "0");
        
        Vector3 p1 = new Vector3(x1, y1, z1);
        Vector3 p2 = new Vector3(x2, y2, z2);
        Vector3 p3 = new Vector3(x3, y3, z3);
        
        Console.WriteLine();
        Console.WriteLine($"Point 1: {p1}");
        Console.WriteLine($"Point 2: {p2}");
        Console.WriteLine($"Point 3: {p3}");
        
        float side1 = Vector3.Distance(p1, p2);
        float side2 = Vector3.Distance(p2, p3);
        float side3 = Vector3.Distance(p3, p1);
        
        float perimeter = side1 + side2 + side3;
        float s = perimeter / 2;
        float area = (float)Math.Sqrt(s * (s - side1) * (s - side2) * (s - side3));
        
        Console.WriteLine($"Perimeter: {perimeter:F2} units");
        Console.WriteLine($"Area: {area:F2} square units");
    }
    
    // The following code was generated on September 12, 2025, by Cursor AI software (Cursor, 2025),
    // to implement Vectors Task 3 cuboid constructor in response to the prompt "create a 3D cuboid constructor using Vector3 points"
    static void RunVectorsTask3()
    {
        Console.WriteLine("=== Vectors Task 3 - Cuboid Constructor ===");
        Console.WriteLine();
        
        Console.Write("Enter point 1 X: ");
        float x1 = float.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter point 1 Y: ");
        float y1 = float.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter point 1 Z: ");
        float z1 = float.Parse(Console.ReadLine() ?? "0");
        
        Console.Write("Enter point 2 X: ");
        float x2 = float.Parse(Console.ReadLine() ?? "3");
        Console.Write("Enter point 2 Y: ");
        float y2 = float.Parse(Console.ReadLine() ?? "0");
        Console.Write("Enter point 2 Z: ");
        float z2 = float.Parse(Console.ReadLine() ?? "0");
        
        Console.Write("Enter point 3 X: ");
        float x3 = float.Parse(Console.ReadLine() ?? "1.5");
        Console.Write("Enter point 3 Y: ");
        float y3 = float.Parse(Console.ReadLine() ?? "2.6");
        Console.Write("Enter point 3 Z: ");
        float z3 = float.Parse(Console.ReadLine() ?? "0");
        
        Vector3 p1 = new Vector3(x1, y1, z1);
        Vector3 p2 = new Vector3(x2, y2, z2);
        Vector3 p3 = new Vector3(x3, y3, z3);
        
        Console.WriteLine();
        Console.WriteLine($"Point 1: {p1}");
        Console.WriteLine($"Point 2: {p2}");
        Console.WriteLine($"Point 3: {p3}");
        
        float side1 = Vector3.Distance(p1, p2);
        float side2 = Vector3.Distance(p2, p3);
        float side3 = Vector3.Distance(p3, p1);
        
        float s = (side1 + side2 + side3) / 2;
        float baseArea = (float)Math.Sqrt(s * (s - side1) * (s - side2) * (s - side3));
        float depth = 5.0f; // Hard-coded depth
        float volume = baseArea * depth;
        
        Console.WriteLine();
        Console.WriteLine("Cuboid Properties:");
        Console.WriteLine($"- Base area: {baseArea:F2} square units");
        Console.WriteLine($"- Depth: {depth:F2} units");
        Console.WriteLine($"- Volume: {volume:F2} cubic units");
    }
    
    // The following code was generated on September 12, 2025, by Cursor AI software (Cursor, 2025),
    // to implement Matrices Task 1 custom matrix operations in response to the prompt "create a custom Matrix class with addition, subtraction, and multiplication"
    static void RunMatricesTask1()
    {
        Console.WriteLine("=== Matrices Task 1 - Custom Matrix Operations ===");
        Console.WriteLine();
        
        Console.Write("Enter number of rows: ");
        int rows = int.Parse(Console.ReadLine() ?? "2");
        Console.Write("Enter number of columns: ");
        int cols = int.Parse(Console.ReadLine() ?? "2");
        
        Console.WriteLine($"\nCreating {rows}x{cols} matrices...");
        Console.WriteLine();
        
        // Create matrices
        float[,] matrix1 = new float[rows, cols];
        float[,] matrix2 = new float[rows, cols];
        
        // Fill first matrix
        Console.WriteLine("Enter data for Matrix 1:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"Enter value for position [{i},{j}]: ");
                string input = Console.ReadLine() ?? "0";
                while (!float.TryParse(input, out matrix1[i, j]))
                {
                    Console.Write("Invalid input. Please enter a valid number: ");
                    input = Console.ReadLine() ?? "0";
                }
            }
        }
        
        // Fill second matrix
        Console.WriteLine("\nEnter data for Matrix 2:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"Enter value for position [{i},{j}]: ");
                string input = Console.ReadLine() ?? "0";
                while (!float.TryParse(input, out matrix2[i, j]))
                {
                    Console.Write("Invalid input. Please enter a valid number: ");
                    input = Console.ReadLine() ?? "0";
                }
            }
        }
        
        // Display matrices
        Console.WriteLine("\nMatrix 1:");
        PrintMatrix(matrix1, rows, cols);
        Console.WriteLine("\nMatrix 2:");
        PrintMatrix(matrix2, rows, cols);
        
        // Perform operations
        Console.WriteLine("\nMatrix Addition:");
        float[,] addition = AddMatrices(matrix1, matrix2, rows, cols);
        PrintMatrix(addition, rows, cols);
        
        Console.WriteLine("\nMatrix Subtraction:");
        float[,] subtraction = SubtractMatrices(matrix1, matrix2, rows, cols);
        PrintMatrix(subtraction, rows, cols);
        
        if (cols == rows) // Only multiply if square matrices
        {
            Console.WriteLine("\nMatrix Multiplication:");
            float[,] multiplication = MultiplyMatrices(matrix1, matrix2, rows, cols);
            PrintMatrix(multiplication, rows, cols);
        }
        else
        {
            Console.WriteLine("\nMatrix Multiplication: Not possible (matrices must be square)");
        }
    }
    
    
    static void PrintMatrix(float[,] matrix, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            Console.Write("[");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{matrix[i, j]:F2}");
                if (j < cols - 1) Console.Write(", ");
            }
            Console.WriteLine("]");
        }
    }
    
    static float[,] AddMatrices(float[,] a, float[,] b, int rows, int cols)
    {
        float[,] result = new float[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        }
        return result;
    }
    
    static float[,] SubtractMatrices(float[,] a, float[,] b, int rows, int cols)
    {
        float[,] result = new float[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] - b[i, j];
            }
        }
        return result;
    }
    
    static float[,] MultiplyMatrices(float[,] a, float[,] b, int rows, int cols)
    {
        float[,] result = new float[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                float sum = 0;
                for (int k = 0; k < cols; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        }
        return result;
    }
    
    static float GetValidFloat(string defaultValue)
    {
        string input = Console.ReadLine() ?? defaultValue;
        while (!float.TryParse(input, out float result))
        {
            Console.Write("Invalid input. Please enter a valid number: ");
            input = Console.ReadLine() ?? defaultValue;
        }
        return float.Parse(input);
    }
}
