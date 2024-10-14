using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AddressBookSystem
{
    
    public class InvalidContactFieldException : Exception
    {
        public InvalidContactFieldException(string message) : base(message)
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

        
        public static string PromptForValidInput(string fieldName, string pattern, string errorMessage)
        {
            string input;
            do
            {
                try
                {
                    Console.Write($"Enter {fieldName}: ");
                    input = Console.ReadLine();

                    
                    if (!Regex.IsMatch(input, pattern))
                    {
                        throw new InvalidContactFieldException(errorMessage);
                    }

                    return input; 
                }
                catch (InvalidContactFieldException ex)
                {
                    
                    Console.WriteLine($"Error: {ex.Message}");
                }

            } while (true);
        }
    }

    public class AddressBook
    {
        private List<Contact> contacts;

        public AddressBook()
        {
            contacts = new List<Contact>();
        }

        public void AddContact(Contact contact)
        {
            contacts.Add(contact);
            Console.WriteLine("Contact added successfully.");
        }

        public void DisplayContacts()
        {
            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.FirstName} {contact.LastName}");
            }
        }
    }

    internal class AddressBookMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("Welcome to Address Book System");
            Console.WriteLine(new String('-', 50));
            AddressBook addressBook = new AddressBook();

            try
            {
               
                string firstName = Contact.PromptForValidInput(
                    "First Name", @"^[A-Za-z]{2,}$", "Invalid first name! It must be at least 2 characters long and contain only letters."
                );

                
                string lastName = Contact.PromptForValidInput(
                    "Last Name", @"^[A-Za-z]{2,}$", "Invalid last name! It must be at least 2 characters long and contain only letters."
                );

                
                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                
                Console.Write("Enter City: ");
                string city = Console.ReadLine();

                
                Console.Write("Enter State: ");
                string state = Console.ReadLine();

                
                string zip = Contact.PromptForValidInput(
                    "Zip", @"^\d{6}$", "Invalid zip code! It must be exactly 6 digits."
                );

                
                string phoneNumber = Contact.PromptForValidInput(
                    "Phone Number", @"(^\d{10}$)|(^\+[0-9]{2}[0-9]{10}$)", "Invalid phone number! It must be 10 digits or follow international format with a country code."
                );

                
                string email = Contact.PromptForValidInput(
                    "Email", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "Invalid email address! Please provide a valid email in the format example@domain.com."
                );

                
                Contact contact = new Contact()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Address = address,
                    City = city,
                    State = state,
                    Zip = zip,
                    PhoneNumber = phoneNumber,
                    Email = email
                };

                
                addressBook.AddContact(contact);
            }
            catch (InvalidContactFieldException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
