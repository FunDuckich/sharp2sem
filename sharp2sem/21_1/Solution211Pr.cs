using System.Collections.Generic;
using System.IO;

namespace sharp2sem._21_1
{
    public static class Solution211Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\21_1\input.txt";
            string outputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\21_1\output.txt";
            List<int> inputNums = new List<int>();
            using (StreamReader inF = new StreamReader(inputFilePath))
            {
                string line;
                while ((line = inF.ReadLine()) != null)
                {
                    string[] nums = line.Split(' ');
                    foreach (string num in nums)
                    {
                        inputNums.Add(int.Parse(num));
                    }
                }
            }

            BinaryTree btree = new BinaryTree();
            foreach (int num in inputNums)
            {
                btree.Add(num);
            }

            int minValue = int.MaxValue;
            int minCount = 0;
            btree.FindMinValues(ref minValue, ref minCount);

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

                outF.WriteLine("Минимальное значение {0} встречается в дереве {1} р.", minValue, minCount);

            }
        }
    }
}
