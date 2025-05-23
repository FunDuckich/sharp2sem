using System;
using System.IO;

namespace sharp2sem._22_1
{
    public static class Solution221Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\22_1\input.txt";
            string outputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\22_1\output.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(outputFilePath))
                {
                    int[,] adjacencyMatrix;
                    int source;
                    int destination;

                    using (StreamReader sr = new StreamReader(inputFilePath))
                    {
                        string nLine = sr.ReadLine();
                        if (nLine == null || !int.TryParse(nLine, out int n) || n < 0)
                        {
                            sw.WriteLine("Ошибка: Не удалось прочитать корректный размер матрицы (n) из файла.");
                            return;
                        }

                        if (n == 0)
                        {
                            sw.WriteLine("Размер матрицы 0, дальнейшая обработка не требуется.");
                            Orgraph emptyGraph = new Orgraph(new int[0, 0], sw);
                            emptyGraph.ShowMatrix();
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

                        string verticesLine = sr.ReadLine();
                        if (verticesLine == null)
                        {
                            sw.WriteLine("Ошибка: Отсутствует строка с вершинами для удаления дуги.");
                            return;
                        }

                        string[] verticesToRemoveStr = verticesLine.Split();

                        if (!int.TryParse(verticesToRemoveStr[0], out source) ||
                            !int.TryParse(verticesToRemoveStr[1], out destination))
                        {
                            sw.WriteLine("Ошибка: Некорректный формат вершин для удаления дуги.");
                            return;
                        }
                    }

                    Orgraph graph = new Orgraph(adjacencyMatrix, sw);

                    sw.WriteLine("Начальная матрица:");
                    graph.ShowMatrix();

                    sw.WriteLine($"\nПопытка удалить дугу {source} -> {destination}:");
                    graph.RemoveArc(source, destination);

                    sw.WriteLine("\nМатрица после попытки удаления дуги:");
                    graph.ShowMatrix();
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