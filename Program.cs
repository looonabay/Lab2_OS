
using System;
using System.Threading;
using System.Diagnostics;

namespace Lab2_OS;

class Program
{
    static int completedRows = 0;
    static object locker = new object();

    static void Main()
    {
        Checked check = new Checked();

        Console.WriteLine("Многопоточное умножение матриц");

        int n = check.NaturalReadInt(
            "Введите количество строк первой матрицы:");

        int m = check.NaturalReadInt(
            "Введите количество столбцов первой матрицы:");

        int k = check.NaturalReadInt(
            "Введите количество столбцов второй матрицы:");

        int t = check.NaturalReadInt(
            "Введите количество потоков:");

        int[,] A = new int[n, m];
        int[,] B = new int[m, k];
        int[,] C = new int[n, k];

        Random random = new Random();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                A[i, j] = random.Next(1, 10);
            }
        }

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < k; j++)
            {
                B[i, j] = random.Next(1, 10);
            }
        }

        if (n <= 10 && m <= 10 && k <= 10)
        {
            Console.WriteLine("Матрица A:");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(A[i, j] + "\t");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Матрица B:");

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    Console.Write(B[i, j] + "\t");
                }

                Console.WriteLine();
            }
        }

        completedRows = 0;

        Thread[] threads = new Thread[t];

        int rowsPerThread = n / t;
        int remainingRows = n % t;

        int startRow = 0;

        Console.WriteLine("Прогресс: 0%");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

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
                MultiplyRows(A, B, C, firstRow, lastRow);
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
                Console.WriteLine("Прогресс: " + progress + "%");
                lastProgress = progress;
            }

            if (currentRows == n)
            {
                break;
            }

            Thread.Sleep(50);
        }

        for (int i = 0; i < t; i++)
        {
            threads[i].Join();
        }

        stopwatch.Stop();

        if (lastProgress < 100)
        {
            Console.WriteLine("Прогресс: 100%");
        }

        Console.WriteLine("Время умножения: " +
                          stopwatch.ElapsedMilliseconds + " мс");

        if (n <= 10 && k <= 10)
        {
            Console.WriteLine("Результат умножения матриц (C):");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    Console.Write(C[i, j] + "\t");
                }

                Console.WriteLine();
            }
        }

        long checksum = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < k; j++)
            {
                checksum += C[i, j];
            }
        }

        Console.WriteLine("Контрольная сумма: " + checksum);
    }

    static void MultiplyRows(int[,] A, int[,] B, int[,] C,
        int startRow, int endRow)
    {
        int m = A.GetLength(1);
        int k = B.GetLength(1);

        for (int i = startRow; i < endRow; i++)
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
    }
}
