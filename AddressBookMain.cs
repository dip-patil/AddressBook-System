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

        public bool Validate()
        {
            if (!Regex.IsMatch(FirstName, @"^[A-Za-z]{2,}$"))
            {
                Console.WriteLine("Invalid first name!");
                return false;
            }

            if (!Regex.IsMatch(LastName, @"^[A-Za-z]{2,}$"))
            {
                Console.WriteLine("Invalid last name!");
                return false;
            }

            if (!Regex.IsMatch(Zip, @"^\d{6}$"))
            {
                Console.WriteLine("Invalid zip code!");
                return false;
            }

            if (!Regex.IsMatch(PhoneNumber, @"(^\d{10}$)|(^\+[0-9]{2}[0-9]{10}$)"))
            {
                Console.WriteLine("Invalid phone number!");
                return false;
            }

            if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                Console.WriteLine("Invalid email address!");
                return false;
            }

            return true;
        }
    }

    public class AddressBook
    {
        private List<Contact> contacts;

        public AddressBook()
        {
            contacts = new List<Contact>();
        }

        public void AddContact()
        {
            Contact contact = new Contact()
            {
                FirstName = Contact.PromptForValidInput("First Name", @"^[A-Za-z]{2,}$", "Invalid first name! It must be at least 2 characters long and contain only letters."),
                LastName = Contact.PromptForValidInput("Last Name", @"^[A-Za-z]{2,}$", "Invalid last name! It must be at least 2 characters long and contain only letters."),
                Address = Contact.PromptForValidInput("Address", @"^.+$", "Invalid address!"),
                City = Contact.PromptForValidInput("City", @"^[A-Za-z ]+$", "Invalid city name!"),
                State = Contact.PromptForValidInput("State", @"^[A-Za-z ]+$", "Invalid state name!"),
                Zip = Contact.PromptForValidInput("Zip", @"^\d{6}$", "Invalid zip code! It must be exactly 6 digits."),
                PhoneNumber = Contact.PromptForValidInput("Phone Number", @"(^\d{10}$)|(^\+[0-9]{2}[0-9]{10}$)", "Invalid phone number!"),
                Email = Contact.PromptForValidInput("Email", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "Invalid email address!")
            };

            if (contact.Validate())
            {
                contacts.Add(contact);
                Console.WriteLine("Contact added successfully.");
            }
            else
            {
                Console.WriteLine("Failed to add contact due to validation errors.");
            }
        }

        public void DisplayContacts()
        {
            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.FirstName} {contact.LastName}");
            }
        }

        public void EditContact(string firstName, string lastName)
        {
            Contact contact = contacts.Find(c => c.FirstName == firstName && c.LastName == lastName);

            if (contact != null)
            {
                contact.Address = Contact.PromptForValidInput("Address", @"^.+$", "Invalid address!");
                contact.City = Contact.PromptForValidInput("City", @"^[A-Za-z ]+$", "Invalid city name!");
                contact.State = Contact.PromptForValidInput("State", @"^[A-Za-z ]+$", "Invalid state name!");
                contact.Zip = Contact.PromptForValidInput("Zip", @"^\d{6}$", "Invalid zip code! It must be exactly 6 digits.");
                contact.PhoneNumber = Contact.PromptForValidInput("Phone Number", @"(^\d{10}$)|(^\+[0-9]{2}[0-9]{10}$)", "Invalid phone number!");
                contact.Email = Contact.PromptForValidInput("Email", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "Invalid email address!");

                Console.WriteLine("Contact updated successfully.");
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }

        public void DeleteContact(string firstName, string lastName)
        {
            Contact contact = contacts.Find(c => c.FirstName == firstName && c.LastName == lastName);

            if (contact != null)
            {
                contacts.Remove(contact);
                Console.WriteLine("Contact deleted successfully.");
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }
    }

    internal class AddressBookMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Welcome to Address Book System");
            Console.WriteLine(new string('-', 50));

            AddressBook addressBook = new AddressBook();

            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("Enter 1 to add a contact: ");
                Console.WriteLine("Enter 2 to edit a contact: ");
                Console.WriteLine("Enter 3 to delete a contact: ");
                Console.WriteLine("Enter 4 to exit: ");
                char c = Convert.ToChar(Console.ReadLine());

                switch (c)
                {
                    case '1':
                        addressBook.AddContact();
                        break;
                    case '2':
                        Console.WriteLine("Enter the first name of the contact to edit: ");
                        string editFirstName = Console.ReadLine();
                        Console.WriteLine("Enter the last name of the contact to edit: ");
                        string editLastName = Console.ReadLine();
                        addressBook.EditContact(editFirstName, editLastName);
                        break;
                    case '3':
                        Console.WriteLine("Enter the first name of the contact to delete: ");
                        string deleteFirstName = Console.ReadLine();
                        Console.WriteLine("Enter the last name of the contact to delete: ");
                        string deleteLastName = Console.ReadLine();
                        addressBook.DeleteContact(deleteFirstName, deleteLastName);
                        break;
                    case '4':
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }

            
            Console.ReadLine();
        }
    }
}
