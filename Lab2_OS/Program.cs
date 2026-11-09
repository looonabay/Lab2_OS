
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Lab2_OS;

class Program
{
    static void Main()
    {
        Console.WriteLine("Автоматическое тестирование умножения матриц");

        int[] sizes = { 500, 1000, 1500 };
        int[] threadCounts = { 1, 2, 4, 8, 16, 32, 64 };

        string measurementsFile = "measurements.csv";
        string averagesFile = "averages.csv";

        File.WriteAllText(measurementsFile,
            "Размер;Потоки;Измерение;Время_мс;Контрольная_сумма\n");

        File.WriteAllText(averagesFile,
            "Размер;Потоки;Среднее_время_мс\n");

        // Прогрев JIT-компилятора
        int[,] warmA = new int[10, 10];
        int[,] warmB = new int[10, 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                warmA[i, j] = 1;
                warmB[i, j] = 1;
            }
        }

        Console.WriteLine("\nПрогрев программы...");

        MatrixMultiplier.Multiply(warmA, warmB, 1);

        Console.WriteLine("Прогрев завершен.");

        Random random = new Random();

        foreach (int size in sizes)
        {
            Console.WriteLine("\n================================");
            Console.WriteLine("Размер матриц: " + size + " x " + size);
            Console.WriteLine("================================");

            int[,] A = new int[size, size];
            int[,] B = new int[size, size];

            // Создание первой матрицы
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    A[i, j] = random.Next(1, 10);
                }
            }

            // Создание второй матрицы
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    B[i, j] = random.Next(1, 10);
                }
            }

            long expectedChecksum = -1;

            foreach (int t in threadCounts)
            {
                Console.WriteLine("\nКоличество потоков: " + t);

                List<double> times = new List<double>();

                for (int run = 1; run <= 5; run++)
                {
                    Stopwatch stopwatch = new Stopwatch();

                    stopwatch.Start();

                    int[,] C = MatrixMultiplier.Multiply(A, B, t);

                    stopwatch.Stop();

                    double elapsed = stopwatch.Elapsed.TotalMilliseconds;

                    // Вычисление контрольной суммы
                    long checksum = 0;

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            checksum += C[i, j];
                        }
                    }

                    if (expectedChecksum == -1)
                    {
                        expectedChecksum = checksum;
                    }
                    else if (checksum != expectedChecksum)
                    {
                        throw new Exception("Ошибка: результаты умножения отличаются!");
                    }

                    times.Add(elapsed);

                    Console.WriteLine(
                        "Измерение " + run + ": " +
                        elapsed.ToString("F2", CultureInfo.InvariantCulture) +
                        " мс");

                    // Сохранение каждого измерения
                    File.AppendAllText(measurementsFile,
                        size + ";" + t + ";" + run + ";" +
                        elapsed.ToString("F2", CultureInfo.InvariantCulture) +
                        ";" + checksum + "\n");
                }

                // Сортировка результатов
                times.Sort();

                // Удаление минимального и максимального значений
                double average = (times[1] + times[2] + times[3]) / 3;

                Console.WriteLine(
                    "Среднее время: " +
                    average.ToString("F2", CultureInfo.InvariantCulture) +
                    " мс");

                // Сохранение среднего времени
                File.AppendAllText(averagesFile,
                    size + ";" + t + ";" +
                    average.ToString("F2", CultureInfo.InvariantCulture) +
                    "\n");
            }
        }

        Console.WriteLine("\n================================");
        Console.WriteLine("Все измерения завершены!");
        Console.WriteLine("Файл измерений: " + Path.GetFullPath(measurementsFile));
        Console.WriteLine("Файл средних значений: " + Path.GetFullPath(averagesFile));
        Console.WriteLine("================================");
    }
}
