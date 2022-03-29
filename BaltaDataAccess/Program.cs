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
                //UpdateCategory(connection);
                //CreateManyCategory(connection);
                //ListCategories(connection);
                //CreateCategory(connection);
                //ExecuteProcedure(connection);
                //ExecuteReadProcedure(connection);
                //ExecuteScalar(connection);
                //ReadView(connection);
                OneToOne(connection);

            }
        }

        static void ListCategories(SqlConnection connection)
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
        static void DeleteCategory(SqlConnection connection) 
        {
            var deleteQuery = "DELETE [Category] WHERE [Id]=@id";
            var rows = connection.Execute(deleteQuery, new
            {
                id = new Guid("ea8059a2-e679-4e74-99b5-e4f0b310fe6f"),
            });

            Console.WriteLine($"{rows} registros excluídos");
        }
        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "categoria-nova";
            category2.Description = "Categoria nova";
            category2.Order = 9;
            category2.Summary = "Categoria";
            category2.Featured = true;

            var insertSql = @"INSERT INTO Category 
                              VALUES (
                                    @Id, 
                                    @title, 
                                    @url, 
                                    @summary, 
                                    @order, 
                                    @description, 
                                    @featured)";

            var rows = connection.Execute(insertSql, new[] {
            new {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Description,
                    category.Order,
                    category.Summary,
                    category.Featured
                }, 
            new {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Description,
                    category2.Order,
                    category2.Summary,
                    category2.Featured
                }
            });

            Console.WriteLine($"{rows} linhas inseridas");

        }
        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "[spDeleteStudent]";
            var pars = new { StudentId = "c55390d4-71dd-4f3c-b978-d1582f51a327" };
            var affectedRows = connection.Execute(
                procedure, 
                pars, 
                commandType: System.Data.CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows} Linhas Afetadas");
        }
        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[spGetCoursesByCategory]";
            var pars = new { CategoryId = "25d510c8-3108-44c2-86c5-924d9832aa8c" };
            var courses = connection.Query<Category>(
                procedure,
                pars,
                commandType: System.Data.CommandType.StoredProcedure);

            foreach (var item in courses)
            {
                Console.WriteLine(item.Title);
            }
        }
        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO Category 
                              OUTPUT inserted.Id
                              VALUES (
                                    NEWID(), 
                                    @title, 
                                    @url, 
                                    @summary, 
                                    @order, 
                                    @description, 
                                    @featured) 
                            SELECT SCOPE_IDENTITY()";

            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                title = category.Title,
                url = category.Url,
                description = category.Description,
                order = category.Order,
                summary = category.Summary,
                featured = category.Featured

            }
                                  );

            Console.WriteLine($"A categoria inserida foi: {id}");

        }
        static void ReadView(SqlConnection connection) 
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void OneToOne(SqlConnection connection)
        {
            var sql = @"SELECT 
                            *
                        FROM 
                            [CareerItem] 
                        INNER JOIN 
                            [Career] ON [CareerItem].[CareerId] = [Career].[Id]";
            var items = connection.Query<CareerItem, Course, CareerItem>(
                sql,
                (careerItem, course) => { 
                    careerItem.Course = course; 
                    return careerItem; 
                  }, splitOn: "Id");

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
            }
        }
    }
}
