using System;
using System.IO;

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
                    int rh = (this.Right != null) ? this.Right.Height : 0;
                    int lh = (this.Left != null) ? this.Left.Height : 0;
                    return rh - lh;
                }
            }

            //пересчитывает высоту узла
            public void NewHeight()
            {
                int rh = (this.Right != null) ? this.Right.Height : 0;
                int lh = (this.Left != null) ? this.Left.Height : 0;
                this.Height = ((rh > lh) ? rh : lh) + 1;
            }

            //правый поворот
            public static void RotationRigth(ref Node t)
            {
                Node x = t.Left;
                t.Left = x.Right;
                x.Right = t;
                t.NewHeight();
                x.NewHeight();
                t = x;
            }

            //левый поворот
            public static void RotationLeft(ref Node t)
            {
                Node x = t.Right;
                t.Right = x.Left;
                x.Left = t;
                t.NewHeight();
                x.NewHeight();
                t = x;
            }

            //балансировка
            public static void Rotation(ref Node t)
            {
                t.NewHeight();
                if (t.BalanceFactor == 2)
                {
                    if (t.Right.BalanceFactor < 0)
                    {
                        RotationRigth(ref t.Right);
                    }

                    RotationLeft(ref t);
                }

                if (t.BalanceFactor == -2)
                {
                    if (t.Left.BalanceFactor > 0)
                    {
                        RotationLeft(ref t.Left);
                    }
                    RotationRigth(ref t);
                }
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

                Rotation(ref r);
            }

            public static void Preorder(Node r, StreamWriter file) 
            {
                if (r != null)
                {
                    file.Write("({0} {1}) ", r.Inf, r.Height);
                    Preorder(r.Left, file);
                    Preorder(r.Right, file);
                }
            }

            public static void Inorder(Node r, StreamWriter file) 
            {
                if (r != null)
                {
                    Inorder(r.Left, file);
                    file.Write("({0} {1}) ", r.Inf, r.Height);
                    Inorder(r.Right, file);
                }
            }

            public static void Postorder(Node r, StreamWriter file)
            {
                if (r != null)
                {
                    Postorder(r.Left, file);
                    Postorder(r.Right, file);
                    file.Write("({0} {1}) ", r.Inf, r.Height);
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
                    Console.WriteLine("Данное значение в дереве отсутствует");
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

                    Rotation(ref t);
                }
            }
        } 

        private Node _tree; 
        
        public int Inf => _tree.Inf;

        public AvlTree() 
        {
            _tree = null;
        }

        private AvlTree(Node r) 
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

        public AvlTree Search(int key)
        {
            Node.Search(_tree, key, out Node r);
            AvlTree t = new AvlTree(r);
            return t;
        }

        public void Delete(int key)
        {
            Node.Delete(ref _tree, key);
        }
    }
}