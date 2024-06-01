﻿using Adapar.src;
using Adapar.src.UseCase.ProductCompositions;
using Adapar.src.UseCase.Products;
using Newtonsoft.Json;

//lista atualizada
//https://www.adapar.pr.gov.br/system/files/publico/Agrotoxicos/lista.pdf

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

  Guid productId = Guid.NewGuid();

  products.Add(new()
  {
    Id = productId,
    Description = row.MarcaComercial,
    Category = row.ClasseDeUso,
    Unit = row.Unid,
    Code = row.Registro,
    Enterprise = row.EmpresaRegistrante,
    ClassToxicological = row.ClasseToxicologica
  });

  if (string.IsNullOrEmpty(row.ConcIa) || string.IsNullOrEmpty(row.IngredienteAtivo))
    Console.WriteLine($"Error: {JsonConvert.SerializeObject(row)}");
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
        Console.WriteLine($"Error: {JsonConvert.SerializeObject(row)}");
      }
    }
  }
}