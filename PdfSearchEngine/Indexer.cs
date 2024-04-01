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
        private string PdfsPath { get; set; }
        private string pdfNameIndexer { get; set; }

        private string[] availablePdfs;
       
        public SortedDictionary<string, List<int>> wordIndexer;
        public Dictionary<int, string> FileNameIndexer;

        private string[] stopWords;
        public string punctuationChars;
        public int nullCharAscii;

        public Indexer() {
            this.PdfsPath = "C:\\Users\\Takiacademy\\Desktop\\PDF-Search-Engine\\pdfs-challenge-2";
            this.pdfNameIndexer = "C:\\Users\\Takiacademy\\Desktop\\PDF-Search-Engine\\Current-pdfs.txt";

            this.availablePdfs = Directory.GetFiles(PdfsPath);
            this.wordIndexer = new SortedDictionary<string, List<int>>();
            this.FileNameIndexer = new Dictionary<int, string>();

            this.nullCharAscii = 0;
            this.punctuationChars = ",?!.-'";
            this.stopWords = new string[] { "the", "a", "and" };

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
                        if (wordIndexer.ContainsKey(wordsArr[wordIndex]))
                        {
                            wordIndexer[wordsArr[wordIndex]].Add(pdfId);
                            wordIndexer[wordsArr[wordIndex]].Add(pageIndex);
                            wordIndexer[wordsArr[wordIndex]].Add(lineIndex);
                            wordIndexer[wordsArr[wordIndex]].Add(wordIndex);
                        }
                        else if (wordsArr[wordIndex].Length!=0 && wordsArr[wordIndex][0] != (Char)nullCharAscii)
                        {
                            List<int> tempList = new List<int>
                            {
                                pdfId,
                                pageIndex,
                                lineIndex,
                                wordIndex
                            };
                            wordIndexer.Add(wordsArr[wordIndex], tempList);

                        }
                    }

                }
            }
            reader.Close();
        }
       

        public void storeIndexation()
        {
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

            File.WriteAllText("index.txt", sb.ToString());
        }


        public void listDirectoryFiles()
        {
            for(int i = 0; i< availablePdfs.Length; ++i)
            { 
                string finalPdfName = availablePdfs[i].Split('\\')[^1].Split(".pdf")[0];
                FileNameIndexer.Add(i + 1, finalPdfName);
            }
            
        }
    }
} 