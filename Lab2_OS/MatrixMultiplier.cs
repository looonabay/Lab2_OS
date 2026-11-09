
using System;
using System.Threading;

namespace Lab2_OS;

public class MatrixMultiplier
{
    public static int[,] Multiply(int[,] A, int[,] B, int t,
        Action<int>? progressChanged = null)
    {
        if (A == null || B == null)
        {
            throw new ArgumentException("Матрицы не должны быть пустыми.");
        }

        if (A.GetLength(1) != B.GetLength(0))
        {
            throw new ArgumentException("Размеры матриц несовместимы.");
        }

        if (t <= 0)
        {
            throw new ArgumentException("Количество потоков должно быть положительным.");
        }

        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int k = B.GetLength(1);

        int[,] C = new int[n, k];

        int completedRows = 0;
        object locker = new object();

        Thread[] threads = new Thread[t];

        int rowsPerThread = n / t;
        int remainingRows = n % t;

        int startRow = 0;

        for (int i = 0; i < t; i++)
        {
            int rows = rowsPerThread;

            if (i < remainingRows)
            {
                rows++;
            }

            int endRow = startRow + rows;

            int firstRow = startRow;
            int lastRow = endRow;

            threads[i] = new Thread(() =>
            {
                for (int i = firstRow; i < lastRow; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        for (int p = 0; p < m; p++)
                        {
                            C[i, j] += A[i, p] * B[p, j];
                        }
                    }

                    lock (locker)
                    {
                        completedRows++;
                    }
                }
            });

            threads[i].Start();

            startRow = endRow;
        }

        int lastProgress = 0;

        while (true)
        {
            int currentRows;

            lock (locker)
            {
                currentRows = completedRows;
            }

            int progress = currentRows * 100 / n;

            if (progress != lastProgress)
            {
                if (progressChanged != null)
                {
                    progressChanged(progress);
                }

                lastProgress = progress;
            }

            if (currentRows == n)
            {
                break;
            }

            Thread.Sleep(10);
        }

        for (int i = 0; i < t; i++)
        {
            threads[i].Join();
        }

        if (lastProgress < 100 && progressChanged != null)
        {
            progressChanged(100);
        }

        return C;
    }
}
