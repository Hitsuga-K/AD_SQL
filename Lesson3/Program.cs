using System;
using College_DB.Data;

namespace College_DB
{
    class Program
    {
        public static string conn_str = "Data Source=COMP11A1\\SQLEXPRESS;" +
            "Initial Catalog=College_DB;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

        static StudentRepository student_repo = new StudentRepository(conn_str);
        static GroupRepository group_repo = new GroupRepository(conn_str);

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Меню ===");
                Console.WriteLine("1 - Показать всех студентов");
                Console.WriteLine("2 - Показать все группы");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllStudent(conn_str);
                        break;

                    case "2":
                        ShowAllGroups(conn_str);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("не та команда");
                        break;
                }

                Console.WriteLine("Продолжить");
                Console.ReadKey();
            }
        }

        static void ShowAllStudent(string conn_str)
        {
            var students = student_repo.GetAllStudent();
            foreach (var student in students)
            {
                Console.WriteLine(student.ToString());
            }
        }

        static void ShowAllGroups(string conn_str)
        {
            var groups = group_repo.GetAllGroups();
            foreach (var group in groups)
            {
                Console.WriteLine(group.ToString());
            }
        }
    }
}