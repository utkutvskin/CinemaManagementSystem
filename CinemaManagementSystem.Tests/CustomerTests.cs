using NUnit.Framework;
using System;
using System.IO;
using CinemaManagementSystem;

namespace CinemaManagementSystem.Tests
{
    [TestFixture]
    public class CustomerTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Customer)
                .GetField("_customers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new System.Collections.Generic.List<Customer>());
        }

        [Test]
        public void Constructor_ValidData_ShouldCreateCustomer()
        {
            var c = new Customer("Alice", "Smith", new DateTime(1995, 1, 1));
            Assert.That(c.Name, Is.EqualTo("Alice"));
        }

        [Test]
        public void Constructor_FutureDate_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Customer("Bob", "Smith", DateTime.Now.AddYears(1)));
        }

        [Test]
        public void BuyTicket_ShouldPrintMessage()
        {
            var c = new Customer("Alice", "Smith", new DateTime(1995, 1, 1));
            Assert.DoesNotThrow(() => c.BuyTicket("Titanic"));
        }

        [Test]
        public void SaveAndLoad_ShouldPersistCustomers()
        {
            var c = new Customer("Alice", "Smith", new DateTime(1995, 1, 1));
            string path = "customers_test.xml";
            Customer.Save(path);
            Assert.That(File.Exists(path));

            Customer.Load(path);
            Assert.That(Customer.Customers.Count, Is.EqualTo(1));
            File.Delete(path);
        }
    }
}