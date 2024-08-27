using Adapar.src;
using Adapar.src.UseCase.ProductCompositions;
using Adapar.src.UseCase.Products;
using ClosedXML.Excel;
using Newtonsoft.Json;

//lista atualizada
//https://www.adapar.pr.gov.br/system/files/publico/Agrotoxicos/lista.pdf

try
{
  Pages.RemoveFile();
  Pages.Separate();
  var rows = Table.Separate();
  Pages.RemoveFile();

  var headerTable = rows[0];
  rows.Remove(headerTable);

  var products = new List<Product>();
  var productCompositionList = new List<ProductComposition>();

  foreach (var row in rows)
  {
    if (row is null)
      throw new ArgumentException("Error in row");

    var concentrations = row.ConcIa.Split("+");
    var Ingredients = row.IngredienteAtivo.Split("+");

    int productId = products.Count + 1;

    products.Add(new()
    {
      Id = productId,
      Description = row.MarcaComercial,
      Category = row.ClasseDeUso,
      Code = row.Registro,
      Enterprise = row.EmpresaRegistrante,
      ClassToxicological = row.ClasseToxicologica,
      Restricao = row.Restricao
    });

    if (string.IsNullOrEmpty(row.ConcIa) || string.IsNullOrEmpty(row.IngredienteAtivo))
    {
      string json = JsonConvert.SerializeObject(row);
      Console.WriteLine($"Error: {json}");

      File.AppendAllText($"{Constantes.PATH_LOG}", json + Environment.NewLine);
    }
    else if (concentrations.Count() < Ingredients.Count())
    {
      for (int i = 0; i < concentrations.Length; i++)
      {
        try
        {
          productCompositionList.Add(new()
          {
            Product = productId,
            Concentration = concentrations[0].Trim(),
            Ingredient = Ingredients[i].Trim()
          });
        }
        catch (Exception)
        {
          string json = JsonConvert.SerializeObject(row);
          Console.WriteLine($"Error: {json}");

          File.AppendAllText($"{Constantes.PATH_LOG}", json + Environment.NewLine);
        }
      }
    }
    else if (concentrations.Count() > Ingredients.Count())
    {
      for (int i = 0; i < concentrations.Length; i++)
      {
        try
        {
          productCompositionList.Add(new()
          {
            Product = productId,
            Concentration = concentrations[i].Trim(),
            Ingredient = Ingredients[0].Trim()
          });
        }
        catch (Exception)
        {
          string json = JsonConvert.SerializeObject(row);
          Console.WriteLine($"Error: {json}");

          File.AppendAllText($"{Constantes.PATH_LOG}", json + Environment.NewLine);
        }
      }
    }
    else
    {
      for (int i = 0; i < concentrations.Length; i++)
      {
        try
        {
          productCompositionList.Add(new()
          {
            Product = productId,
            Concentration = concentrations[i].Trim(),
            Ingredient = Ingredients[i].Trim()
          });
        }
        catch (Exception)
        {
          string json = JsonConvert.SerializeObject(row);
          Console.WriteLine($"Error: {json}");

          File.AppendAllText($"{Constantes.PATH_LOG}", json + Environment.NewLine);
        }
      }
    }
  }

  using (var workbook = new XLWorkbook())
  {
    // Adiciona a planilha de Products
    var productsSheet = workbook.Worksheets.Add("Products");

    productsSheet.Cell(1, 1).Value = "Id";
    productsSheet.Cell(1, 2).Value = "MARCA";
    productsSheet.Cell(1, 3).Value = "EMPRESA";
    productsSheet.Cell(1, 4).Value = "CLASSE";
    productsSheet.Cell(1, 5).Value = "REGISTRO";
    productsSheet.Cell(1, 6).Value = "CLASSE TOXICOL.";
    productsSheet.Cell(1, 7).Value = "RESTRIÇÃO";

    int row = 2;
    foreach (var product in products)
    {
      productsSheet.Cell(row, 1).Value = product.Id;
      productsSheet.Cell(row, 2).Value = product.Description;
      productsSheet.Cell(row, 3).Value = product.Enterprise;
      productsSheet.Cell(row, 4).Value = product.Category;
      productsSheet.Cell(row, 5).Value = product.Code;
      productsSheet.Cell(row, 6).Value = product.ClassToxicological;
      productsSheet.Cell(row, 7).Value = product.Restricao;

      row++;
    }

    // Adiciona a planilha de ProductComposition
    var compositionSheet = workbook.Worksheets.Add("ProductComposition");

    compositionSheet.Cell(1, 1).Value = "ProductId";
    compositionSheet.Cell(1, 2).Value = "INGREDIENTE ATIVO";
    compositionSheet.Cell(1, 3).Value = "CONCENTRAÇÃO";

    row = 2;
    foreach (var composition in productCompositionList)
    {
      compositionSheet.Cell(row, 1).Value = composition.Product;
      compositionSheet.Cell(row, 2).Value = composition.Ingredient;
      compositionSheet.Cell(row, 3).Value = composition.Concentration;

      row++;
    }

    // Salva o arquivo
    workbook.SaveAs("ProductsAndComposition.xlsx");
  }
}
catch (Exception ex)
{
  System.Console.WriteLine(ex.Message);
  Pages.RemoveFile();
}
finally
{
  Pages.RemoveFile();
}


// DatabaseProductCompositionHandler dbCompositionHandler = new();
// DatabaseProductHandler dbProductHandler = new();

// dbProductHandler.InsertProducts(products);
// dbCompositionHandler.InsertProductCompositions(productCompositionList);