using System;
using System.Linq;

namespace Lab2_OS;

// Один раз опишем и каждый раз будем его использовать
public class Checked
{
    // Проверка на int
    public int ReadInt(string promt)
    {
        Console.WriteLine(promt);
        string? input = Console.ReadLine();
        int x = 0;

        while (true)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine
                    ("Ошибка ввода! Нельзя вводить пустую" +
                     " строку.");
            }
            else if (int.TryParse(input, out x))
            {
                return x;
            }
            else
            {
                Console.WriteLine("Ошибка " +
                                  "ввода! Введите целое число:");
            }

            input = Console.ReadLine();
        }
    }

    // Проверка на натуральное число
    public int NaturalReadInt(string promt)
    {
        Console.WriteLine(promt);
        string? input = Console.ReadLine();
        int x = 0;

        while (true)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine
                    ("Ошибка ввода! Нельзя вводить " +
                     "пустую строку.");
            }
            else if (int.TryParse(input, out x) && x > 0)
            {
                return x;
            }
            else
            {
                Console.WriteLine
                    ("Ошибка ввода! Введите натуральное " +
                     "число больше нуля:");
            }

            input = Console.ReadLine();
        }
    }

    // Проверка на double
    public double ReadDouble(string promt)
    {
        Console.WriteLine(promt);
        string? input = Console.ReadLine();
        double x = 0;

        while (true)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine
                    ("Ошибка ввода! Нельзя вводить " +
                     "пустую строку.");
            }
            else if (double.TryParse(input, out x))
            {
                return x;
            }
            else
            {
                Console.WriteLine
                    ("Ошибка ввода! Введите число:");
            }

            input = Console.ReadLine();
        }
    }

    // Проверка на bool
    public bool ReadBool(string promt)
    {
        Console.WriteLine(promt);
        string? input = Console.ReadLine();
        bool x = false;

        while (true)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine
                    ("Ошибка ввода! Нельзя вводить " +
                     "пустую строку.");
            }
            else if (bool.TryParse(input, out x))
            {
                return x;
            }
            else
            {
                Console.WriteLine
                    ("Ошибка ввода! Введите true или false:");
            }

            input = Console.ReadLine();
        }
    }

    // Проверка на string
    public string ReadString(string promt)
    {
        Console.WriteLine(promt);
        string? input = Console.ReadLine();

        while (true)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
                Console.WriteLine
                    ("Ошибка ввода! Строка не должна " +
                     "быть пустой");
            }

            input = Console.ReadLine();
        }
    }
    
    // проверка для значений в определенном промежутке 
    public int ReadGrade(string prompt)
    {
        int value = ReadInt(prompt);
        if (value < 2 || value > 5)
        {
            Console.WriteLine
                ("Ошибка: оценка должна быть от 2 до 5.");
            return ReadGrade(prompt);
        }
        return value;
    }
}