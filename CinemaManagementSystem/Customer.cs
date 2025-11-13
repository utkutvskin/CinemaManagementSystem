using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace CinemaManagementSystem
{
    [Serializable]
    public class Customer
    {
        // ---------- Attributes ----------
        public string Name { get; set; }           
        public string Surname { get; set; }        
        public DateTime DateOfBirth { get; set; }  

        [XmlIgnore]
        public int Age
        {
            get
            {
                int age = DateTime.Now.Year - DateOfBirth.Year;
                if (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear)
                    age--;
                return age;
            }
        }

        // ---------- Class extent ----------
        private static List<Customer> _customers = new List<Customer>();
        public static IReadOnlyList<Customer> Customers => _customers.AsReadOnly();

        // ---------- Constructors ----------
        public Customer() { }

        public Customer(string name, string surname, DateTime dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Surname cannot be empty.");

            if (dateOfBirth > DateTime.Now)
                throw new ArgumentException("Date of birth cannot be in the future.");

            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;

            _customers.Add(this);
        }

        // ---------- Methods ----------
        public void BuyTicket(string movieTitle)
        {
            if (string.IsNullOrWhiteSpace(movieTitle))
                throw new ArgumentException("Movie title cannot be empty.");
            Console.WriteLine($"{Name} {Surname} bought a ticket for '{movieTitle}'.");
        }

        public void BuyItem(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                throw new ArgumentException("Item name cannot be empty.");
            Console.WriteLine($"{Name} {Surname} purchased '{itemName}'.");
        }

        public void RequestNewStampCard()
        {
            Console.WriteLine($"{Name} {Surname} requested a new stamp card.");
        }

        // ---------- Persistence ----------
        public static void Save(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, _customers);
            }
        }

        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Customer file not found.");

            XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>));
            using (StreamReader reader = new StreamReader(filePath))
            {
                var loaded = (List<Customer>)serializer.Deserialize(reader);
                _customers = loaded ?? new List<Customer>();
            }
        }

        public override string ToString()
        {
            return $"{Name} {Surname}, Age: {Age}";
        }
    }
}
