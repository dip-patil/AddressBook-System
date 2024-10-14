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

        
        public override bool Equals(object obj)
        {
            if (obj is Contact otherContact)
            {
                return FirstName.Equals(otherContact.FirstName, StringComparison.OrdinalIgnoreCase) &&
                       LastName.Equals(otherContact.LastName, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        
        public override string ToString()
        {
            return $"{FirstName} {LastName}, {Address}, {City}, {State}, {Zip}, {PhoneNumber}, {Email}";
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
                FirstName = Contact.PromptForValidInput("First Name", @"^[A-Za-z]{2,}$", "Invalid first name!"),
                LastName = Contact.PromptForValidInput("Last Name", @"^[A-Za-z]{2,}$", "Invalid last name!"),
                Address = Contact.PromptForValidInput("Address", @"^.+$", "Invalid address!"),
                City = Contact.PromptForValidInput("City", @"^[A-Za-z ]+$", "Invalid city name!"),
                State = Contact.PromptForValidInput("State", @"^[A-Za-z ]+$", "Invalid state name!"),
                Zip = Contact.PromptForValidInput("Zip", @"^\d{6}$", "Invalid zip code!"),
                PhoneNumber = Contact.PromptForValidInput("Phone Number", @"(^\d{10}$)|(^\+[0-9]{2}[0-9]{10}$)", "Invalid phone number!"),
                Email = Contact.PromptForValidInput("Email", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "Invalid email address!")
            };

            if (contact.Validate())
            {
                if (contacts.Contains(contact))
                {
                    Console.WriteLine("Duplicate contact found! This person already exists in the address book.");
                }
                else
                {
                    contacts.Add(contact);
                    Console.WriteLine("Contact added successfully.");
                }
            }
            else
            {
                Console.WriteLine("Failed to add contact due to validation errors.");
            }
        }

        
        public void DisplayContacts()
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts available.");
            }
            else
            {
                foreach (var contact in contacts)
                {
                    Console.WriteLine(contact);
                }
            }
        }

        
        public void EditContact(string firstName, string lastName)
        {
            Contact contact = contacts.Find(c => c.FirstName == firstName && c.LastName == lastName);
            if (contact != null)
            {
                Console.WriteLine("Edit contact details:");

                contact.Address = Contact.PromptForValidInput("Address", @"^.+$", "Invalid address!");
                contact.City = Contact.PromptForValidInput("City", @"^[A-Za-z ]+$", "Invalid city name!");
                contact.State = Contact.PromptForValidInput("State", @"^[A-Za-z ]+$", "Invalid state name!");
                contact.Zip = Contact.PromptForValidInput("Zip", @"^\d{6}$", "Invalid zip code!");
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

    
    public class AddressBookManager
    {
        private Dictionary<string, AddressBook> addressBooks;

        public AddressBookManager()
        {
            addressBooks = new Dictionary<string, AddressBook>();
        }

        
        public void AddAddressBook(string name)
        {
            if (!addressBooks.ContainsKey(name))
            {
                addressBooks[name] = new AddressBook();
                Console.WriteLine($"Address book '{name}' created.");
            }
            else
            {
                Console.WriteLine("Address book with this name already exists.");
            }
        }

        
        public AddressBook GetAddressBook(string name)
        {
            if (addressBooks.ContainsKey(name))
            {
                return addressBooks[name];
            }

            Console.WriteLine("Address book not found.");
            return null;
        }

        
        public void DisplayAddressBooks()
        {
            Console.WriteLine("Available Address Books:");
            foreach (var book in addressBooks.Keys)
            {
                Console.WriteLine($"- {book}");
            }
        }

        
        public void DeleteAddressBook(string name)
        {
            if (addressBooks.ContainsKey(name))
            {
                addressBooks.Remove(name);
                Console.WriteLine($"Address book '{name}' deleted.");
            }
            else
            {
                Console.WriteLine("Address book not found.");
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

            AddressBookManager addressBookManager = new AddressBookManager();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Add Address Book");
                Console.WriteLine("2. Select Address Book");
                Console.WriteLine("3. Display Address Books");
                Console.WriteLine("4. Delete Address Book");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                char option = Convert.ToChar(Console.ReadLine());

                switch (option)
                {
                    case '1':
                        Console.Write("Enter name for the new Address Book: ");
                        string bookName = Console.ReadLine();
                        addressBookManager.AddAddressBook(bookName);
                        break;

                    case '2':
                        Console.Write("Enter the name of the Address Book to select: ");
                        string selectedBookName = Console.ReadLine();
                        AddressBook selectedBook = addressBookManager.GetAddressBook(selectedBookName);
                        if (selectedBook != null)
                        {
                            ManageAddressBook(selectedBook);
                        }
                        break;

                    case '3':
                        addressBookManager.DisplayAddressBooks();
                        break;

                    case '4':
                        Console.Write("Enter the name of the Address Book to delete: ");
                        string deleteBookName = Console.ReadLine();
                        addressBookManager.DeleteAddressBook(deleteBookName);
                        break;

                    case '5':
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        
        static void ManageAddressBook(AddressBook selectedBook)
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("\nAddress Book Menu:");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. Edit Contact");
                Console.WriteLine("3. Delete Contact");
                Console.WriteLine("4. Display Contacts");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Choose an option: ");
                char option = Convert.ToChar(Console.ReadLine());

                switch (option)
                {
                    case '1':
                        selectedBook.AddContact();
                        break;

                    case '2':
                        Console.Write("Enter the first name of the contact to edit: ");
                        string editFirstName = Console.ReadLine();
                        Console.Write("Enter the last name of the contact to edit: ");
                        string editLastName = Console.ReadLine();
                        selectedBook.EditContact(editFirstName, editLastName);
                        break;

                    case '3':
                        Console.Write("Enter the first name of the contact to delete: ");
                        string deleteFirstName = Console.ReadLine();
                        Console.Write("Enter the last name of the contact to delete: ");
                        string deleteLastName = Console.ReadLine();
                        selectedBook.DeleteContact(deleteFirstName, deleteLastName);
                        break;

                    case '4':
                        selectedBook.DisplayContacts();
                        break;

                    case '5':
                        keepRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
