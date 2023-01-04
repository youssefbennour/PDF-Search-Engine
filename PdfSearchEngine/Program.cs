using System.Diagnostics;

namespace PdfSearchEngine {
    internal class Program {
        static void Main(string[] args) {
            Indexer idx = new Indexer();
            //idx.listDirectoryFiles();
            idx.indexAll();
    
            Searcher sc = new Searcher();
            sc.displaySearchResult(idx);
            
        }
    }
}