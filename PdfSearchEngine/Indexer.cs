using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfSearchEngine {
    internal class Indexer {
        //string directoryPath = @"C:\Users\Takiacademy\Desktop\PDF-Search-Engine";

        private string[] availablePdfs;
        private string PdfsPath { get; set; }
        private string pdfNameIndexer { get; set; }
        public SortedDictionary<int, List<int>> wordIndexer;
        public Dictionary<int, string> FileNameIndexer;

        private string[] stopWords;
        public string punctuationChars;

        public Indexer() {
            this.PdfsPath = "C:\\Users\\Takiacademy\\Desktop\\PDF-Search-Engine\\pdfs-challenge-2";
            this.pdfNameIndexer = "C:\\Users\\Takiacademy\\Desktop\\PDF-Search-Engine\\Current-pdfs.txt";
            this.availablePdfs = Directory.GetFiles(PdfsPath);
            this.wordIndexer = new SortedDictionary<int, List<int>>();
            this.FileNameIndexer = new Dictionary<int, string>();

            this.punctuationChars = ",?!.-'";
            this.stopWords = new string[] { "the", "a", "and" };

        }

        public void indexPdf(string path, int pdfId)
        {
            var codePages = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(codePages);
            PdfReader reader = new PdfReader(path);
            for (int pageIndex = 1; pageIndex <= reader.NumberOfPages; pageIndex++)
            {
                String[] lines = PdfTextExtractor.GetTextFromPage(reader, pageIndex).Split('\n');
                for(int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                {
                    string[] wordsArr = lines[lineIndex].Split(' ');
                    for (int wordIndex = 0; wordIndex < wordsArr.Length; wordIndex++)
                    {
                        int hashedWord = wordsArr[wordIndex].GetHashCode();
                        if (wordIndexer.ContainsKey(hashedWord))
                        {
                            wordIndexer[hashedWord].Add(pdfId);
                            wordIndexer[hashedWord].Add(pageIndex);
                            wordIndexer[hashedWord].Add(lineIndex);
                            wordIndexer[hashedWord].Add(wordIndex);
                        }
                        else
                        {
                            List<int> tempList = new List<int>();
                            tempList.Add(pdfId);
                            tempList.Add(pageIndex);
                            tempList.Add(lineIndex);
                            tempList.Add(wordIndex);
                            wordIndexer.Add(hashedWord, tempList);

                        }
                    }

                }
            }
            reader.Close();
        }
        public void indexAll()
        {
            int pdfId = 0;
            foreach (string file in this.availablePdfs)
            {
                pdfId++;
                indexPdf(file.ToString(), pdfId);
            }
        }

        public void storeIndexation()
        {
            //using (StreamWriter sw = new StreamWriter("indexation.txt", true, Encoding.UTF8, 65536))
            //{
            //    foreach(var item in wordIndexer)
            //    {
            //        string line = item.Key.ToString();
            //        foreach(var subItem in item.Value)
            //        {
            //            line += " " + subItem.ToString();
            //        }
            //        sw.WriteLine(line);
            //    }
            //}
            StringBuilder sb = new StringBuilder();
            foreach(var item in wordIndexer)
            {
                sb.Append(item.Key);
                foreach(var subItem in item.Value)
                {
                    sb.Append($" {subItem.ToString()}");
                }
                sb.Append('\n');
            }

            File.AppendAllText("indexxx.txt", sb.ToString());
        }


        public void listDirectoryFiles()
        {
            for(int i = 0; i< availablePdfs.Length; ++i)
            { 
                string finalPdfName = availablePdfs[i].Split('\\')[^1].Split(".pdf")[0];
                FileNameIndexer.Add(i + 1, finalPdfName);
                string line = $"{i + 1}";
            }
            
        }
    }
} 