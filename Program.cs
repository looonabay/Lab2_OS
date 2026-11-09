
using System;
using System.Diagnostics;

namespace Lab2_OS;

class Program
{
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

        Console.WriteLine("Прогресс: 0%");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int[,] C = MatrixMultiplier.Multiply(A, B, t, progress =>
        {
            Console.WriteLine("Прогресс: " + progress + "%");
        });

        stopwatch.Stop();

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
}
