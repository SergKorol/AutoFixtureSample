using AutoFixture.AutoFakeItEasy;
using AutoFixture.AutoMoq;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using AutoFixtureSample.Models;
using FakeItEasy;
using FluentAssertions;
using Moq;
using NSubstitute;

namespace AutoFixtureSample;

public class SampleTests
{
    private readonly IFixture _fixture;
    public SampleTests()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            }).Customize(new AutoNSubstituteCustomization()
            {
                ConfigureMembers = true
            }).Customize(new AutoFakeItEasyCustomization()
            {
                ConfigureMembers = true
            });
    }
    
    [Fact]
    public void WhenEmployeeInstanceCreated_ThenCreatedValidInstance()
    {
        //Arrange
        var name = _fixture.Create<string>();
        var surname = _fixture.Create<string>();
        var age = _fixture.Create<int>();
        var position = _fixture.Create<string>();
        var rate = _fixture.Create<decimal>();
        //Act
        var employee = new Employee(name, surname, age, position, rate);

        //Assert
        employee.Name.Should().Be(name);
        employee.Surname.Should().Be(surname);
        employee.Age.Should().Be(age);
        employee.Position.Should().Be(position);
        employee.Rate.Should().Be(rate);
    }
    
    [Theory, AutoData]
    public void WhenEmployeeInstanceCreatedWithAutoData_ThenCreatedValidInstance(
        string name, string surname, int age, string position, decimal rate)
    {
        // Act
        var employee = new Employee(name, surname, age, position, rate);
        // Assert
        employee.Name.Should().Be(name);
        employee.Surname.Should().Be(surname);
        employee.Age.Should().Be(age);
        employee.Position.Should().Be(position);
        employee.Rate.Should().Be(rate);
    }
    
    [Fact]
    public void WhenAddNewEmployee_ThenAddedOneEmployee()
    {
        // Arrange
        var employee = _fixture.Create<Employee>();
        var company = _fixture.Create<Company>();
        var employeesCount = company.Employees.Count;
        // Act
        company.AddEmployee(employee);
        // Assert
        company.Employees.Count.Should().Be(employeesCount + 1);
    }
    
    [Fact]
    public void WhenRemoveEmployee_ThenRemovedEmployee()
    {
        // Arrange
        var employees = _fixture.CreateMany<Employee>(5).ToList();
        var company = _fixture.Build<Company>()
            .With(x => x.Employees, employees)
            .Create();
        // Act
        company.RemoveEmployee(employees.First());
        // Assert
        company.Employees.Count.Should().Be(4);
    }
    
    [Fact]
    public void WhenAddNewEmployee_CreatedInstanceWithAutoProperties()
    {
        // Arrange
        var employee = _fixture.Build<Employee>()
            .OmitAutoProperties()
            .Create();
        var employees = _fixture.Build<Employee>()
            .OmitAutoProperties()
            .CreateMany(5)
            .ToList();
        var company = _fixture.Build<Company>()
            .With(x => x.Employees, employees)
            .Without(x => x.City)
            .Create();
        // Act
        company.AddEmployee(employee);
        // Assert
        company.Employees.Count.Should().Be(6);
        company.City.Should().Be(null);
    }

    [Theory, AutoData]
    public void GetEmployee_WhenSpecificNameAndSurname_ThenEmployeeIsReturned(
        string name, string surname)
    {
        // Arrange
        var employees = _fixture.Build<Employee>()
            .OmitAutoProperties()
            .With(x => x.Name, name)
            .With(x => x.Surname, surname)
            .CreateMany(1)
            .ToList();
        var department = _fixture.Build<Company>()
            .With(x => x.Employees, employees)
            .Create();
        // Act
        var employee = department.GetEmployee(name);
        // Assert 
        employee.Should().NotBeNull();
        employee?.Name.Should().Be(name);
        employee?.Email.Should().Be(null);
    }
    
    [Theory, AutoData]
    public void CalculateSalary_ThenValidInstanceIsReturnedMoq(string name)
    {
        // Act
        var payrollService = new Mock<IPayrollService>();
        var employee = _fixture.Create<Employee>();
        payrollService.Setup(x => x.GetSalary(employee)).Returns(1000);
        var company = new Company(name, payrollService.Object);
        var salary = company.GetSalary(employee);
        // Assert
        salary.Should().Be(1000);
    }
    
    [Theory, AutoData]
    public void CalculateSalary_ThenValidInstanceIsReturnedNSubstitute(string name)
    {
        // Arrange
        var payrollService = Substitute.For<IPayrollService>();
        var employee = _fixture.Create<Employee>();
        payrollService.GetSalary(employee).Returns(1000);
    
        // Act
        var company = new Company(name, payrollService);
        var salary = company.GetSalary(employee);
    
        // Assert
        salary.Should().Be(1000);
    }

    [Fact]
    public void WhenEmployeeInstanceCreatedFake_ThenCreatedValidInstance()
    {
        //Arrange
        var employee = A.Fake<Employee>();
        var company = A.Fake<Company>();
        //Act
        company.AddEmployee(employee);
        var currentEmployee = company.Employees.Single();
        //Assert

        currentEmployee.Name.Should().Be(employee.Name);
        currentEmployee.Surname.Should().Be(employee.Surname);
        currentEmployee.Age.Should().Be(employee.Age);
        currentEmployee.Position.Should().Be(employee.Position);
        currentEmployee.Rate.Should().Be(employee.Rate);
    }
}