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
                Console.WriteLine("3 - Найти студента");
                Console.WriteLine("4 - Создать студента");
                Console.WriteLine("5 - Найти группу");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllStudent();
                        break;

                    case "2":
                        ShowAllGroups();
                        break;
                    case "3":
                        ShowStudentById();
                        break;
                    case "4":
                        addStudent();
                        break;
                    case "5":
                        ShowGroupById();
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

        static void ShowAllStudent()
        {
            var students = student_repo.GetAllStudent2();
            foreach (var student in students)
            {
                Console.WriteLine(student.ToString());
            }
        }

        static void addStudent()
        {
            Console.WriteLine("Введите имя студента");
            string first_name = Console.ReadLine();
            Console.WriteLine("Введите фамилию студента");
            string last_name = Console.ReadLine();
            Console.WriteLine("Введите возраст студента");
            string age = Console.ReadLine();
            Console.WriteLine("Введите группу студента");
            string group_id = Console.ReadLine();
            student_repo.CreateStudent(first_name, last_name, age, group_id);
            ShowAllStudent();
        }
        static void ShowStudentById()
        {
            Console.WriteLine("Введите номер студента");
            string choice = Console.ReadLine();

            var student = student_repo.GetStudentById(choice);

            if (student != null)
                Console.WriteLine(student.ToString());
            else
                Console.WriteLine("Студент с таким Id не найден");
        }
        static void ShowAllGroups()
        {
            var groups = group_repo.GetAllGroups();
            foreach (var group in groups)
            {
                Console.WriteLine(group.ToString());
            }
        }
        static void ShowGroupById()
        {
            Console.WriteLine("Введите номер группы");
            int groupId = Convert.ToInt32(Console.ReadLine());

            var group = group_repo.GetGroupById(groupId);

            Console.WriteLine(group.ToString());
        }
    }
}