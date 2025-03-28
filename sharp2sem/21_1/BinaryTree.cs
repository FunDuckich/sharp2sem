using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp2sem._21_1
{
    public class BinaryTree //класс, реализующий АТД «дерево бинарного поиска»
    {
        private class Node
        {
            public int inf;
            public Node left; 
            public Node rigth; 

            public Node(int nodeInf)
            {
                inf = nodeInf;
                left = null;
                rigth = null;
            }

            public static void Add(ref Node r, int nodeInf)
            {
                if (r == null)
                {
                    r = new Node(nodeInf);
                }
                else
                {
                if (r.inf >= nodeInf)
                    {
                        Add(ref r.left, nodeInf);
                    }
                    else
                    {
                        Add(ref r.rigth, nodeInf);
                    }
                }
            }
            public static void Preorder(Node r, StreamWriter file) 
            {
                if (r != null)
                {
                    file.Write("{0} ", r.inf);
                    Preorder(r.left, file);
                    Preorder(r.rigth, file);
                }
            }
            public static void Inorder(Node r, StreamWriter file)
            {
                if (r != null)
                {
                    Inorder(r.left, file);
                    file.Write("{0} ", r.inf);
                    Inorder(r.rigth, file);
                }
            }
            public static void Postorder(Node r, StreamWriter file)
            {
                if (r != null)
                {
                    Postorder(r.left, file);
                    Postorder(r.rigth, file);
                    file.Write("{0} ", r.inf);
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
                    if (r.inf == key)
                    {
                        item = r;
                    }
                    else
                    {
                        if (r.inf > key)
                        {
                            Search(r.left, key, out item);
                        }
                        else
                        {
                            Search(r.rigth, key, out item);
                        }
                    }
                }
            }

            private static void Del(Node t, ref Node tr)
            {
                if (tr.rigth != null)
                {
                    Del(t, ref tr.rigth);
                }
                else
                {
                    t.inf = tr.inf;
                    tr = tr.left;
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
                    if (t.inf > key)
                    {
                        Delete(ref t.left, key);
                    }
                    else
                    {
                        if (t.inf < key)
                        {
                            Delete(ref t.rigth, key);
                        }
                        else
                        {
                            if (t.left == null)
                            {
                                t = t.rigth;
                            }
                            else
                            {
                                if (t.rigth == null)
                                    {
                                        t = t.left;
                                    }
                                    else
                                    {
                                        Del(t, ref t.left);
                                    }
                            }
                        }
                    }
                }
            }
        } 

        Node tree; 

        public int Inf
        {
            set { tree.inf = value; }
            get { return tree.inf; }
        }
        public BinaryTree() 
        {
            tree = null;
        }
        private BinaryTree(Node r) 
        {
            tree = r;
        }
        public void Add(int nodeInf) 
        {
            Node.Add(ref tree, nodeInf);
        }

        public void Preorder(StreamWriter file)
        {
            Node.Preorder(tree, file);
        }
        public void Inorder(StreamWriter file)
        {
            Node.Inorder(tree, file);
        }
        public void Postorder(StreamWriter file)
        {
            Node.Postorder(tree, file);
        }

        public BinaryTree Search(int key)
        {
            Node r;
            Node.Search(tree, key, out r);
            BinaryTree t = new BinaryTree(r);
            return t;
        }

        public void Delete(int key)
        {
            Node.Delete(ref tree, key);
        }

        public void FindMins(out int minValue, out int minCount)
        {
            minValue = 0;
            minCount = 0;
            if (tree != null)
            {
                Node current = tree;
                minValue = current.inf;
                minCount += 1;

                while (current.left != null)
                {
                    minValue = current.left.inf;
                    if (current.inf == minValue)
                    {
                        minCount += 1;
                    }
                    current = current.left;
                }
            }
        }
    }
}
