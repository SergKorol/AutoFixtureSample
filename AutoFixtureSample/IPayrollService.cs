using AutoFixtureSample.Models;

namespace AutoFixtureSample;

public interface IPayrollService
{
    decimal GetSalary(Employee employee);
}