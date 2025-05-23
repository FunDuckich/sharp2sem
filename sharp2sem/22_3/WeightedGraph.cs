using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace sharp2sem._22_3
{
    public class WeightedGraph
    {
        private class GraphRepresentation
        {
            private double[,] _weightedAdjacencyMatrix;

            public int Size { get; }

            public GraphRepresentation(int n, List<City> cities, int[,] connectivityMatrix)
            {
                Size = n;
                _weightedAdjacencyMatrix = new double[n, n];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        _weightedAdjacencyMatrix[i, j] = double.PositiveInfinity;
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    _weightedAdjacencyMatrix[i, i] = 0;
                    for (int j = i + 1; j < n; j++)
                    {
                        if (connectivityMatrix[i, j] != 0 || connectivityMatrix[j, i] != 0)
                        {
                            double distance = cities[i].DistanceTo(cities[j]);
                            _weightedAdjacencyMatrix[i, j] = distance;
                            _weightedAdjacencyMatrix[j, i] = distance;
                        }
                    }
                }
            }

            public Tuple<double, List<int>> DijkstraShortestPath(int startNodeIndex, int endNodeIndex,
                int forbiddenNodeIndex)
            {
                if (startNodeIndex < 0 || startNodeIndex >= Size ||
                    endNodeIndex < 0 || endNodeIndex >= Size)
                {
                    return Tuple.Create(double.PositiveInfinity, new List<int>());
                }

                double[] distances = new double[Size];
                int[] predecessors = new int[Size];
                bool[] visited = new bool[Size];

                for (int i = 0; i < Size; i++)
                {
                    distances[i] = double.PositiveInfinity;
                    predecessors[i] = -1;
                    visited[i] = false;
                }

                distances[startNodeIndex] = 0;

                for (int count = 0; count < Size; count++)
                {
                    int u = -1;
                    double minDistance = double.PositiveInfinity;

                    for (int vIdx = 0; vIdx < Size; vIdx++)
                    {
                        if (!visited[vIdx] && distances[vIdx] < minDistance)
                        {
                            if (vIdx == forbiddenNodeIndex && vIdx != startNodeIndex && vIdx != endNodeIndex)
                            {
                                continue;
                            }

                            minDistance = distances[vIdx];
                            u = vIdx;
                        }
                    }

                    if (u == -1 || double.IsPositiveInfinity(distances[u])) break;

                    visited[u] = true;

                    if (u == endNodeIndex) break;

                    for (int v = 0; v < Size; v++)
                    {
                        if (v == forbiddenNodeIndex && v != endNodeIndex)
                        {
                            continue;
                        }

                        if (!visited[v] && !double.IsPositiveInfinity(_weightedAdjacencyMatrix[u, v]) &&
                            distances[u] + _weightedAdjacencyMatrix[u, v] < distances[v])
                        {
                            distances[v] = distances[u] + _weightedAdjacencyMatrix[u, v];
                            predecessors[v] = u;
                        }
                    }
                }

                List<int> path = new List<int>();
                if (double.IsPositiveInfinity(distances[endNodeIndex]))
                {
                    return Tuple.Create(double.PositiveInfinity, path);
                }

                int current = endNodeIndex;
                while (current != -1)
                {
                    path.Add(current);
                    current = predecessors[current];
                }

                path.Reverse();

                if ((path.Count == 0 && startNodeIndex != endNodeIndex) ||
                    (path.Count > 0 && path[0] != startNodeIndex))
                {
                    return Tuple.Create(double.PositiveInfinity, new List<int>());
                }

                return Tuple.Create(distances[endNodeIndex], path);
            }
        }

        private GraphRepresentation _graphRepresentation;
        private StreamWriter _fileOut;
        private List<City> _citiesList;

        public WeightedGraph(List<City> cities, int[,] connectivityMatrix, StreamWriter fileOut)
        {
            _fileOut = fileOut;
            _citiesList = cities;

            if (cities == null || cities.Count == 0 || connectivityMatrix == null)
            {
                _graphRepresentation = new GraphRepresentation(0, new List<City>(), new int[0, 0]);
                _fileOut.WriteLine(
                    "Ошибка: некорректные входные данные для создания графа (города или матрица связности).");
                return;
            }

            int n = cities.Count;
            if (n != connectivityMatrix.GetLength(0) || n != connectivityMatrix.GetLength(1))
            {
                _graphRepresentation = new GraphRepresentation(0, new List<City>(), new int[0, 0]);
                _fileOut.WriteLine("Ошибка: размерность матрицы связности не соответствует количеству городов.");
                return;
            }

            _graphRepresentation = new GraphRepresentation(n, cities, connectivityMatrix);
        }

        public void FindAndPrintShortestPathAvoidingCity(int startCityIndex, int endCityIndex, int forbiddenCityIndex)
        {
            if (_graphRepresentation == null || _graphRepresentation.Size == 0)
            {
                _fileOut.WriteLine("Граф не инициализирован или пуст.");
                return;
            }

            if (startCityIndex < 0 || startCityIndex >= _graphRepresentation.Size ||
                endCityIndex < 0 || endCityIndex >= _graphRepresentation.Size ||
                forbiddenCityIndex < 0 || forbiddenCityIndex >= _graphRepresentation.Size)
            {
                _fileOut.WriteLine("Ошибка: некорректные индексы городов.");
                return;
            }

            if (startCityIndex == forbiddenCityIndex)
            {
                _fileOut.WriteLine(
                    $"Невозможно найти путь из города '{_citiesList[startCityIndex].Name}' в '{_citiesList[endCityIndex].Name}', так как начальный город является запрещенным ('{_citiesList[forbiddenCityIndex].Name}').");
                return;
            }

            if (endCityIndex == forbiddenCityIndex)
            {
                _fileOut.WriteLine(
                    $"Невозможно найти путь из города '{_citiesList[startCityIndex].Name}' в '{_citiesList[endCityIndex].Name}', так как конечный город является запрещенным ('{_citiesList[forbiddenCityIndex].Name}').");
                return;
            }

            Tuple<double, List<int>> result =
                _graphRepresentation.DijkstraShortestPath(startCityIndex, endCityIndex, forbiddenCityIndex);

            double distance = result.Item1;
            List<int> pathIndices = result.Item2;

            _fileOut.WriteLine(
                $"Поиск кратчайшего пути из '{_citiesList[startCityIndex].Name}' в '{_citiesList[endCityIndex].Name}', не проходя через '{_citiesList[forbiddenCityIndex].Name}':");

            if (double.IsPositiveInfinity(distance) || pathIndices.Count == 0)
            {
                _fileOut.WriteLine("Путь не найден.");
            }
            else
            {
                _fileOut.WriteLine($"Кратчайшее расстояние: {distance:F2}");
                List<string> cityNamesInPath = pathIndices.Select(index => _citiesList[index].Name).ToList();
                _fileOut.WriteLine($"Путь: {string.Join(" -> ", cityNamesInPath)}");
            }
        }
    }
}