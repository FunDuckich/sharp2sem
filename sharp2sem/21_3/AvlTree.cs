using System;
using System.Collections.Generic;

namespace sharp2sem._21_3
{
    public class AvlTree
    {
        private class Node
        {
            public int Key;
            public int Height;
            public int Count;
            public Node Left, Right;

            public Node(int key)
            {
                Key = key;
                Height = 1;
                Count = 1;
            }

            public void Update()
            {
                int lh = Left != null ? Left.Height : 0;
                int rh = Right != null ? Right.Height : 0;
                Height = Math.Max(lh, rh) + 1;

                int lc = Left != null ? Left.Count : 0;
                int rc = Right != null ? Right.Count : 0;
                Count = lc + rc + 1;
            }

            private static int BalanceFactor(Node n)
            {
                return (n.Right != null ? n.Right.Height : 0) - (n.Left != null ? n.Left.Height : 0);
            }

            private static Node RotateLeft(Node x)
            {
                Node y = x.Right;
                Node t = y.Left;
                y.Left = x;
                x.Right = t;
                x.Update();
                y.Update();
                return y;
            }

            private static Node RotateRight(Node y)
            {
                Node x = y.Left;
                Node t = x.Right;
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
                    if (BalanceFactor(n.Right) < 0)
                        n.Right = RotateRight(n.Right);
                    return RotateLeft(n);
                }

                if (BalanceFactor(n) < -1)
                {
                    if (BalanceFactor(n.Left) > 0)
                        n.Left = RotateLeft(n.Left);
                    return RotateRight(n);
                }

                return n;
            }

            public static Node Add(Node n, int key)
            {
                if (n == null) return new Node(key);
                if (key < n.Key) n.Left = Add(n.Left, key);
                else n.Right = Add(n.Right, key);
                return Balance(n);
            }
        }

        private Node _root;

        public void Add(int key)
        {
            _root = Node.Add(_root, key);
        }

        public int GetCount()
        {
            return _root != null ? _root.Count : 0;
        }

        public bool TryIdealBalance(int maxRemovable, out List<int> removed)
        {
            removed = new List<int>();
            bool tooMany = false;
            CountNeededRemovals(_root, maxRemovable, ref tooMany);
            if (tooMany) return false;
            BalanceWeight(ref _root, removed);
            return true;
        }

        private class RemovalsResult
        {
            public int SubtreeSize;
            public int Removals;

            public RemovalsResult(int size, int removals)
            {
                SubtreeSize = size;
                Removals = removals;
            }
        }

        private RemovalsResult CountNeededRemovals(Node node, int maxRem, ref bool tooMany)
        {
            if (node == null || tooMany)
                return new RemovalsResult(0, 0);

            RemovalsResult left = CountNeededRemovals(node.Left, maxRem, ref tooMany);
            RemovalsResult right = CountNeededRemovals(node.Right, maxRem, ref tooMany);

            int diff = Math.Abs(left.SubtreeSize - right.SubtreeSize);
            int remHere = diff > 1 ? diff - 1 : 0;
            int totalRem = left.Removals + right.Removals + remHere;

            if (totalRem > maxRem)
            {
                tooMany = true;
            }

            int finalL = left.SubtreeSize;
            int finalR = right.SubtreeSize;

            if (diff > 1)
            {
                if (left.SubtreeSize > right.SubtreeSize)
                    finalL = right.SubtreeSize + 1;
                else
                    finalR = left.SubtreeSize + 1;
            }

            return new RemovalsResult(finalL + finalR + 1, totalRem);
        }

        private int BalanceWeight(ref Node node, List<int> removed)
        {
            if (node == null) return 0;

            int leftSize = BalanceWeight(ref node.Left, removed);
            int rightSize = BalanceWeight(ref node.Right, removed);
            int diff = Math.Abs(leftSize - rightSize);

            if (diff > 1)
            {
                int toRemove = diff - 1;
                if (leftSize > rightSize)
                    RemoveKLeaves(ref node.Left, ref toRemove, removed);
                else
                    RemoveKLeaves(ref node.Right, ref toRemove, removed);

                leftSize = GetSubtreeCount(node.Left);
                rightSize = GetSubtreeCount(node.Right);
            }

            node.Update();
            return leftSize + rightSize + 1;
        }

        private void RemoveKLeaves(ref Node node, ref int k, List<int> removed)
        {
            if (node == null || k <= 0) return;

            if (node.Left == null && node.Right == null)
            {
                removed.Add(node.Key);
                node = null;
                k--;
                return;
            }

            int lh = node.Left != null ? node.Left.Height : 0;
            int rh = node.Right != null ? node.Right.Height : 0;

            if (lh >= rh)
            {
                RemoveKLeaves(ref node.Left, ref k, removed);
                if (k > 0) RemoveKLeaves(ref node.Right, ref k, removed);
            }
            else
            {
                RemoveKLeaves(ref node.Right, ref k, removed);
                if (k > 0) RemoveKLeaves(ref node.Left, ref k, removed);
            }
        }

        private int GetSubtreeCount(Node node)
        {
            if (node == null) return 0;
            return (node.Left != null ? node.Left.Count : 0) +
                   (node.Right != null ? node.Right.Count : 0) + 1;
        }
    }
}
