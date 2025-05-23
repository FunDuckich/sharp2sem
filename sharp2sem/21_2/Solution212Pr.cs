using System.Collections.Generic;
using System.IO;

namespace sharp2sem._21_2
{
    public static class Solution212Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\21_2\input.txt";
            string outputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\21_2\output.txt";
            List<int> inputNums = new List<int>();
            int k = 0;
            using (StreamReader inF = new StreamReader(inputFilePath))
            {
                string line;
                while ((line = inF.ReadLine()) != null)
                {
                    if (line.Length > 1)
                    {
                        string[] nums = line.Split(' ');
                        foreach (string num in nums)
                        {
                            inputNums.Add(int.Parse(num));
                        }
                    }
                    else
                    {
                        k = int.Parse(line);
                    }
                }
            }

            BinaryTree btree = new BinaryTree();
            foreach (int num in inputNums)
            {
                btree.Add(num);
            }

            int countedNodes = btree.CountNodesAtLevel(k);

            using (StreamWriter outF =
                   new StreamWriter(outputFilePath, false))
            {
                outF.Write("Числа в файле: ");
                foreach (int num in inputNums)
                {
                    outF.Write("{0} ", num);
                }
                outF.WriteLine();

                outF.Write("Числа в дереве: ");
                btree.Preorder(outF);
                outF.Write("(preorder), ");
                btree.Inorder(outF);
                outF.Write("(inorder), ");
                btree.Postorder(outF);
                outF.Write("(postorder)");
                outF.WriteLine();

                outF.WriteLine("На уровне {0} узлов - {1}", k, countedNodes);

            }
        }
    }
}
