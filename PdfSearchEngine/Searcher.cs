using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfSearchEngine {
    internal class Searcher {
        public void displaySearchResult(Indexer idx)
        {
            idx.listDirectoryFiles();
            //idx.storeIndexation();
            while (true)
            {
                Console.Write("Enter a word: ");
                string word = Console.ReadLine();
                string filePath = "index.txt";
                int numberOfLines = File.ReadAllLines(filePath).Length;

                int start = 1, end = numberOfLines;
                bool foundWord = false;

                while (start <= end)
                {
                    int mid = start + (end - start) / 2;
                    string [] splittedData = File.ReadLines(filePath).Skip(mid - 1).Take(1).First().Split(' ');
                    if (string.Compare(word, splittedData[0]) == 0)
                    {
                        foundWord = true;
                        for(int occurenceIndex = 1, searchCounter = 0; occurenceIndex < splittedData.Length && searchCounter < 15; occurenceIndex+=4, searchCounter++)
                        {
                            Console.WriteLine("--------------------------------------");
                            Console.WriteLine($"|PDF NAME: {idx.FileNameIndexer[Int32.Parse(splittedData[occurenceIndex])]}");
                            Console.WriteLine($"|PDF PAGE: {splittedData[occurenceIndex+1]}");
                            Console.WriteLine($"|Line Number: {splittedData[occurenceIndex+2]+1}");
                            Console.WriteLine($"|word number: {splittedData[occurenceIndex+3]+1}");
                            Console.WriteLine("--------------------------------------");
                        }
                        break;
                    }
                    else if (string.Compare(word, splittedData[0]) > 0)
                    {
                        start = mid + 1;
                    }
                    else
                    {
                        end = mid - 1;
                    }
                }
                if (!foundWord)
                {
                    Console.WriteLine("Not found");
                }
            }
        }
    }
}
