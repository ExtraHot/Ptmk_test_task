using System.Diagnostics;
using Ptmk_test_task.Models;
using Ptmk_test_task.Services;

namespace Ptmk_test_task;

static class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No arguments provided.");
            return;
        }

        var connectionString = "Host=localhost;Username=postgres;Password=123;Database=employeesdb";
        var dbService = new DatabaseService(connectionString);

        switch (args[0])
        {
            case "1":
                dbService.CreateTable();
                Console.WriteLine("Table created.");
                break;

            case "2":
                if (args.Length < 4)
                {
                    Console.WriteLine("Invalid arguments for mode 2.");
                    return;
                }
                var employee = new Employee(args[1], DateTime.Parse(args[2]), args[3]);
                dbService.InsertEmployee(employee);
                Console.WriteLine("Employee added.");
                break;

            case "3":
                var employees = dbService.GetAllEmployees();
                foreach (var emp in employees)
                {
                    Console.WriteLine($"{emp.FullName}, {emp.DateOfBirth.ToShortDateString()}, {emp.Gender}, {emp.CalculateAge()} years old");
                }
                break;

            case "4":
                var random = new Random();
                var employeesBatch = new List<Employee>();
                for (int i = 0; i < 1000000; i++)
                {
                    var fullName = GenerateRandomName(random);
                    var dateOfBirth = GenerateRandomDate(random);
                    var gender = random.Next(2) == 0 ? "Male" : "Female";
                    employeesBatch.Add(new Employee(fullName, dateOfBirth, gender));
                }
                for (int i = 0; i < 100; i++)
                {
                    employeesBatch.Add(new Employee("F" + GenerateRandomName(random), GenerateRandomDate(random), "Male"));
                }
                dbService.InsertEmployeesBatch(employeesBatch);
                Console.WriteLine("Employees added.");
                break;

            case "5":
                var stopwatch = Stopwatch.StartNew();
                var maleEmployees = dbService.GetMaleEmployeesWithFLastName();
                stopwatch.Stop();
                Console.WriteLine($"Query executed in {stopwatch.ElapsedMilliseconds} ms");
                foreach (var emp in maleEmployees)
                {
                    Console.WriteLine($"{emp.FullName}, {emp.DateOfBirth.ToShortDateString()}, {emp.Gender}, {emp.CalculateAge()} years old");
                }
                break;

            default:
                Console.WriteLine("Invalid mode.");
                break;
        }
    }

    static string GenerateRandomName(Random random)
    {
        var firstNames = new[] { "John", "Jane", "Michael", "Emily", "David", "Sarah" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia" };
        return $"{lastNames[random.Next(lastNames.Length)]} {firstNames[random.Next(firstNames.Length)]}";
    }

    static DateTime GenerateRandomDate(Random random)
    {
        var start = new DateTime(1950, 1, 1);
        var range = (DateTime.Today - start).Days;
        return start.AddDays(random.Next(range));
    }
}