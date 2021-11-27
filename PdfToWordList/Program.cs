using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

string filePath = args[0];
Console.WriteLine(pdfText(filePath));

string pdfText(string path)
{
    //Read PDF
    StringBuilder text = new StringBuilder();
    using (PdfReader reader = new PdfReader(path))
    {
        PdfDocument document = new PdfDocument(reader);
        for (int i = 1; i <= document.GetNumberOfPages(); i++)
            text.Append(PdfTextExtractor.GetTextFromPage(document.GetPage(i)));
    }
    
    //Handle data
    char[] arr = text.ToString().ToCharArray();
    arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-')));
    string str = new string(arr);

    List<string> words = str.Replace("\n", "").ToLower().Split(" ").ToList();
    List<string> uniqueWords = words.Select(x => x).Distinct().ToList();
    uniqueWords.Remove(""); //..?

    //Save results
    string newFilePath = filePath + ".txt";
    using (StreamWriter sw = File.CreateText(newFilePath))
    {
        for (int i = 0; i < uniqueWords.Count; i++)
            sw.Write(uniqueWords[i] + (i == uniqueWords.Count - 1 ? "" : ", "));
    }

    return "File saved at " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, newFilePath);
}