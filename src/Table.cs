using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace Adapar.src
{
  public class Table
  {
    public static List<Column> Separate()
    {
      var Columns = new List<Column>();
      var column = new Column();
      int colIndex = 1;

      int maxPagesPerPage = Constantes.MAX_PAGES_PER_FILE;
      int totalPages = (Pages.CountTotalPages() / maxPagesPerPage) + 2;
      try
      {
        for (int fileCounter = 1; fileCounter < totalPages; fileCounter++)
        {
          if (fileCounter > Pages.CountTotalPages())
            continue;

          Console.WriteLine($"File: {fileCounter}/{Pages.CountTotalPages() / maxPagesPerPage}");

          string path = $"{Constantes.PATH_OUTPUT}{fileCounter}.pdf";

          var document = new Document(path);


          foreach (Page page in document.Pages)
          {
            var pageNumber = page.Number;

            TableAbsorber absorber = new();
            absorber.Visit(page);

            foreach (AbsorbedTable table in absorber.TableList)
            {
              foreach (AbsorbedRow row in table.RowList)
              {
                foreach (AbsorbedCell cell in row.CellList)
                {
                  try
                  {
                    string content = string.Empty;

                    foreach (var item in cell.TextFragments)
                    {
                      content += item.Text.Trim();
                    }

                    if (content.Contains("Restrição", StringComparison.CurrentCultureIgnoreCase) ||
                        content.Contains("Restrições", StringComparison.CurrentCultureIgnoreCase))
                    {
                      int lastItem = Columns.Count - 1;
                      var lastRow = Columns[lastItem];

                      lastRow.Restricao = content;

                      column = new();
                      colIndex = 1;
                      continue;
                    }
                    else if (colIndex == 1 && WordIsLowCase(content)) // é restrição
                    {
                      int lastItem = Columns.Count - 1;
                      var lastRow = Columns[lastItem];

                      lastRow.Restricao += $" {content}";
                      lastRow.Restricao = lastRow.Restricao.Trim();

                      column = new();
                      colIndex = 1;
                      continue;
                    }

                    if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(content.Trim()))
                      Console.WriteLine(content);
                    else
                      content = content.Trim();

                    switch (colIndex)
                    {
                      case (int)ColumnEnum.Marca:
                        column.MarcaComercial = content;
                        break;
                      case (int)ColumnEnum.Classe:
                        column.ClasseDeUso = content;
                        break;
                      case (int)ColumnEnum.Concentracao:
                        column.ConcIa = content;
                        break;
                      case (int)ColumnEnum.Registro:
                        column.Registro = content;
                        break;
                      case (int)ColumnEnum.Empresa:
                        column.EmpresaRegistrante = content;
                        break;
                      case (int)ColumnEnum.IngredienteAtivo:
                        column.IngredienteAtivo = content;
                        break;
                      case (int)ColumnEnum.ClasseToxicologica:
                        column.ClasseToxicologica = content;
                        Columns.Add(column);
                        column = new();
                        colIndex = 0;
                        break;
                      default:
                        break;
                    }

                    colIndex++;
                  }
                  catch (Exception ex)
                  {
                    Console.WriteLine(ex.Message);
                    throw;
                  }
                }
              }
            }
          }

        }
        return Columns;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
      }
    }

    public static bool WordIsLowCase(string word)
    {
      if (word.Length < 20)
        return false;

      foreach (char letter in word)
      {
        if (char.IsLower(letter))
        {
          return true;
        }
      }

      return false;
    }
  }
}
