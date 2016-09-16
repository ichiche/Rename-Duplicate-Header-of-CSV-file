using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace RenameDuplicateMember
{
    class Program
    {
        static void CheckOccurrence(string fileName, string[] csvContent)
        {
            string[] firstLineArr = csvContent[0].Split(',');

            var groups = firstLineArr.GroupBy(item => item);
            foreach (var group in groups)
            {
                Console.WriteLine(string.Format("{0} occurences of {1}", group.Count(), group.Key));
            }
        }

        static void ReplaceDuplicateMember(string fileName, string[] csvContent)
        {
            string[] firstColumnArray = csvContent[0].Split(',');
            Dictionary<string, int> duplicatedColumn = new Dictionary<string, int>();

            //Find duplicate column
            for (int i = 0; i < firstColumnArray.Length; i++)
            {
                if (!(duplicatedColumn.ContainsKey(firstColumnArray[i])))
                {
                    for (int j = i + 1; j < firstColumnArray.Length; j++)
                    {
                        if (firstColumnArray[i].Equals(firstColumnArray[j]) && !(duplicatedColumn.ContainsKey(firstColumnArray[i])))
                        {
                            duplicatedColumn.Add(firstColumnArray[i], 1);
                        }
                    }                  
                }
            }

            //Rename the duplicate column in Final Array
            for (int index = 0; index < duplicatedColumn.Count; index++)
            {
                int count = 1;
                for (int j = 0; j < firstColumnArray.Length; j++)
                {
                    if (duplicatedColumn.ElementAt(index).Key.Equals(firstColumnArray[j]))
                    {
                        firstColumnArray[j] += count;
                        count++;
                    }
                }
            }

            //Export output to CSV
            csvContent[0] = string.Join(",", firstColumnArray);
            Console.WriteLine("Complete File : {0}", fileName.Substring((fileName.IndexOf("CSV\\") + 4)) + "\n");
            ExportAsCsv(fileName, csvContent);  			
        }

        static void ExportAsCsv(string fileName, string[] input)
        {
            File.WriteAllLines(fileName, input);
        }

        static void Main(string[] args)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string csvDir = @"\CSV";
            string[] csvFileEntries = Directory.GetFiles(currentDir + csvDir);

            if(csvFileEntries.Length > 0)
            {
                foreach (string fileName in csvFileEntries)
                {
                    if (fileName.Contains(".csv"))
                    {
                        Console.WriteLine("Loading : " + fileName);
                        string[] csvContent = File.ReadAllLines(fileName);

                        CheckOccurrence(fileName, csvContent);
                        ReplaceDuplicateMember(fileName, csvContent);
                    }
                }
            }
        }
    }
}