using Adapar;
using Adapar.src;
using Aspose.Pdf;
using Aspose.Pdf.Text;

string path = "./src/Pdfs/adapar-24-05-2024.pdf";

var Columns = new List<Column>();
var column = new Column();
int colIndex = 1;

var document = new Document(path);
var x = document.Pages[5].Number;


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

            Console.WriteLine(content);

            if (content.Contains("Restrição") ||
                content.Contains("Restrições"))
            {
              int lastItem = Columns.Count - 1;
              var lastRow = Columns[lastItem];

              lastRow.Restricao = content;

              column = new();
              colIndex = 1;
              continue;
            }
            else if (content.Length >= 48)
            {
              int lastItem = Columns.Count - 1;
              var lastRow = Columns[lastItem];

              lastRow.Restricao += $" {content}";

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

        if (lastRow_test.MarcaComercial.Equals("ACTELLICLAMBDA"))
          Console.WriteLine("tt");

        Console.WriteLine("FINISH_OBJ");
      }
    }
  }
}
catch (Exception ex)
{
  System.Console.WriteLine(ex.Message);
  throw;
}