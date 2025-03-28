using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace sharp2sem._18_19
{
    public static class Solution1819Pr
    {
        public static void Execute()
        {
            string outputFilePath = @"C:\Users\petroved\source\repos\sharp2sem\sharp2sem\18_19\output.txt";
            using (StreamWriter outF = new StreamWriter(outputFilePath, false))
            {
                string binaryFilePath = @"C:\Users\petroved\source\repos\sharp2sem\sharp2sem\18_19\data.dat";
                BinaryFormatter formatter = new BinaryFormatter();
                List<Client> clients = new List<Client>();
                using (FileStream fs = new FileStream(binaryFilePath, FileMode.OpenOrCreate))
                {
                    if (fs.Length > 0)
                    {
                        clients = (List<Client>)formatter.Deserialize(fs);
                    }
                }
                
                outF.WriteLine("Данные о клиентах из бинарного файла:");
                clients.Sort();

                foreach (Client client in clients)
                {
                    outF.WriteLine(client);
                    outF.WriteLine();
                }

                clients.Add(new LegalEntity(1, "BibusHolding"));
                clients.Add(new Individual(0, "Платон"));
                
                outF.WriteLine();
                outF.WriteLine("Новые данные:");
                foreach (Client client in clients)
                {
                    outF.WriteLine(client);
                    outF.WriteLine();
                }
                
                using (FileStream fs = new FileStream(binaryFilePath, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, clients);
                }
            }
        }
    }
}