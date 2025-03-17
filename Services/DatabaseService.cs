using Npgsql;
using Ptmk_test_task.Models;

namespace Ptmk_test_task.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateTable()
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Employees (
                        Id SERIAL PRIMARY KEY,
                        FullName TEXT NOT NULL,
                        DateOfBirth DATE NOT NULL,
                        Gender TEXT NOT NULL
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void InsertEmployee(Employee employee)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO Employees (FullName, DateOfBirth, Gender) VALUES (@fullName, @dateOfBirth, @gender)";
                cmd.Parameters.AddWithValue("fullName", employee.FullName);
                cmd.Parameters.AddWithValue("dateOfBirth", employee.DateOfBirth);
                cmd.Parameters.AddWithValue("gender", employee.Gender);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void InsertEmployeesBatch(List<Employee> employees)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO Employees (FullName, DateOfBirth, Gender) VALUES (@fullName, @dateOfBirth, @gender)";
                foreach (var employee in employees)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("fullName", employee.FullName);
                    cmd.Parameters.AddWithValue("dateOfBirth", employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("gender", employee.Gender);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public List<Employee> GetAllEmployees()
    {
        var employees = new List<Employee>();
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT FullName, DateOfBirth, Gender FROM Employees ORDER BY FullName", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee(
                            reader.GetString(0),
                            reader.GetDateTime(1),
                            reader.GetString(2)
                        ));
                    }
                }
            }
        }
        return employees;
    }

    public List<Employee> GetMaleEmployeesWithFLastName()
    {
        var employees = new List<Employee>();
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT FullName, DateOfBirth, Gender FROM Employees WHERE Gender = 'Male' AND FullName LIKE 'F%'", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee(
                            reader.GetString(0),
                            reader.GetDateTime(1),
                            reader.GetString(2)
                        ));
                    }
                }
            }
        }
        return employees;
    }
}