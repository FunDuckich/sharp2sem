using System.Text.RegularExpressions;
using System.IO;

namespace sharp2sem._20
{
    public static class Solution20Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petroved\source\repos\sharp2sem\sharp2sem\20\input.txt";
            string outputFilePath = @"C:\Users\petroved\source\repos\sharp2sem\sharp2sem\20\output.txt";
            Listik list = new Listik();
            using (StreamReader inF = new StreamReader(inputFilePath))
            {
                Regex regForNums = new Regex(@"-?\d+");
                string line;
                while ((line = inF.ReadLine()) != null)
                {
                    MatchCollection numsMatches = regForNums.Matches(line);
                    foreach (Match num in numsMatches)
                    {
                        list.AddToBegin(int.Parse(num.Value));
                    }
                }
            }

            using (StreamWriter outF =
                   new StreamWriter(outputFilePath, false))
            {
                outF.WriteLine(list);
                list.DoubleOdds();
                outF.WriteLine(list);
            }
        }
    }
}