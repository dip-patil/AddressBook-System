using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AddressBookSystem
{
    public class InvalidContactException : Exception
    {
        public InvalidContactException(string message) : base(message)
        {
        }
    }
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public bool Validate()
        {
           try { 

                if (!Regex.IsMatch(FirstName, @"^[A-Za-z]{2,}$"))
                {
                    throw new InvalidContactException("Invalid first name!");

                }

                if (!Regex.IsMatch(LastName, @"^[A-Za-z]{2,}$"))
                {
                    throw new InvalidContactException("Invalid last name!");

                }

                if (!Regex.IsMatch(Zip, @"^\d{5}$"))
                {
                    throw new InvalidContactException("Invalid zip code!");

                }

                if (!Regex.IsMatch(PhoneNumber, @"(^\d{10}$)|(^\+[0-9]{2}[0-9]{10}$)"))
                {
                    throw new InvalidContactException("Invalid phone number!");

                }

                if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    throw new InvalidContactException("Invalid email address!");

                }

                return true;
             }
            catch (InvalidContactException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }

        }
    }
    public class AddressBook
    {

    }
    internal class AddressBookMain
    {

        static void Main(string[] args)
        {
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("Welcome to Address Book System");
            Console.WriteLine(new String('-', 50));
            Console.ReadLine();
        }
    }
}