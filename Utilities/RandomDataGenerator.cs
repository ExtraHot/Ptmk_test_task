namespace Ptmk_test_task.Utilities;

public static class RandomDataGenerator
{
    public static string GenerateRandomName(Random random)
    {
        var firstNames = new[] { "John", "Jane", "Michael", "Emily", "David", "Sarah" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia" };
        return $"{lastNames[random.Next(lastNames.Length)]} {firstNames[random.Next(firstNames.Length)]}";
    }

    public static DateTime GenerateRandomDate(Random random)
    {
        var start = new DateTime(1950, 1, 1);
        var range = (DateTime.Today - start).Days;
        return start.AddDays(random.Next(range));
    }
}