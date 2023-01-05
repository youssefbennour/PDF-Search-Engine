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
            idx.storeIndexation();
            while (true)
            {
                Console.Write("Enter a word: ");
                string hashedWordWord = Console.ReadLine();
                int hashedWord = hashedWordWord.GetHashCode();

                if (idx.wordIndexer.ContainsKey(hashedWord))
                {
                    for (int j = 0, outputCounter = 0; j < idx.wordIndexer[hashedWord].Count & outputCounter <= 15; j += 4, outputCounter++)
                    {
                        Console.WriteLine("--------------------------------------");
                        Console.WriteLine($"|PDF NAME: {idx.FileNameIndexer[idx.wordIndexer[hashedWord].ElementAt(j)]}");
                        Console.WriteLine($"|PDF PAGE: {idx.wordIndexer[hashedWord].ElementAt(j + 1)}");
                        Console.WriteLine($"|Line Number: {idx.wordIndexer[hashedWord].ElementAt(j + 2)}");
                        Console.WriteLine($"|word number: {idx.wordIndexer[hashedWord].ElementAt(j + 3)}");
                        Console.WriteLine("--------------------------------------");

                    }
                }
                else
                {
                    Console.WriteLine("Not Found");
                }
            }
        }
    }
}
