using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace sharp2sem._21_3
{
    public class AvlTree
    {
        private class Node
        {
            public int Inf;
            public int Height;
            public Node Left;
            public Node Right;

            public Node(int nodeInf)
            {
                Inf = nodeInf;
                Height = 1;
                Left = null;
                Right = null;
            }

            public int BalanceFactor
            {
                get
                {
                    int rh = Right?.Height ?? 0;
                    int lh = Left?.Height ?? 0;
                    return rh - lh;
                }
            }

            public void UpdateHeight()
            {
                int rh = Right?.Height ?? 0;
                int lh = Left?.Height ?? 0;
                Height = (rh > lh ? rh : lh) + 1;
            }

            public int CountNodes()
            {
                int count = 1;
                if (Left != null) count += Left.CountNodes();
                if (Right != null) count += Right.CountNodes();
                return count;
            }

            public static Node RotateRight(Node y)
            {
                if (y == null || y.Left == null) return y;
                Node x = y.Left;
                Node t2 = x.Right;

                x.Right = y;
                y.Left = t2;

                y.UpdateHeight();
                x.UpdateHeight();
                return x;
            }

            public static Node RotateLeft(Node x)
            {
                if (x == null || x.Right == null) return x;
                Node y = x.Right;
                Node t2 = y.Left ?? throw new ArgumentNullException("x");

                y.Left = x;
                x.Right = t2;

                x.UpdateHeight();
                y.UpdateHeight();
                return y;
            }

            public static Node Balance(Node node)
            {
                if (node == null) return null;
                node.UpdateHeight();
                int balance = node.BalanceFactor;

                if (balance > 1)
                {
                    if (node.Right != null && (node.Right.BalanceFactor) < 0)
                    {
                        node.Right = RotateRight(node.Right);
                    }

                    return RotateLeft(node);
                }

                if (balance < -1)
                {
                    if (node.Left != null && (node.Left.BalanceFactor) > 0)
                    {
                        node.Left = RotateLeft(node.Left);
                    }

                    return RotateRight(node);
                }

                return node;
            }

            public static Node Add(Node node, int inf)
            {
                if (node == null) return new Node(inf);

                if (inf < node.Inf)
                    node.Left = Add(node.Left, inf);
                else if (inf > node.Inf)
                    node.Right = Add(node.Right, inf);
                else
                    return node;

                return Balance(node);
            }

            public static void InOrderTraversal(Node node, List<int> result)
            {
                if (node == null) return;
                InOrderTraversal(node.Left, result);
                result.Add(node.Inf);
                InOrderTraversal(node.Right, result);
            }
        }

        private Node _root;

        public AvlTree()
        {
            _root = null;
        }

        public void Add(int nodeInf)
        {
            _root = Node.Add(_root, nodeInf);
        }

        public List<int> GetInOrderTraversal()
        {
            List<int> result = new List<int>();
            Node.InOrderTraversal(_root, result);
            return result;
        }

        public int CountNodes()
        {
            return _root?.CountNodes() ?? 0;
        }

        private static bool CanFormIdeallyBalancedTreeFromNodeCount(int nodeCount)
        {
            if (nodeCount <= 0) return true;
            if (nodeCount == 1) return true;

            int nodesForSubtrees = nodeCount - 1;
            int leftSubtreeSize = nodesForSubtrees / 2;
            int rightSubtreeSize = nodesForSubtrees - leftSubtreeSize;

            return CanFormIdeallyBalancedTreeFromNodeCount(leftSubtreeSize) &&
                   CanFormIdeallyBalancedTreeFromNodeCount(rightSubtreeSize);
        }

        public bool TrySelectNodesForIdealBalance(int maxRemovals, StreamWriter file, out List<int> nodesToKeep,
            out List<int> nodesToDelete)
        {
            nodesToKeep = new List<int>();
            nodesToDelete = new List<int>();
            List<int> originalSortedNodes = GetInOrderTraversal();
            int initialCount = originalSortedNodes.Count;

            if (initialCount == 0)
            {
                file.WriteLine("Дерево пустое, оно уже идеально сбалансировано (0 узлов).");
                return true;
            }

            for (int numRemoved = 0; numRemoved <= maxRemovals; numRemoved++)
            {
                int targetSize = initialCount - numRemoved;
                if (targetSize <= 0) continue;

                if (CanFormIdeallyBalancedTreeFromNodeCount(targetSize))
                {
                    // Если из targetSize узлов можно построить идеал. сбаланс. дерево,
                    // то мы можем выбрать любые targetSize узлов из originalSortedNodes (например, первые, или последние, или средние).
                    // Чтобы минимизировать "сложность" выбора, обычно выбирают центральный блок.
                    // Однако, для задачи "удалить узлы", мы просто должны показать, что это возможно.
                    // Любые targetSize узлов из отсортированного списка можно будет расположить в дереве поиска.
                    // Нам нужно указать, КАКИЕ узлы удалить.
                    // Если мы удаляем `numRemoved` узлов, мы можем удалить, например, `numRemoved` самых больших,
                    // или `numRemoved` самых маленьких, или какие-то из середины.
                    // Задача не уточняет, какие именно узлы удалять, если есть выбор.
                    // "указать удаляемые узлы" - значит, нужно выбрать конкретные.
                    // Простейший вариант - удалить numRemoved самых больших (или самых маленьких) элементов.

                    if (initialCount >= targetSize) // Эта проверка уже есть через targetSize > 0
                    {
                        // Выбираем первые targetSize узлов для сохранения
                        nodesToKeep = originalSortedNodes.Take(targetSize).ToList();
                        // Остальные - на удаление
                        nodesToDelete = originalSortedNodes.Skip(targetSize).ToList();

                        // Убедимся, что количество удаляемых соответствует numRemoved
                        // Это будет так, если мы взяли targetSize = initialCount - numRemoved
                        if (nodesToDelete.Count == numRemoved)
                        {
                            file.WriteLine(
                                $"Можно сделать дерево идеально сбалансированным, оставив {targetSize} узел(узлов) (удалив {numRemoved}).");
                            if (nodesToDelete.Any())
                            {
                                file.WriteLine("Пример удаляемых узлов (самые большие): " +
                                               string.Join(", ", nodesToDelete));
                            }
                            else
                            {
                                file.WriteLine(
                                    "Удаление узлов не требуется, текущее количество узлов позволяет построить идеально сбалансированное дерево.");
                            }

                            return true;
                        }
                    }
                }
            }

            file.WriteLine(
                $"Невозможно сделать дерево идеально сбалансированным, удалив не более {maxRemovals} узел(узлов).");
            return false;
        }
    }
}