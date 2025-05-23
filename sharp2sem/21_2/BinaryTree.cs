using System;
using System.IO;

namespace sharp2sem._21_2
{
    public class BinaryTree 
    {
        private class Node
        {
            public int Inf;
            public Node Left;
            public Node Right;

            public Node(int nodeInf)
            {
                Inf = nodeInf;
                Left = null;
                Right = null;
            }

            public static void Add(ref Node r, int nodeInf)
            {
                if (r == null)
                {
                    r = new Node(nodeInf);
                }
                else
                {
                    if (r.Inf > nodeInf)
                    {
                        Add(ref r.Left, nodeInf);
                    }
                    else
                    {
                        Add(ref r.Right, nodeInf);
                    }
                }
            }

            public static void Preorder(Node r, StreamWriter file)
            {
                if (r != null)
                {
                    file.Write("{0} ", r.Inf);
                    Preorder(r.Left, file);
                    Preorder(r.Right, file);
                }
            }

            public static void Inorder(Node r, StreamWriter file)
            {
                if (r != null)
                {
                    Inorder(r.Left, file);
                    file.Write("{0} ", r.Inf);
                    Inorder(r.Right, file);
                }
            }

            public static void Postorder(Node r, StreamWriter file)
            {
                if (r != null)
                {
                    Postorder(r.Left, file);
                    Postorder(r.Right, file);
                    file.Write("{0} ", r.Inf);
                }
            }

            public static void Search(Node r, int key, out Node item)
            {
                if (r == null)
                {
                    item = null;
                }
                else
                {
                    if (r.Inf == key)
                    {
                        item = r;
                    }
                    else
                    {
                        if (r.Inf > key)
                        {
                            Search(r.Left, key, out item);
                        }
                        else
                        {
                            Search(r.Right, key, out item);
                        }
                    }
                }
            }

            private static void Del(Node t, ref Node tr)
            {
                if (tr.Right != null)
                {
                    Del(t, ref tr.Right);
                }
                else
                {
                    t.Inf = tr.Inf;
                    tr = tr.Left;
                }
            }

            public static void Delete(ref Node t, int key)
            {
                if (t == null)
                {
                    throw new Exception("Данное значение в дереве отсутствует");
                }
                else
                {
                    if (t.Inf > key)
                    {
                        Delete(ref t.Left, key);
                    }
                    else
                    {
                        if (t.Inf < key)
                        {
                            Delete(ref t.Right, key);
                        }
                        else
                        {
                            if (t.Left == null)
                            {
                                t = t.Right;
                            }
                            else
                            {
                                if (t.Right == null)
                                {
                                    t = t.Left;
                                }
                                else
                                {
                                    Del(t, ref t.Left);
                                }
                            }
                        }
                    }
                }
            }

            public static int CountNodesAtLevel(Node node, int level, int currentLevel = 0)
            {
                if (node == null)
                    return 0;

                if (currentLevel == level)
                    return 1;

                return CountNodesAtLevel(node.Left, level, currentLevel + 1) +
                       CountNodesAtLevel(node.Right, level, currentLevel + 1);
            }

        }

        private Node _tree;
        
        public int Inf
        {
            set => _tree.Inf = value;
            get => _tree.Inf;
        }

        public BinaryTree()
        {
            _tree = null;
        }

        private BinaryTree(Node r)
        {
            _tree = r;
        }

        public void Add(int nodeInf)
        {
            Node.Add(ref _tree, nodeInf);
        }

        public void Preorder(StreamWriter file)
        {
            Node.Preorder(_tree, file);
        }

        public void Inorder(StreamWriter file)
        {
            Node.Inorder(_tree, file);
        }

        public void Postorder(StreamWriter file)
        {
            Node.Postorder(_tree, file);
        }

        public BinaryTree Search(int key)
        {
            Node r;
            Node.Search(_tree, key, out r);
            BinaryTree t = new BinaryTree(r);
            return t;
        }

        public void Delete(int key)
        {
            Node.Delete(ref _tree, key);
        }

        public int CountNodesAtLevel(int k)
        {
            return Node.CountNodesAtLevel(_tree, k);
        }

        
    }
}