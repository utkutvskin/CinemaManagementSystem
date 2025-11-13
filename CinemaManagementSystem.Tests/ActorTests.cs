using System;
using System.IO;
using NUnit.Framework;
using CinemaManagementSystem;

namespace CinemaManagementSystem.Tests
{
    [TestFixture]
    public class ActorTests
    {
        private string filePath = "actors_test.xml";

        [SetUp]
        public void SetUp()
        {
            // Eski dosyayı temizle
            if (File.Exists(filePath))
                File.Delete(filePath);

            // Statik _actors listesini temizle
            var actorsField = typeof(Actor).GetField("_actors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var list = (System.Collections.IList)actorsField.GetValue(null);
            list.Clear();
        }

        [Test]
        public void Constructor_ValidData_ShouldCreateActor()
        {
            var actor = new Actor("John", "Doe", "Male", new DateTime(1990, 5, 10));

            Assert.AreEqual("John", actor.Name);
            Assert.AreEqual("Doe", actor.Surname);
            Assert.AreEqual("Male", actor.Gender);
            Assert.AreEqual(1990, actor.BirthDate.Year);
        }

        [Test]
        public void Constructor_EmptyName_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Actor("", "Doe", "Male", new DateTime(1990, 5, 10))
            );
        }

        [Test]
        public void Constructor_EmptySurname_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Actor("John", "", "Male", new DateTime(1990, 5, 10))
            );
        }

        [Test]
        public void Constructor_FutureBirthDate_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Actor("John", "Doe", "Male", DateTime.Now.AddYears(2))
            );
        }

        [Test]
        public void Age_ShouldCalculateCorrectly()
        {
            var birthDate = new DateTime(2000, 1, 1);
            var actor = new Actor("Jane", "Smith", "Female", birthDate);

            int expectedAge = DateTime.Now.Year - 2000;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                expectedAge--;

            Assert.AreEqual(expectedAge, actor.Age);
        }

        [Test]
        public void ToString_ShouldReturnFormattedString()
        {
            var actor = new Actor("John", "Doe", "Male", new DateTime(1990, 5, 10));
            string text = actor.ToString();
            StringAssert.Contains("John Doe", text);
            StringAssert.Contains("Male", text);
        }

        [Test]
        public void SaveAndLoad_ShouldPersistActors()
        {
            // Arrange
            new Actor("John", "Doe", "Male", new DateTime(1990, 5, 10));
            new Actor("Jane", "Smith", "Female", new DateTime(1985, 3, 20));

            Actor.Save(filePath);

            // Listeyi temizle (yükleme testine hazırlık)
            var actorsField = typeof(Actor).GetField("_actors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var list = (System.Collections.IList)actorsField.GetValue(null);
            list.Clear();

            // Act
            Actor.Load(filePath);

            // Assert
            var actors = Actor.Actors;
            Assert.AreEqual(2, actors.Count);
            Assert.AreEqual("John", actors[0].Name);
            Assert.AreEqual("Jane", actors[1].Name);
        }
    }
}
