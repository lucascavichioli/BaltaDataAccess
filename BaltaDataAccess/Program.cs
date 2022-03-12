using System;
using System.Data.SqlClient;
using BaltaDataAccess.Models;
using Dapper; //Dapper

namespace BaltaDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";

            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Conectado");
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT Id, Title FROM Category";

                    var reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        //Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
                    }

                    connection.Close();
                }
            }


            Dapper();

        }

        static void Dapper()
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";
                       
            using (var connection = new SqlConnection(connectionString))
            {
                UpdateCategory(connection);
                ListaCategories(connection);
                //CreateCategory(connection);

            }
        }


        static void ListaCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT Id, Title FROM Category");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }
        static void CreateCategory(SqlConnection connection) 
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO Category 
                              VALUES (
                                    @Id, 
                                    @title, 
                                    @url, 
                                    @summary, 
                                    @order, 
                                    @description, 
                                    @featured)";

            var rows = connection.Execute(insertSql, new
            {
                Id = category.Id,
                title = category.Title,
                url = category.Url,
                description = category.Description,
                order = category.Order,
                summary = category.Summary,
                featured = category.Featured

            }
                                  );

            Console.WriteLine($"{rows} linhas inseridas");

        }
        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title] =@title WHERE [Id]=@id";

            var rows = connection.Execute(updateQuery, new { 
                  id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
                  title = "Front End 2021"
            });

            Console.WriteLine($"{rows} registros Atualizadas");

        }
        static void DeleteCategory() 
        { 
        
        }
    }
}
