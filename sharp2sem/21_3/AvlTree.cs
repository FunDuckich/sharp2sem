using System;
using System.Collections.Generic;

namespace sharp2sem._21_3
{
    public class AvlTree
    {
        private class Node
        {
            public int Key, Height, Count;
            public Node Left, Right;

            public Node(int key)
            {
                Key = key;
                Height = 1;
                Count = 1;
            }

            private void Update()
            {
                int lh = Left?.Height ?? 0, rh = Right?.Height ?? 0;
                Height = Math.Max(lh, rh) + 1;
                int lc = Left?.Count ?? 0, rc = Right?.Count ?? 0;
                Count = lc + rc + 1;
            }

            private static int BalanceFactor(Node n)
            {
                return (n?.Right?.Height ?? 0) - (n?.Left?.Height ?? 0);
            }

            private static Node RotateLeft(Node x)
            {
                Node y = x.Right, t = y.Left;
                y.Left = x;
                x.Right = t;
                x.Update();
                y.Update();
                return y;
            }

            private static Node RotateRight(Node y)
            {
                Node x = y.Left, t = x.Right;
                x.Right = y;
                y.Left = t;
                y.Update();
                x.Update();
                return x;
            }

            private static Node Balance(Node n)
            {
                if (n == null) return null;
                n.Update();
                if (BalanceFactor(n) > 1)
                {
                    if (BalanceFactor(n.Right) < 0) n.Right = RotateRight(n.Right);
                    return RotateLeft(n);
                }

                if (BalanceFactor(n) < -1)
                {
                    if (BalanceFactor(n.Left) > 0) n.Left = RotateLeft(n.Left);
                    return RotateRight(n);
                }

                return n;
            }

            public static Node Add(Node n, int key)
            {
                if (n == null) return new Node(key);
                if (key < n.Key) n.Left = Add(n.Left, key);
                else if (key > n.Key) n.Right = Add(n.Right, key);
                return Balance(n);
            }

            private static Node FindMin(Node n)
            {
                while (n.Left != null) n = n.Left;
                return n;
            }

            public static Node Delete(Node n, int key)
            {
                if (n == null) return null;
                if (key < n.Key) n.Left = Delete(n.Left, key);
                else if (key > n.Key) n.Right = Delete(n.Right, key);
                else
                {
                    if (n.Left == null) return n.Right;
                    if (n.Right == null) return n.Left;
                    Node m = FindMin(n.Right);
                    n.Key = m.Key;
                    n.Right = Delete(n.Right, m.Key);
                }

                return Balance(n);
            }
        }

        private Node _root;

        public void Add(int key)
        {
            _root = Node.Add(_root, key);
        }

        public bool TryIdealBalance(int n, out List<int> removed)
        {
            //
        }

        public int GetCount()
        {
            return _root.Count;
        }
    }
}