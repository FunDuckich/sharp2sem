using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sharp2sem._21_1
{
    class Solution21_1Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petroved\source\repos\sharp2sem\sharp2sem\21_1\input.txt";
            string outputFilePath = @"C:\Users\petroved\source\repos\sharp2sem\sharp2sem\21_1\output.txt";
            List<int> inputNums = new List<int>();
            using (StreamReader inF = new StreamReader(inputFilePath))
            {
                Regex regForNums = new Regex(@"-?\d+");
                string line;
                while ((line = inF.ReadLine()) != null)
                {
                    MatchCollection numsMatches = regForNums.Matches(line);
                    foreach (Match num in numsMatches)
                    {
                        inputNums.Add(int.Parse(num.Value));
                    }
                }
            }

            BinaryTree btree = new BinaryTree();
            foreach (int num in inputNums)
            {
                btree.Add(num);
            }

            btree.FindMins(out int minValue, out int minCount);

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
