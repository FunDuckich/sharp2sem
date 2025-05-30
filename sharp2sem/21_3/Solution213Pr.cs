using System;
using System.Collections.Generic;
using System.IO;

namespace sharp2sem._21_3
{
    public static class Solution213Pr
    {
        public static void Execute()
        {
            string inputFilePath = @"C:\Users\Анна\Source\Repos\sharp2sem\sharp2sem\21_3\input.txt";
            string outputFilePath = @"C:\Users\Анна\Source\Repos\sharp2sem\sharp2sem\21_3\output.txt";

            List<int> numbersForTree = new List<int>();
            int maxNodesToRemove = 0;

            string[] allLines = File.ReadAllLines(inputFilePath);
            if (allLines.Length == 0)
            {
                using (StreamWriter sw = new StreamWriter(outputFilePath))
                {
                    sw.WriteLine("Входной файл пуст.");
                }

                return;
            }

            for (int i = 0; i < allLines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(allLines[i]))
                {
                    string[] parts = allLines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == allLines.Length - 1)
                    {
                        if (parts.Length == 1 && int.TryParse(parts[0], out int n))
                        {
                            maxNodesToRemove = n;
                        }
                    }
                    else
                    {
                        foreach (string s in parts)
                        {
                            if (int.TryParse(s, out int num))
                            {
                                numbersForTree.Add(num);
                            }
                        }
                    }
                }
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
                outF.WriteLine($"Исходное количество узлов в АВЛ-дереве: {avlTree.GetCount()}");

                if (avlTree.TryIdealBalance(maxNodesToRemove, out List<int> removed))
                {
                    outF.WriteLine("Можно удалить не более N узлов для идеального баланса.");
                    outF.WriteLine("Удаляемые узлы:");
                    outF.WriteLine(string.Join(" ", removed));
                }
                else
                {
                    outF.WriteLine("Невозможно удалить не более N узлов для идеального баланса.");
                }
            }
        }
    }
}