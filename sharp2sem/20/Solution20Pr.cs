using System;
using System.Text.RegularExpressions;
using System.IO;

namespace sharp2sem._20
{
    public class Solution20Pr
    {
        public static void Execute()
        {
            Listik list = new Listik();
            using (StreamReader inpF =
                   new StreamReader(@"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\20\input.txt"))
            {
                Regex regForNums = new Regex(@"-?\d+");
                string line;
                while ((line = inpF.ReadLine()) != null)
                {
                    MatchCollection numsMatches = regForNums.Matches(line);
                    foreach (Match num in numsMatches)
                    {
                        list.AddToBegin(int.Parse(num.Value));
                    }
                }
            }

            using (StreamWriter outpF =
                   new StreamWriter(@"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\20\output.txt", false))
            {
                outpF.WriteLine(list.ToString());
                ListikNode currentItem = list.Head;
                while (true)
                {
                    if (currentItem == null)
                    {
                        break;
                    }

                    if (Math.Abs(currentItem.Value) % 2 == 0)
                    {
                        list.Insert(currentItem, currentItem.Value);
                        currentItem = currentItem.Next.Next;
                    }
                    else
                    {
                        currentItem = currentItem.Next;
                    }
                }

                outpF.WriteLine(list.ToString());
            }
        }
    }
}