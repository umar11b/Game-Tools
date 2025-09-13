using System;

namespace lab1.matrices
{
    public class CustomMatrix
    {
        private float[,] data;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public CustomMatrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new float[rows, cols];
        }

        public float this[int row, int col]
        {
            get { return data[row, col]; }
            set { data[row, col] = value; }
        }

        public static CustomMatrix operator +(CustomMatrix a, CustomMatrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("Matrices must have the same dimensions for addition");

            CustomMatrix result = new CustomMatrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }
            return result;
        }

        public static CustomMatrix operator -(CustomMatrix a, CustomMatrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("Matrices must have the same dimensions for subtraction");

            CustomMatrix result = new CustomMatrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }
            return result;
        }

        public static CustomMatrix operator *(CustomMatrix a, CustomMatrix b)
        {
            if (a.Cols != b.Rows)
                throw new ArgumentException("Number of columns in first matrix must equal number of rows in second matrix");

            CustomMatrix result = new CustomMatrix(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Cols; j++)
                {
                    float sum = 0;
                    for (int k = 0; k < a.Cols; k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Rows; i++)
            {
                result += "[";
                for (int j = 0; j < Cols; j++)
                {
                    result += data[i, j].ToString("F2");
                    if (j < Cols - 1) result += ", ";
                }
                result += "]";
                if (i < Rows - 1) result += "\n";
            }
            return result;
        }
    }
}
