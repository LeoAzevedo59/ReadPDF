

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Adapar.src.UseCase.ProductCompositions
{
  public class ProductComposition
  {
    public int Product { get; set; }
    public string? Concentration { get; set; }
    public string? Ingredient { get; set; }
  }

  public class DatabaseProductCompositionHandler
  {
    // Reutilizando a connection string
    private readonly string _connectionString = "Server=tcp:brever-staging.database.windows.net,1433;Initial Catalog=staging;Persist Security Info=False;User ID=Adm;Password=UZf48vF7qhOyWffFZDTZ;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public void InsertProductCompositions(List<ProductComposition> productCompositions)
    {
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        foreach (var composition in productCompositions)
        {
          string query = "INSERT INTO product_compositions_adapar (Product, Concentration, Ingredient) " +
                         "VALUES (@Product, @Concentration, @Ingredient)";

          try
          {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
              command.Parameters.AddWithValue("@Product", composition.Product);
              command.Parameters.AddWithValue("@Concentration", composition.Concentration ?? (object)DBNull.Value);
              command.Parameters.AddWithValue("@Ingredient", composition.Ingredient ?? (object)DBNull.Value);

              command.ExecuteNonQuery();
            }
          }
          catch (Exception ex)
          {
            string json = JsonConvert.SerializeObject(composition);
            Console.WriteLine($"Error: {json}");

            File.AppendAllText($"{Constantes.PATH_LOG}", json + Environment.NewLine);
            System.Console.WriteLine(ex.Message);
          }


        }
      }
    }
  }
}