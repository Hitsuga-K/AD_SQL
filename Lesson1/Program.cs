using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Project_Lesson
{
    class Program
    {
        static string connectionString =
            "Data Source=COMP11A1\\SQLEXPRESS;" +
            "Initial Catalog=Primer;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("База продуктов");
                Console.WriteLine("1. Показать 10 продуктов");
                Console.WriteLine("2. Найти продукт по ID");
                Console.WriteLine("3. Создать продукт");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAllProducts();
                        break;
                    case "2":
                        FindProduct();
                        break;
                    case "3":
                        addProduct();
                        break;
                    case "x":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("неправильно");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nEnter выйти");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("привет");
            Console.ReadKey();
        }

        static void ListAllProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql_command = "SELECT TOP 10 * FROM Products";
                SqlCommand command = new SqlCommand(sql_command, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Name = reader.GetInt32(0);
                        string ProductNumber = reader.GetString(1);
                        string ListPrice = reader.GetString(2);
                        Boolean StandartCost = reader.GetBoolean(3);

                        Console.WriteLine($"Имя: {Name}, Номер продукта: {ProductNumber}, Лист прайс: {ListPrice}, Стандарт кост: {StandartCost}");
                    }
                }
            }
        }

        static void FindProduct()
        {
            Console.Write("Введите ID продукта: ");
            string id = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql_command = "SELECT * FROM Products WHERE ProductID = @id";
                SqlCommand command = new SqlCommand(sql_command, connection);
                command.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int Name = reader.GetInt32(0);
                        string ProductNumber = reader.GetString(1);
                        string ListPrice = reader.GetString(2);
                        Boolean StandartCost = reader.GetBoolean(3);

                        Console.WriteLine($"Найден: Имя: {Name}, Номер продукта: {ProductNumber}, Лист прайс: {ListPrice}, Стандарт кост: {StandartCost}");
                    }
                    else
                    {
                        Console.WriteLine("Продукт не найден.");
                    }
                }
            }
        }
        static void addProduct()
        {
                Console.Write("Введите название продукта: ");
                string name = Console.ReadLine();

                Console.Write("Введите номер продукта: ");
                string productNumber = Console.ReadLine();

                Console.Write("Введите цену: ");
                string listPrice = Console.ReadLine();

                Console.Write("Введите стандартную стоимость: ");
                string standardCost = Console.ReadLine();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql_command =
                        "INSERT INTO Products (Name, ProductNumber, ListPrice, StandardCost) " +
                        "VALUES (@name, @productNumber, @listPrice, @standardCost)";

                    SqlCommand command = new SqlCommand(sql_command, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@productNumber", productNumber);
                    command.Parameters.AddWithValue("@listPrice", listPrice);
                    command.Parameters.AddWithValue("@standardCost", standardCost);

                    int rows = command.ExecuteNonQuery();
                    Console.WriteLine($"Добавлено строк: {rows}");
                }
        }
    }
}