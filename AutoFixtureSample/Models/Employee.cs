namespace AutoFixtureSample.Models;

public class Employee
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Position { get; set; }

    public string Email { get; set; }
    public decimal Rate { get; set; }
    
    public Employee(string name, string surname, int age, string position, decimal rate)
    {
        Name = name;
        Surname = surname;
        Age = age;
        Position = position;
        Rate = rate;
    }
    public string GetFullName() => $"{Name} {Surname}";

    public decimal DayRate() => Rate * 8;
}