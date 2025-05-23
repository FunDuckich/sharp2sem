using System;
using System.IO;

namespace sharp2sem._22_2
{
    public static class Solution222Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\22_2\input.txt";
            string outputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\22_2\output.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(outputFilePath))
                {
                    int[,] adjacencyMatrix;
                    int n;
                    int startVertexA;

                    using (StreamReader sr = new StreamReader(inputFilePath))
                    {
                        string nLine = sr.ReadLine();
                        if (nLine == null || !int.TryParse(nLine, out n) || n < 0)
                        {
                            sw.WriteLine("Ошибка: Не удалось прочитать корректный размер матрицы (n) из файла.");
                            return;
                        }

                        if (n == 0)
                        {
                            sw.WriteLine("Размер матрицы 0, дальнейшая обработка не требуется.");
                            Orgraph emptyGraph = new Orgraph(new int[0, 0], sw);
                            emptyGraph.FindAndPrintHamiltonianCycle(0);
                            return;
                        }

                        adjacencyMatrix = new int[n, n];

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
                                sw.WriteLine(
                                    $"Ошибка: В строке {i} матрицы ожидалось {n} значений, найдено {values.Length}.");
                                return;
                            }

                            for (int j = 0; j < n; j++)
                            {
                                if (!int.TryParse(values[j], out adjacencyMatrix[i, j]))
                                {
                                    sw.WriteLine(
                                        $"Ошибка: Некорректное значение '{values[j]}' в матрице смежности в позиции [{i},{j}].");
                                    return;
                                }
                            }
                        }

                        string startVertexLine = sr.ReadLine();
                        if (startVertexLine == null || !int.TryParse(startVertexLine, out startVertexA))
                        {
                            sw.WriteLine("Ошибка: Не удалось прочитать начальную вершину A для Гамильтонова цикла.");
                            return;
                        }
                    }

                    Orgraph graph = new Orgraph(adjacencyMatrix, sw);
                    graph.FindAndPrintHamiltonianCycle(startVertexA);
                }
            }
            catch (FileNotFoundException)
            {
                using (StreamWriter errorSw = new StreamWriter(outputFilePath, false))
                {
                    errorSw.WriteLine($"Ошибка: Входной файл не найден по пути: {inputFilePath}");
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter errorSw = new StreamWriter(outputFilePath, false))
                {
                    errorSw.WriteLine($"Произошла ошибка: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }
    }
}