using System;
using System.Collections.Generic;
using System.IO;

namespace sharp2sem.PostfixNotation
{
    public static class PostfixSolution
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\Анна\Source\Repos\sharp2sem\sharp2sem\PostfixNotation\input.txt";
            string outputFilePath = @"C:\Users\Анна\Source\Repos\sharp2sem\sharp2sem\PostfixNotation\output.txt";

            using (var reader = new StreamReader(inputFilePath))
            using (var writer = new StreamWriter(outputFilePath))
            {
                try
                {
                    string line = reader.ReadToEnd().Trim();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        writer.WriteLine("Пустой ввод.");
                        return;
                    }

                    string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Stack<double> stack = new Stack<double>();

                    foreach (var token in tokens)
                    {
                        if (double.TryParse(token, out double number))
                        {
                            stack.Push(number);
                        }
                        else
                        {
                            if (stack.Count < 2)
                            {
                                writer.WriteLine("Ошибка: недостаточно операндов.");
                                return;
                            }

                            double b = stack.Pop();
                            double a = stack.Pop();

                            switch (token)
                            {
                                case "+": stack.Push(a + b); break;
                                case "-": stack.Push(a - b); break;
                                case "*": stack.Push(a * b); break;
                                case "/":
                                    if (b == 0)
                                    {
                                        writer.WriteLine("Ошибка: деление на ноль.");
                                        return;
                                    }
                                    stack.Push(a / b);
                                    break;
                                default:
                                    writer.WriteLine("Ошибка: неизвестный оператор.");
                                    return;
                            }
                        }
                    }

                    if (stack.Count != 1)
                    {
                        writer.WriteLine("Ошибка: неверное выражение.");
                        return;
                    }

                    writer.WriteLine(stack.Pop());
                }
                catch
                {
                    writer.WriteLine("Ошибка обработки.");
                }
            }
        }
    }
}
