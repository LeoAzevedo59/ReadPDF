namespace Adapar.src
{
  using System;
  using iTextSharp.text;
  using iTextSharp.text.pdf;
  using System.IO;

  public class Pages
  {
    const string pathInput = Constantes.PATH_ORIGIN;

    public static void Separate()
    {
      int maxPagesPerPage = Constantes.MAX_PAGES_PER_FILE;

      PdfReader reader = new(pathInput);
      int totalPages = reader.NumberOfPages;

      int fileCounter = 1;

      for (int startPage = 1; startPage <= totalPages; startPage += maxPagesPerPage)
      {
        string outputPdf = $"{Constantes.PATH_OUTPUT}{fileCounter}.pdf";

        using (FileStream fs = new(outputPdf, FileMode.Create, FileAccess.Write))
        {
          using Document document = new();
          PdfCopy writer = new(document, fs);
          document.Open();

          int endPage = Math.Min(startPage + maxPagesPerPage - 1, totalPages);

          for (int currentPage = startPage; currentPage <= endPage; currentPage++)
          {
            PdfImportedPage importedPage = writer.GetImportedPage(reader, currentPage);
            writer.AddPage(importedPage);
          }
        }

        fileCounter++;
      }
    }

    public static void RemoveFile()
    {

    }
    public static int CountTotalPages()
    {
      PdfReader reader = new(pathInput);
      int totalPages = reader.NumberOfPages;

      return totalPages;
    }
  }
}