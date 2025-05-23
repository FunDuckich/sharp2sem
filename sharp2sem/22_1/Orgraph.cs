using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace sharp2sem._22_1
{
    public class Orgraph
    {
        private class GraphRepresentation
        {
            private int[,] array;
            private bool[] nov;
            private StreamWriter _fileOut;

            public int Size { get; }

            public int this[int i, int j]
            {
                get { return array[i, j]; }
                set { array[i, j] = value; }
            }

            public GraphRepresentation(int n, StreamWriter fileOut)
            {
                Size = n;
                array = new int[n, n];
                nov = new bool[n];
                _fileOut = fileOut;
            }

            public void NovSet()
            {
                for (int i = 0; i < Size; i++)
                {
                    nov[i] = false;
                }
            }

            public void Dfs(int v)
            {
                _fileOut.Write("{0} ", v);
                nov[v] =true;

                for (int u = 0; u < Size; u++)
                {
                    if (array[v, u] != 0 && !nov[u])
                    {
                        Dfs(u);
                    }
                }
            }

            public void Bfs(int v)
            {
                Queue<int> q = new Queue<int>();
                nov[v] = true;
                q.Enqueue(v);
                _fileOut.Write("{0} ", v);

                while (q.Count > 0)
                {
                    int curr = q.Dequeue();
                    for (int u = 0; u < Size; u++)
                    {
                        if (array[curr, u] != 0 && !nov[u])
                        {
                            nov[u] = true;
                            q.Enqueue(u);
                            _fileOut.Write("{0} ", u);
                        }
                    }
                }
            }

            public int[] Dijkstra(int startNode, out int[] p)
            {
                int[] d = new int[Size];
                p = new int[Size];
                bool[] visited = new bool[Size];

                for (int i = 0; i < Size; i++)
                {
                    d[i] = int.MaxValue;
                    visited[i] = false;
                    p[i] = -1;
                }

                d[startNode] = 0;

                for (int count = 0; count < Size - 1; count++)
                {
                    int u = -1;
                    int minDistance = int.MaxValue;

                    for (int vIdx = 0; vIdx < Size; vIdx++)
                    {
                        if (!visited[vIdx] && d[vIdx] <= minDistance)
                        {
                            minDistance = d[vIdx];
                            u = vIdx;
                        }
                    }

                    if (u == -1) break;

                    visited[u] = true;

                    for (int vIdx = 0; vIdx < Size; vIdx++)
                    {
                        if (!visited[vIdx] && array[u, vIdx] != 0 && d[u] != int.MaxValue 
                            && d[u] + array[u, vIdx] < d[vIdx])
                        {
                            d[vIdx] = d[u] + array[u, vIdx];
                            p[vIdx] = u;
                        }
                    }
                }

                return d;
            }

            public void WayDijkstra(int startNode, int endNode, int[] predecessors, ref Stack<int> items)
            {
                items.Clear();
                if (predecessors[endNode] == -1 && startNode != endNode)
                {
                    return;
                }

                int current = endNode;

                while (current != -1)
                {
                    items.Push(current);

                    if (current == startNode)
                    {
                        break;
                    }

                    current = predecessors[current];

                    if (current != -1 && items.Peek() != startNode)
                    {
                        items.Clear();
                        break;
                    }
                }
            }


        }

        private GraphRepresentation _graphRepresentation;
        private StreamWriter _fileOut;

        public Orgraph(string filePath)
        {
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    string line = file.ReadLine();

                    if (line == null)
                    {
                        throw new InvalidDataException("Файл пуст или размер графа не указан");
                    }

                    int n = int.Parse(line.Trim());
                    _graphRepresentation = new GraphRepresentation(n, _fileOut);

                    for (int i = 0; i < n; i++)
                    {
                        line = file.ReadLine();
                        
                        if (line == null)
                        {
                            throw new InvalidDataException("Матрица смежности не соответствует заявленному размеру");
                        }

                        string[] values = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (values.Length != n)
                        {
                            throw new InvalidDataException($"Ожидалось {n} значений, но строка {i} содержит {values.Length}");
                        }

                        for (int j = 0; j < n; j++)
                        {
                            _graphRepresentation[i, j] = int.Parse(values[j]);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                _fileOut.WriteLine($"Ошибка при чтении графа из файла: {exp.Message}");
                _graphRepresentation = new GraphRepresentation(0, _fileOut);
            }
        }

        public void ShowMatrix()
        {
            _fileOut.WriteLine("Матрица смежности:");

            if (_graphRepresentation.Size == 0)
            {
                _fileOut.WriteLine("Граф пуст");
                return;
            }

            for (int i = 0; i < _graphRepresentation.Size; i++)
            {
                for (int j = 0; i < _graphRepresentation.Size; j++)
                {
                    _fileOut.Write("{0, 4}", _graphRepresentation[i, j]);
                }
                _fileOut.WriteLine();
            }
            _fileOut.WriteLine();
        }

        public void Dfs(int startNode)
        {
            if (startNode < 0 || startNode >= _graphRepresentation.Size)
            {
                _fileOut.WriteLine($"DFS: начальная вершина {startNode} вне допустимого диапазона");
                return;
            }

            _fileOut.WriteLine($"DFS от вершины {startNode}:");
            _graphRepresentation.NovSet();
            _graphRepresentation.Dfs(startNode);
            _fileOut.WriteLine("\n");
        }

        public void Bfs(int startNode)
        {
            if (startNode < 0 || startNode >= _graphRepresentation.Size)
            {
                _fileOut.WriteLine($"BFS: начальная вершина {startNode} вне допустимого диапазона");
                return;
            }

            _fileOut.WriteLine($"BFS от вершины {startNode}:");
            _graphRepresentation.NovSet();
            _graphRepresentation.Bfs(startNode);
            _fileOut.WriteLine("\n");
        }

        public void Dijkstra(int startNode)
        {
            if (startNode < 0 || startNode >= _graphRepresentation.Size)
            {
                _fileOut.WriteLine($"Алгоритм Дейкстры: начальная вершина {startNode} вне допустимого диапазона");
                return;
            }

            _fileOut.WriteLine($"Алгоритм Дейкстры от вершины {startNode}:");
            int[] predecessors;
            int[] distances = _graphRepresentation.Dijkstra(startNode, out predecessors);

            for (int i = 0; i < _graphRepresentation.Size; i++)
            {
                _fileOut.Write($"Кратчайший путь от {startNode} до {i}: ");

                if (distances[i] == int.MaxValue)
                {
                    _fileOut.WriteLine("Пути нет");
                }
                else
                {
                    _fileOut.Write($"Длина = {distances[i]}, Путь = ");
                    Stack<int> pathItems = new Stack<int>();
                    _graphRepresentation.WayDijkstra(startNode, i, predecessors, ref pathItems);
                    
                    if (pathItems.Count == 0 && startNode != i)
                    {
                        _fileOut.Write("Пути нет");
                    }
                    else if (pathItems.Count == 0 && startNode == i)
                    {
                        _fileOut.Write(startNode);
                    }
                    else
                    {
                        List<int> outputPath = new List<int>();
                        
                        while (pathItems.Count > 0)
                        {
                            outputPath.Add(pathItems.Pop());
                            //
                            //
                            //
                            //
                        }
                    }
                }
            }
            _fileOut.WriteLine("\n");
        }
    }
}