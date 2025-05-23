using System.IO;

namespace sharp2sem._22_1
{
    public class Orgraph
    {
        private class GraphRepresentation
        {
            private int[,] _array;
            private StreamWriter _fileOut;

            public int Size { get; }

            public int this[int i, int j]
            {
                get => _array[i, j];
            }

            public GraphRepresentation(int n, int[,] initialArray, StreamWriter fileOut)
            {
                Size = n;
                _array = initialArray;
                _fileOut = fileOut;
            }

            public void RemoveArc(int source, int destination)
            {
                if (source >= 0 && source < Size && destination >= 0 && destination < Size)
                {
                    if (_array[source, destination] == 0)
                    {
                        _fileOut.WriteLine($"Дуги {source} -> {destination} и так нет (или ее вес 0).");
                    }
                    else
                    {
                        _array[source, destination] = 0;
                        _fileOut.WriteLine($"Дуга {source} -> {destination} была удалена.");
                    }
                }
                else
                {
                    _fileOut.WriteLine("Ошибка: некорректные индексы вершин при попытке удаления дуги.");
                }
            }
        }

        private GraphRepresentation _graphRepresentation;
        private StreamWriter _fileOut;

        public Orgraph(int[,] adjacencyMatrix, StreamWriter fileOut)
        {
            _fileOut = fileOut;
            if (adjacencyMatrix == null)
            {
                if (_fileOut != null)
                {
                    _fileOut.WriteLine("Ошибка: передана нулевая матрица смежности.");
                }

                _graphRepresentation = new GraphRepresentation(0, new int[0, 0], _fileOut);
                return;
            }

            int n = adjacencyMatrix.GetLength(0);
            if (n != adjacencyMatrix.GetLength(1))
            {
                if (_fileOut != null)
                {
                    _fileOut.WriteLine("Ошибка: матрица смежности не является квадратной.");
                }

                _graphRepresentation = new GraphRepresentation(0, new int[0, 0], _fileOut);
                return;
            }

            _graphRepresentation = new GraphRepresentation(n, adjacencyMatrix, _fileOut);
        }

        public void ShowMatrix()
        {
            _fileOut.WriteLine("Матрица смежности:");

            if (_graphRepresentation.Size == 0)
            {
                _fileOut.WriteLine("Граф пуст или не был корректно загружен.");
                return;
            }

            for (int i = 0; i < _graphRepresentation.Size; i++)
            {
                for (int j = 0; j < _graphRepresentation.Size; j++)
                {
                    _fileOut.Write("{0,4}", _graphRepresentation[i, j]);
                }

                _fileOut.WriteLine();
            }

            _fileOut.WriteLine();
        }

        public void RemoveArc(int source, int destination)
        {
            _graphRepresentation.RemoveArc(source, destination);
        }
    }
}