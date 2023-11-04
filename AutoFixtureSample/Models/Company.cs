namespace AutoFixtureSample.Models;

public class Company
{
    private readonly IPayrollService _payrollService;
    public string Name { get; set; }
    public string City { get; set; }
    public List<Employee> Employees { get; set; }

    public Company(string name, IPayrollService payrollService)
    {
        Name = name;
        Employees = new List<Employee>();
        _payrollService = payrollService;
    }
    
    public void AddEmployee(Employee employee)
    {
        Employees.Add(employee);
    }
    
    public void RemoveEmployee(Employee employee)
    {
        Employees.Remove(employee);
    }
    
    public Employee? GetEmployee(string name)
    {
        return Employees
            .FirstOrDefault(e => e.Name == name);
    }

    public decimal GetSalary(Employee employee)
    {
        return _payrollService.GetSalary(employee);
    }
}