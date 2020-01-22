using System;
using System.Collections.Generic;

namespace Matrices
{
    /// <summary>
    /// Matrix of types double
    /// </summary>
    public class Matrix
    {
        private double[,] data;

        /// <summary>
        /// Creates a new matrix instance
        /// </summary>
        /// <param name="data">Two dimensional array of double type</param>
        /// <param name="Height">Number of rows in matrix</param>
        /// <param name="Width">Number of colums in matrix</param>
        public Matrix(double[,] data, int Height, int Width)
        {
            if (Height <= 0 || Width <= 0)
            {
                throw new InvalidOperationException("Matrix dimensions must be positive");
            }

            this.data = data;
            this.Height = Height;
            this.Width = Width;
        }

        /// <summary>
        /// Converts a formated string to matrix
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static Matrix FromString(string inputString)
        {
            List<char> numbers = new List<char>(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'});
            List<char> legalChars = new List<char>(new char[] { ' ', '\t', ',', '-', '\r', '\n' });
            List<double> inputValues = new List<double>();
            int column = 0, row = 0, pointer = 0;
            string tempString = "";

            while(pointer < inputString.Length)
            {
                if(!(numbers.Exists(c => c == inputString[pointer]) || legalChars.Exists(c => c == inputString[pointer])))
                {
                    throw new InvalidCastException($"Char {inputString[pointer]} is not allowed");
                }

                if(inputString[pointer] == ' ' || inputString[pointer] == '\t')
                {
                    pointer++;
                    column++;
                    inputValues.Add(double.Parse(tempString));
                    tempString = "";
                    continue;
                }
                else if(inputString[pointer] == '\r' || inputString[pointer] == '\n')
                {
                    pointer++;
                    row++;
                    column = 0;
                    inputValues.Add(double.Parse(tempString));
                    tempString = "";
                    continue;
                }
                else
                {
                    tempString += inputString[pointer];
                    pointer++;
                }
            }

            if(tempString != "")
            {
                inputValues.Add(double.Parse(tempString));
            }

            Matrix temp = new Matrix(new double[row + 1, column + 1], row + 1, column + 1);
            pointer = 0;
            
            for(int i = 0; i <= row; i++)
            {
                for(int j = 0; j <= column; j++)
                {
                    try
                    {
                        temp[i, j] = inputValues[pointer];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new FormatException("String is empty");
                    }
                    pointer++;
                }
            }
            return temp;
        }

        /// <summary>
        /// Generates a formated string containing the matrix
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string temp = "";
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    temp += $"{data[i, j]}";
                    if(j < Width - 1)
                    {
                        temp += '\t';
                    }
                }
                if(i < Height - 1)
                {
                    temp += '\n';
                }
            }

            return temp;
        }

        /// <summary>
        /// Prints matrix to console
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write($"{data[i, j]}\t");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints matrix to console
        /// </summary>
        /// <param name="inp">Matrix to print</param>
        public static void Print(Matrix inp)
        {
            for (int i = 0; i < inp.Height; i++)
            {
                for (int j = 0; j < inp.Width; j++)
                {
                    Console.Write(" {0} ", inp.data[i, j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Returns a transposed matrix
        /// </summary>
        /// <param name="a">Matrix to transpose</param>
        /// <returns></returns>
        public static Matrix Transpose(Matrix a)
        {
            Matrix temp = new Matrix(new double[a.Width, a.Height], a.Width, a.Height);
            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    temp[j, i] = a[i, j];
                }
            }
            return temp;
        }

        /// <summary>
        /// Returns an identity matrix of given size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix Identity(int size)
        {
            Matrix temp = new Matrix(new double[size, size], size, size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        temp[i, j] = 1;
                    }
                    else
                    {
                        temp[i, j] = 0;
                    }
                }
            }

            return temp;
        }

        /// <summary>
        /// Returns an elementary matrix for swapping rows
        /// </summary>
        /// <param name="size"></param>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <returns></returns>
        public static Matrix Elementary(int size, int row1, int row2)
        {
            Matrix temp = Identity(size);

            temp[row1, row1] = temp[row2, row2] = 0;
            temp[row1, row2] = temp[row2, row1] = 1;

            return temp;
        }

        /// <summary>
        /// Returns an elementary matrix for multiplying the given row
        /// </summary>
        /// <param name="size"></param>
        /// <param name="row"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static Matrix Elementary(int size, int row, double multiplier)
        {
            Matrix temp = Identity(size);

            temp[row, row] *= multiplier;

            return temp;
        }

        /// <summary>
        /// Returns an elementary matrix for adding multiplied row to another row
        /// </summary>
        /// <param name="size"></param>
        /// <param name="row1">Row that will be multiplied and added</param>
        /// <param name="row2">Row to add to</param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static Matrix Elementary(int size, int row1, int row2, double multiplier)
        {
            Matrix temp = Identity(size);

            temp[row2, row1] = multiplier;

            return temp;
        }

        /// <summary>
        /// Returns a submatrix without the given row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Matrix Submatrix(int row, int column)
        {
            Matrix temp = new Matrix(new double[Height - 1, Width - 1], Height - 1, Width - 1);

            int k = 0;
            for (int i = 0; i < temp.Height; i++)
            {
                if (i == row)
                {
                    k++;
                }

                int l = 0;
                for (int j = 0; j < temp.Width; j++)
                {
                    if (l == column)
                    {
                        l++;
                    }
                    temp[i, j] = data[k, l];
                    l++;
                }
                k++;
            }

            return temp;
        }

        /// <summary>
        /// Calculates the determinant of a matrix
        /// </summary>
        /// <returns></returns>
        public double Determinant
        {
            get
            {
                if (Height != Width)
                {
                    throw new InvalidOperationException("Matrix need to be square to calculate determinant");
                }

                if (Width == 1)
                {
                    return data[0, 0];
                }
                else
                {
                    double det = 0;
                    for (int i = 0; i < Width; i++)
                    {
                        det += Math.Pow(-1, i + 2) * data[0, i] * Submatrix(0, i).Determinant;
                    }

                    return det;
                }
            }
        }


        /// <summary>
        /// Returns a matrix of algebraic complements
        /// </summary>
        /// <returns></returns>
        public Matrix Complement()
        {
            if (Width != Height)
            {
                throw new InvalidOperationException("Matrix must be square to complement");
            }
            Matrix temp = new Matrix(new double[Width, Width], Width, Width);

            for (int i = 0; i < temp.Width; i++)
            {
                for (int j = 0; j < temp.Width; j++)
                {
                    temp[i, j] = Math.Pow(-1, i + j + 2) * this.Submatrix(i, j).Determinant;
                }
            }

            return temp;
        }

        /// <summary>
        /// Returns an inverse matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            if (Width != Height)
            {
                throw new InvalidOperationException("Matrix must be square to calculate inverse");
            }

            double det = Determinant;

            if (det == 0)
            {
                throw new InvalidOperationException("Matrix determinant = 0, Inverse does not exist");
            }

            return 1 / det * Complement().Transpose();
        }

        /// <summary>
        /// Returns a transposed matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            Matrix temp = new Matrix(new double[Width, Height], Width, Height);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    temp[j, i] = data[i, j];
                }
            }
            return temp;
        }

        /// <summary>
        /// Number of rows
        /// </summary>
        public int Height
        {
            get;
        }

        /// <summary>
        /// Number of columns
        /// </summary>
        public int Width
        {
            get;
        }

        /// <summary>
        /// Returns a specified cell of the matrix
        /// </summary>
        /// <param name="i">Row number (starts from 0)</param>
        /// <param name="j">Column number (starts from 0)</param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            set
            {
                data[i, j] = value;
            }

            get
            {
                return data[i, j];
            }
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Height != b.Height || a.Width != b.Width)
            {
                throw new InvalidOperationException("Matrices are not of the same size");
            }

            Matrix temp = new Matrix(new double[a.Height, a.Width], a.Height, a.Width);

            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    temp[i, j] = a[i, j] + b[i, j];
                }
            }
            return temp;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Height != b.Height || a.Width != b.Width)
            {
                throw new InvalidOperationException("Matrices are not of the same size");
            }

            Matrix temp = new Matrix(new double[a.Height, a.Width], a.Height, a.Width);

            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    temp[i, j] = a[i, j] - b[i, j];
                }
            }
            return temp;
        }

        public static Matrix operator *(Matrix a, double b)
        {
            Matrix temp = new Matrix(new double[a.Height, a.Width], a.Height, a.Width);

            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    temp[i, j] = a[i, j] * b;
                }
            }
            return temp;
        }

        public static Matrix operator *(double b, Matrix a)
        {
            Matrix temp = new Matrix(new double[a.Height, a.Width], a.Height, a.Width);

            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    temp[i, j] = a[i, j] * b;
                }
            }
            return temp;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Width != b.Height)
            {
                throw new InvalidOperationException("Matrices are of invalid size to be multiplied (Width of a != Height of b)");
            }

            Matrix temp = new Matrix(new double[a.Height, b.Width], a.Height, b.Width);

            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    double sum = 0;
                    for (int x = 0; x < a.Width; x++)
                    {
                        sum += a[i, x] * b[x, j];
                    }
                    temp[i, j] = sum;
                }
            }

            return temp;
        }

        public static Matrix operator -(Matrix a)
        {
            return -1 * a;
        }
    }
}