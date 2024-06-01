using Adapar;
using Aspose.Pdf;
using Aspose.Pdf.Text;

string path = "./src/Pdfs/adapar-24-05-2024.pdf";

var Columns = new List<Column>();
var column = new Column();
int colIndex = 1;

var document = new Document(path);

foreach (Page page in document.Pages)
{
  TableAbsorber absorber = new();
  absorber.Visit(page);

  foreach (AbsorbedTable table in absorber.TableList)
  {
    foreach (AbsorbedRow row in table.RowList)
    {
      foreach (AbsorbedCell cell in row.CellList)
      {

        string content = string.Empty;

        foreach (var item in cell.TextFragments)
        {
          content += item.Text;
        }

        Console.WriteLine(content);

        if (content.Contains("Restrição"))
        {
          int lastItem = Columns.Count - 1;
          var lastRow = Columns[lastItem];

          lastRow.Restricao = content;

          column = new();
          colIndex = 1;
          continue;
        }

        content = content.Trim();

        switch (colIndex)
        {
          case 1:
            column.MarcaComercial = content;
            break;
          case 2:
            column.ClasseDeUso = content;
            break;
          case 3:
            column.Unid = content;
            break;
          case 4:
            column.ConcIa = content;
            break;
          case 5:
            column.Registro = content;
            break;
          case 6:
            column.EmpresaRegistrante = content;
            break;
          case 7:
            column.ClasseToxicologica = content;
            break;
          case 8:
            column.IngredienteAtivo = content;
            Columns.Add(column);
            column = new();
            colIndex = 1;
            break;
          default:
            break;
        }

        colIndex++;
      }

      Console.WriteLine("FINISH_OBJ");
    }
  }
}
