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

      for (int fileCounter = 1; fileCounter < totalPages; fileCounter++)
      {
        Console.WriteLine($"Page: {fileCounter}");

        string path = $"{Constantes.PATH_OUTPUT}{fileCounter}.pdf";

        var document = new Document(path);

        try
        {
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
                      content += item.Text;
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
                    else if (content.Length >= 49 && colIndex == 1)
                    {
                      int lastItem = Columns.Count - 1;
                      var lastRow = Columns[lastItem];

                      lastRow.Restricao += $" {content}";
                      lastRow.Restricao = lastRow.Restricao.Trim();

                      column = new();
                      colIndex = 1;
                      continue;
                    }

                    content = content.Trim();

                    switch (colIndex)
                    {
                      case (int)ColumnEnum.MarcaComercial:
                        column.MarcaComercial = content;
                        break;
                      case (int)ColumnEnum.ClasseDeUso:
                        column.ClasseDeUso = content;
                        break;
                      case (int)ColumnEnum.Unid:
                        column.Unid = content;
                        break;
                      case (int)ColumnEnum.ConcIa:
                        column.ConcIa = content;
                        break;
                      case (int)ColumnEnum.Registro:
                        column.Registro = content;
                        break;
                      case (int)ColumnEnum.EmpresaRegistrante:
                        column.EmpresaRegistrante = content;
                        break;
                      case (int)ColumnEnum.ClasseToxicologica:
                        column.ClasseToxicologica = content;
                        break;
                      case (int)ColumnEnum.IngredienteAtivo:
                        column.IngredienteAtivo = content;
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

                int lastItem_test = Columns.Count - 1;
                var lastRow_test = Columns[lastItem_test];

                Console.WriteLine("Fish Obj");
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          throw;
        }
      }
      return Columns;
    }
  }
}
