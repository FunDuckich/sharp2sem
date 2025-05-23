using System.IO;
using System.Collections.Generic;

namespace sharp2sem._22_2
{
    public class Orgraph
    {
        private class GraphRepresentation
        {
            private int[,] _array;
            private List<int> _hamiltonianCycle; 

            public int Size { get; }

            public GraphRepresentation(int n, int[,] initialArray)
            {
                Size = n;
                _array = initialArray;
                _hamiltonianCycle = null;
            }

            private bool HamiltonianCycleUtil(List<int> path, bool[] visited, int currentVertex, int startVertex)
            {
                path.Add(currentVertex);
                visited[currentVertex] = true;

                if (path.Count == Size)
                {
                    if (_array[currentVertex, startVertex] != 0)
                    {
                        path.Add(startVertex); 
                        _hamiltonianCycle = new List<int>(path);
                        return true;
                    }

                    path.RemoveAt(path.Count - 1);
                    visited[currentVertex] = false;
                    return false;
                }

                for (int v = 0; v < Size; v++)
                {
                    if (_array[currentVertex, v] != 0 && !visited[v])
                    {
                        if (HamiltonianCycleUtil(path, visited, v, startVertex))
                            return true;
                    }
                }
                
                path.RemoveAt(path.Count - 1);
                visited[currentVertex] = false;
                return false;
            }

            public List<int> FindHamiltonianCycle(int startVertexA)
            {
                if (Size == 0) return null;
                if (startVertexA < 0 || startVertexA >= Size) return null;

                _hamiltonianCycle = null;
                List<int> path = new List<int>();
                bool[] visited = new bool[Size];
                
                for(int i=0; i<Size; i++) visited[i] = false;

                HamiltonianCycleUtil(path, visited, startVertexA, startVertexA);
                return _hamiltonianCycle;
            }
        }

        private GraphRepresentation _graphRepresentation;
        private StreamWriter _fileOut;

        public Orgraph(int[,] adjacencyMatrix, StreamWriter fileOut)
        {
            _fileOut = fileOut;
            if (adjacencyMatrix == null)
            {
                _graphRepresentation = new GraphRepresentation(0, new int[0, 0]);
                return;
            }

            int n = adjacencyMatrix.GetLength(0);
            if (n != adjacencyMatrix.GetLength(1))
            {
                _graphRepresentation = new GraphRepresentation(0, new int[0, 0]);
                return;
            }
            _graphRepresentation = new GraphRepresentation(n, adjacencyMatrix);
        }
        
        public void FindAndPrintHamiltonianCycle(int startVertexA)
        {
            if (_graphRepresentation.Size == 0)
            {
                _fileOut.WriteLine("Граф пуст.");
                return;
            }
            
            if (startVertexA < 0 || startVertexA >= _graphRepresentation.Size)
            {
                 _fileOut.WriteLine($"Начальная вершина {startVertexA} вне диапазона графа [0..{_graphRepresentation.Size-1}].");
                 return;
            }

            List<int> cycle = _graphRepresentation.FindHamiltonianCycle(startVertexA);

            if (cycle != null)
            {
                _fileOut.WriteLine($"Найден Гамильтонов цикл, начинающийся с вершины {startVertexA}:");
                _fileOut.WriteLine(string.Join(" -> ", cycle));
            }
            else
            {
                _fileOut.WriteLine($"Гамильтонов цикл, начинающийся с вершины {startVertexA}, не найден.");
            }
        }
    }
}