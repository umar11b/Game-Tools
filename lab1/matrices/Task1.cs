using System;

namespace lab1.matrices
{
    public class Task1
    {
        public static void Run()
        {
            Console.WriteLine("=== Custom Matrix Operations ===");
            Console.WriteLine("This task uses a custom Matrix class (no MonoGame libraries)");
            Console.WriteLine();

            // Get matrix dimensions
            Console.Write("Enter number of rows (X dimension): ");
            int rows = GetValidInt();

            Console.Write("Enter number of columns (Y dimension): ");
            int cols = GetValidInt();

            Console.WriteLine($"\nCreating {rows}x{cols} matrices...");
            Console.WriteLine();

            // Create matrices
            CustomMatrix matrix1 = new CustomMatrix(rows, cols);
            CustomMatrix matrix2 = new CustomMatrix(rows, cols);

            // Fill first matrix
            Console.WriteLine("Enter data for Matrix 1:");
            FillMatrix(matrix1);

            // Fill second matrix
            Console.WriteLine("\nEnter data for Matrix 2:");
            FillMatrix(matrix2);

            // Display matrices
            Console.WriteLine("\nMatrix 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine("\nMatrix 2:");
            Console.WriteLine(matrix2);

            // Perform operations
            try
            {
                Console.WriteLine("Matrix Addition (Matrix1 + Matrix2):");
                CustomMatrix addition = matrix1 + matrix2;
                Console.WriteLine(addition);

                Console.WriteLine("\nMatrix Subtraction (Matrix1 - Matrix2):");
                CustomMatrix subtraction = matrix1 - matrix2;
                Console.WriteLine(subtraction);

                Console.WriteLine("\nMatrix Multiplication (Matrix1 * Matrix2):");
                CustomMatrix multiplication = matrix1 * matrix2;
                Console.WriteLine(multiplication);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        private static int GetValidInt()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int value) && value > 0)
                {
                    return value;
                }
                Console.Write("Please enter a valid positive integer: ");
            }
        }

        private static void FillMatrix(CustomMatrix matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    Console.Write($"Enter value for position [{i},{j}]: ");
                    matrix[i, j] = GetValidFloat();
                }
            }
        }

        private static float GetValidFloat()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (float.TryParse(input, out float value))
                {
                    return value;
                }
                Console.Write("Please enter a valid number: ");
            }
        }
    }
}
