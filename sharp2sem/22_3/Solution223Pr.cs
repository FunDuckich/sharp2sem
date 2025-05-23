// Solution223Pr.cs
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace sharp2sem._22_3
{
    public static class Solution223Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\22_3\input.txt"; 
            string outputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\22_3\output.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(outputFilePath))
                {
                    List<City> cities = new List<City>();
                    int[,] connectivityMatrix;
                    int n;

                    using (StreamReader sr = new StreamReader(inputFilePath))
                    {
                        string nLine = sr.ReadLine();
                        if (nLine == null || !int.TryParse(nLine, out n) || n <= 0) 
                        {
                            sw.WriteLine("Ошибка: Не удалось прочитать корректное количество городов (N) из файла.");
                            return;
                        }

                        for (int i = 0; i < n; i++)
                        {
                            string cityLine = sr.ReadLine();
                            if (cityLine == null)
                            {
                                sw.WriteLine($"Ошибка: Недостаточно строк для описания городов. Ожидалось {n} строк.");
                                return;
                            }
                            
                            string[] parts = cityLine.Trim().Split(' '); 
                            
                            if (parts.Length < 3) 
                            {
                                sw.WriteLine($"Ошибка: Некорректный формат данных для города в строке (слишком мало частей): '{cityLine}'. Ожидалось: [Имя/Имена] X Y");
                                return;
                            }

                            string cityName = string.Join(" ", parts.Take(parts.Length - 2));
                            if (!int.TryParse(parts[parts.Length - 2], out int x) || !int.TryParse(parts[parts.Length - 1], out int y))
                            {
                                sw.WriteLine($"Ошибка: Некорректный формат координат для города в строке: '{cityLine}'. Координаты X и Y должны быть последними двумя элементами.");
                                return;
                            }
                            cities.Add(new City(cityName, x, y, i));
                        }

                        connectivityMatrix = new int[n, n];
                        for (int i = 0; i < n; i++)
                        {
                            string matrixLine = sr.ReadLine();
                            if (matrixLine == null)
                            {
                                sw.WriteLine($"Ошибка: Недостаточно строк для матрицы смежности. Ожидалось {n} строк.");
                                return;
                            }
                            string[] values = matrixLine.Split(); 
                            if (values.Length != n)
                            {
                                sw.WriteLine($"Ошибка: В строке {i + 1} матрицы смежности ожидалось {n} значений, найдено {values.Length}.");
                                return;
                            }
                            for (int j = 0; j < n; j++)
                            {
                                if (!int.TryParse(values[j], out connectivityMatrix[i, j])) 
                                {
                                    sw.WriteLine($"Ошибка: Некорректное значение '{values[j]}' в матрице смежности в позиции [{i},{j}].");
                                    return;
                                }
                            }
                        }
                    } 

                    Console.WriteLine("Список доступных городов:");
                    for(int i=0; i < cities.Count; i++)
                    {
                        Console.WriteLine($"{i}: {cities[i].Name}");
                    }

                    Console.Write("Введите индекс начального города A: ");
                    string cityAStr = Console.ReadLine();
                    Console.Write("Введите индекс конечного города B: ");
                    string cityBStr = Console.ReadLine();
                    Console.Write("Введите индекс города D (через который нельзя проходить): ");
                    string cityDStr = Console.ReadLine();

                    if (!int.TryParse(cityAStr, out int cityAIdx) ||
                        !int.TryParse(cityBStr, out int cityBIdx) ||
                        !int.TryParse(cityDStr, out int cityDIdx))
                    {
                        sw.WriteLine("Ошибка: Некорректный ввод индексов городов с клавиатуры.");
                        Console.WriteLine("Ошибка ввода индексов. Проверьте файл вывода.");
                        return;
                    }
                    
                    if (cityAIdx < 0 || cityAIdx >= n || cityBIdx < 0 || cityBIdx >= n || cityDIdx < 0 || cityDIdx >= n) {
                        sw.WriteLine("Ошибка: Один или несколько введенных индексов городов вне допустимого диапазона.");
                        Console.WriteLine("Ошибка: индексы вне диапазона. Проверьте файл вывода.");
                        return;
                    }

                    WeightedGraph graph = new WeightedGraph(cities, connectivityMatrix, sw);
                    graph.FindAndPrintShortestPathAvoidingCity(cityAIdx, cityBIdx, cityDIdx);
                    
                    Console.WriteLine("\nОбработка завершена. Результаты записаны в файл: " + Path.GetFullPath(outputFilePath));

                }
            }
            catch (FileNotFoundException)
            {
                 Console.WriteLine($"Ошибка: Входной файл не найден по пути: {inputFilePath}");
                 using (StreamWriter errorSw = new StreamWriter(outputFilePath, false))
                 {
                     errorSw.WriteLine($"Ошибка: Входной файл не найден по пути: {inputFilePath}");
                 }
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Произошла ошибка: {ex.Message}");
                 using (StreamWriter errorSw = new StreamWriter(outputFilePath, false))
                 {
                     errorSw.WriteLine($"Произошла ошибка: {ex.Message}\n{ex.StackTrace}");
                 }
            }
        }
    }
}