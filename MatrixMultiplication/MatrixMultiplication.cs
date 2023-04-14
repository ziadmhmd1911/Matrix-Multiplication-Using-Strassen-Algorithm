using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class MatrixMultiplication
    {
        static public int[,] AddMatrices(int[,] matrix1, int[,] matrix2, int len)
        {
            int[,] result = new int[len, len];
            for (int row = 0; row < len; row++)
            {
                for (int col = 0; col < len; col++)
                {
                    result[row, col] = matrix1[row, col] + matrix2[row, col];
                }
            }
            return result;
        }
        static public int[,] SubtractMatrices(int[,] matrix1, int[,] matrix2, int len)
        {
            int[,] result = new int[len, len];
            for (int row = 0; row < len; row++)
            {
                for (int col = 0; col < len; col++)
                {
                    result[row, col] = matrix1[row, col] - matrix2[row, col];
                }
            }
            return result;
        }
        static public int[,] mmpp(int[,] A, int[,] B, int N)
        {
            //int N = A.GetLength(0);
            int[,] C = new int[N, N];
            //Base Case
            if (N <= 128)
            {
                int[][] matrixA = new int[N][];
                int[][] matrixB = new int[N][];
                for (int i = 0; i < N; i++)
                {
                    matrixA[i] = new int[N];
                    matrixB[i] = new int[N];
                    for (int j = 0; j < N; j++)
                    {
                        matrixA[i][j] = A[i, j];
                        matrixB[i][j] = B[i, j];
                    }
                }
                int[][] result = new int[N][];
                for (int i = 0; i < N; i++)
                {
                    result[i] = new int[N];
                    for (int j = 0; j < N; j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < N; k++)
                        {
                            sum += matrixA[i][k] * matrixB[k][j];
                        }
                        result[i][j] = sum;
                    }
                }
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        C[i, j] = result[i][j];
                    }
                }
                return C;
            }
            int half = N / 2;
            int[,] a11 = new int[half, half];
            int[,] a12 = new int[half, half];
            int[,] a21 = new int[half, half];
            int[,] a22 = new int[half, half];
            int[,] b11 = new int[half, half];
            int[,] b12 = new int[half, half];
            int[,] b21 = new int[half, half];
            int[,] b22 = new int[half, half];
            for (int i = 0; i < half; i++)
            {
                for (int j = 0; j < half; j++)
                {
                    a11[i, j] = A[i, j];
                    a12[i, j] = A[i, j + half];
                    a21[i, j] = A[i + half, j];
                    a22[i, j] = A[i + half, j + half];

                    b11[i, j] = B[i, j];
                    b12[i, j] = B[i, j + half];
                    b21[i, j] = B[i + half, j];
                    b22[i, j] = B[i + half, j + half];
                }
            }
            int[,] p1 = new int[half,half];
            int[,] p2 = new int[half,half];
            int[,] p3 = new int[half, half];
            int[,] p4 = new int[half, half];
            int[,] p5 = new int[half, half];
            int[,] p6 = new int[half, half];
            int[,] p7 = new int[half, half];
            Parallel.Invoke(
            () => p1 = mmpp(AddMatrices(a11, a22, half), AddMatrices(b11, b22, half), half),
            () => p2 = mmpp(AddMatrices(a21, a22, half), b11, half),
            () => p3 = mmpp(a11, SubtractMatrices(b12, b22, half), half),
            () => p4 = mmpp(a22, SubtractMatrices(b21, b11, half), half),
            () => p5 = mmpp(AddMatrices(a11, a12, half), b22, half),
            () => p6 = mmpp(SubtractMatrices(a21, a11, half), AddMatrices(b11, b12, half), half),
            () => p7 = mmpp(SubtractMatrices(a12, a22, half), AddMatrices(b21, b22, half), half)
            );



            int[,] tempc11 = AddMatrices(p1, p4, half);
            int[,] tempc112 = SubtractMatrices(tempc11, p5, half);
            int[,] c11 = AddMatrices(tempc112, p7, half);

            int[,] c12 = AddMatrices(p3, p5, half);    //p3+p5

            int[,] c21 = AddMatrices(p2, p4, half);    //p2+p4

            int[,] tempc22 = SubtractMatrices(p1, p2, half);
            int[,] tempc221 = AddMatrices(tempc22, p3, half);
            int[,] c22 = AddMatrices(tempc221, p6, half);

            for (int i=0; i<half; i++)
            {
                for (int j = 0; j < half; j++)
                {
                    C[i, j] = c11[i, j];
                    C[i, j + half] = c12[i, j];
                    C[i + half, j] = c21[i, j];
                    C[i + half, j + half] = c22[i, j];
                }
            }
            return C;
        }
        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 square matrices in an efficient way [Strassen's Method]
        /// </summary>
        /// <param name="M1">First square matrix</param>
        /// <param name="M2">Second square matrix</param>
        /// <param name="N">Dimension (power of 2)</param>
        /// <returns>Resulting square matrix</returns>
        static public int[,] MatrixMultiply(int[,] M1, int[,] M2, int N)
        {
            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();
            int[,] Ans = mmpp(M1, M2, N);
            return Ans;
        }
    }
}