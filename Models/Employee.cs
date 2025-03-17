using System;

namespace Ptmk_test_task.Models;

public class Employee
{
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }

    public Employee(string fullName, DateTime dateOfBirth, string gender)
    {
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }

    public int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}