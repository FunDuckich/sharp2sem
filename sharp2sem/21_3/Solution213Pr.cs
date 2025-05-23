using System;
using System.Collections.Generic;
using System.IO;

namespace sharp2sem._21_3
{
    public static class Solution213Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\21_3\input.txt";
            string outputFilePath = @"C:\Users\petro\RiderProjects\sharp2sem\sharp2sem\21_3\output.txt";

            List<int> numbersForTree = new List<int>();
            int maxNodesToRemove = 0;

            try
            {
                string[] allLines = File.ReadAllLines(inputFilePath);
                if (allLines.Length == 0)
                {
                    using (StreamWriter sw = new StreamWriter(outputFilePath))
                    {
                        sw.WriteLine("Входной файл пуст.");
                    }

                    return;
                }

                if (allLines.Length > 0 && !string.IsNullOrWhiteSpace(allLines[0]))
                {
                    string[] numStrings = allLines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in numStrings)
                    {
                        if (int.TryParse(s, out int num))
                        {
                            numbersForTree.Add(num);
                        }
                    }
                }

                if (allLines.Length > 1)
                {
                    string lastNonEmptyLine = null;
                    for (int i = allLines.Length - 1; i >= 0; i--)
                    {
                        if (!string.IsNullOrWhiteSpace(allLines[i]))
                        {
                            lastNonEmptyLine = allLines[i];
                            break;
                        }
                    }

                    if (lastNonEmptyLine != null && !int.TryParse(lastNonEmptyLine, out maxNodesToRemove))
                    {
                        maxNodesToRemove = 0;
                    }
                }
                else if (allLines.Length == 1 && numbersForTree.Count > 1)
                {
                    maxNodesToRemove = 0;
                }

                AvlTree avlTree = new AvlTree();
                foreach (int num in numbersForTree)
                {
                    avlTree.Add(num);
                }

                using (StreamWriter outF = new StreamWriter(outputFilePath, false))
                {
                    outF.WriteLine("Числа для построения дерева:");
                    outF.WriteLine(string.Join(" ", numbersForTree));
                    outF.WriteLine($"Максимальное количество узлов для удаления (N): {maxNodesToRemove}");
                    outF.WriteLine($"Исходное количество узлов в АВЛ-дереве: {avlTree.CountNodes()}");


                    avlTree.TrySelectNodesForIdealBalance(maxNodesToRemove, outF, out List<int> nodesToKeep,
                        out List<int> nodesToDelete);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Ошибка: Входной файл не найден: {inputFilePath}");

                File.WriteAllText(outputFilePath, $"Ошибка: Входной файл не найден: {inputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");

                File.WriteAllText(outputFilePath, $"Произошла ошибка: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}