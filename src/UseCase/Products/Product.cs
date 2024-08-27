using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Adapar.src.UseCase.Products
{
  public class Product
  {
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Code { get; set; }
    public string? Enterprise { get; set; }
    public string? ClassToxicological { get; set; }
    public string? Restricao { get; set; }
  }

  public class DatabaseProductHandler
  {
    private readonly string _connectionString = "Server=tcp:brever-staging.database.windows.net,1433;Initial Catalog=staging;Persist Security Info=False;User ID=Adm;Password=UZf48vF7qhOyWffFZDTZ;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public void InsertProducts(List<Product> products)
    {
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        foreach (var product in products)
        {
          try
          {

            string query = "INSERT INTO products_adapar (Id, Description, Category, Unit, Code, Enterprise, ClassToxicological) " +
                          "VALUES (@Id, @Description, @Category, @Unit, @Code, @Enterprise, @ClassToxicological)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
              command.Parameters.AddWithValue("@Id", product.Id);
              command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
              command.Parameters.AddWithValue("@Category", product.Category ?? (object)DBNull.Value);
              command.Parameters.AddWithValue("@Code", product.Code ?? (object)DBNull.Value);
              command.Parameters.AddWithValue("@Enterprise", product.Enterprise ?? (object)DBNull.Value);
              command.Parameters.AddWithValue("@ClassToxicological", product.ClassToxicological ?? (object)DBNull.Value);

              command.ExecuteNonQuery();
            }
          }
          catch (Exception ex)
          {
            string json = JsonConvert.SerializeObject(product);
            Console.WriteLine($"Error: {json}");

            File.AppendAllText($"{Constantes.PATH_LOG}", json + Environment.NewLine);
            System.Console.WriteLine(ex.Message);
          }
        }
      }
    }
  }
}
