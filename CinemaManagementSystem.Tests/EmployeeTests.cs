using System;
using System.IO;
using NUnit.Framework;
using CinemaManagementSystem;

namespace CinemaManagementSystem.Tests
{
    public class EmployeeTests
    {
        private const string FilePath = "employees_test.xml";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        [Test]
        public void Constructor_ValidData_ShouldCreateEmployee()
        {
            var emp = new Employee("John", "Doe", new DateTime(1990, 1, 1), new DateTime(2015, 6, 1), 2500);
            Assert.AreEqual("John", emp.Name);
            Assert.AreEqual("Doe", emp.Surname);
            Assert.AreEqual(2500, emp.Salary);
        }

        [Test]
        public void Constructor_FutureBirthDate_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Employee("Jane", "Doe", DateTime.Now.AddYears(1), DateTime.Now, 1000));
        }

        [Test]
        public void Constructor_NegativeSalary_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Employee("Sam", "Smith", new DateTime(1995, 5, 5), new DateTime(2020, 1, 1), -100));
        }

        [Test]
        public void SaveAndLoad_ShouldPersistEmployees()
        {
            var emp = new Employee("John", "Doe", new DateTime(1990, 1, 1), new DateTime(2015, 6, 1), 2500);
            Employee.Save(FilePath);
            Employee.Load(FilePath);

            Assert.IsTrue(Employee.Employees.Count > 0);
            Assert.AreEqual("John", Employee.Employees[0].Name);
        }
    }
}