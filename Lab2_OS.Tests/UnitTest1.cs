
using System;
using NUnit.Framework;
using Lab2_OS;

namespace Lab2_OS.Tests;

public class Tests
{
    // Тест 1. Умножение двух матриц 2x2
    [Test]
    public void Multiply_TwoMatrices2x2()
    {
        int[,] A =
        {
            { 1, 2 },
            { 3, 4 }
        };

        int[,] B =
        {
            { 5, 6 },
            { 7, 8 }
        };

        int[,] expected =
        {
            { 19, 22 },
            { 43, 50 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 2);

        Assert.That(actual, Is.EqualTo(expected));
    }

    // Тест 2. Умножение прямоугольных матриц 2x3 и 3x2
    [Test]
    public void Multiply_RectangularMatrices()
    {
        int[,] A =
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        int[,] B =
        {
            { 7, 8 },
            { 9, 10 },
            { 11, 12 }
        };

        int[,] expected =
        {
            { 58, 64 },
            { 139, 154 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 2);

        Assert.That(actual, Is.EqualTo(expected));
    }

    // Тест 3. Умножение на единичную матрицу
    [Test]
    public void Multiply_IdentityMatrix()
    {
        int[,] A =
        {
            { 2, 3 },
            { 4, 5 }
        };

        int[,] B =
        {
            { 1, 0 },
            { 0, 1 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 2);

        Assert.That(actual, Is.EqualTo(A));
    }

    // Тест 4. Умножение на нулевую матрицу
    [Test]
    public void Multiply_ZeroMatrix()
    {
        int[,] A =
        {
            { 2, 3 },
            { 4, 5 }
        };

        int[,] B =
        {
            { 0, 0 },
            { 0, 0 }
        };

        int[,] expected =
        {
            { 0, 0 },
            { 0, 0 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 2);

        Assert.That(actual, Is.EqualTo(expected));
    }

    // Тест 5. Потоков больше, чем строк
    [Test]
    public void Multiply_MoreThreadsThanRows()
    {
        int[,] A =
        {
            { 1, 2 },
            { 3, 4 }
        };

        int[,] B =
        {
            { 5 },
            { 6 }
        };

        int[,] expected =
        {
            { 17 },
            { 39 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 5);

        Assert.That(actual, Is.EqualTo(expected));
    }

    // Тест 6. Проверка несовместимых размеров матриц
    [Test]
    public void Multiply_IncompatibleMatrices()
    {
        int[,] A =
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        int[,] B =
        {
            { 1, 2 },
            { 3, 4 }
        };

        Assert.Throws<ArgumentException>(() =>
            MatrixMultiplier.Multiply(A, B, 2));
    }

    // Тест 7. Проверка количества потоков: 1, 3 и 7
    [Test]
    public void Multiply_DifferentThreadCounts()
    {
        int[,] A =
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        int[,] B =
        {
            { 7, 8 },
            { 9, 10 },
            { 11, 12 }
        };

        int[,] expected =
        {
            { 58, 64 },
            { 139, 154 }
        };

        int[] threadCounts = { 1, 3, 7 };

        for (int i = 0; i < threadCounts.Length; i++)
        {
            int[,] actual = MatrixMultiplier.Multiply(A, B, threadCounts[i]);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    // Тест 8. Проверка матрицы размером 1x1
    [Test]
    public void Multiply_OneElement()
    {
        int[,] A =
        {
            { 5 }
        };

        int[,] B =
        {
            { 4 }
        };

        int[,] expected =
        {
            { 20 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 1);

        Assert.That(actual, Is.EqualTo(expected));
    }

    // Тест 9. Проверка матрицы с одной строкой
    [Test]
    public void Multiply_OneRow()
    {
        int[,] A =
        {
            { 1, 2, 3 }
        };

        int[,] B =
        {
            { 4, 5 },
            { 6, 7 },
            { 8, 9 }
        };

        int[,] expected =
        {
            { 40, 46 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 2);

        Assert.That(actual, Is.EqualTo(expected));
    }

    // Тест 10. Проверка матрицы с одним столбцом
    [Test]
    public void Multiply_OneColumn()
    {
        int[,] A =
        {
            { 1, 2 },
            { 3, 4 }
        };

        int[,] B =
        {
            { 5 },
            { 6 }
        };

        int[,] expected =
        {
            { 17 },
            { 39 }
        };

        int[,] actual = MatrixMultiplier.Multiply(A, B, 2);

        Assert.That(actual, Is.EqualTo(expected));
    }

    // Тест 11. Проверка завершения прогресса на 100%
    [Test]
    public void Multiply_ProgressReaches100()
    {
        int[,] A =
        {
            { 1, 2 },
            { 3, 4 }
        };

        int[,] B =
        {
            { 5, 6 },
            { 7, 8 }
        };

        int lastProgress = 0;

        MatrixMultiplier.Multiply(A, B, 2, progress =>
        {
            lastProgress = progress;
        });

        Assert.That(lastProgress, Is.EqualTo(100));
    }

    // Тест 12. Повторное выполнение умножения 100 раз
    [Test]
    public void Multiply_Repeat100Times()
    {
        int[,] A =
        {
            { 1, 2 },
            { 3, 4 }
        };

        int[,] B =
        {
            { 5, 6 },
            { 7, 8 }
        };

        int[,] expected =
        {
            { 19, 22 },
            { 43, 50 }
        };

        for (int i = 0; i < 100; i++)
        {
            int[,] actual = MatrixMultiplier.Multiply(A, B, 7);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
