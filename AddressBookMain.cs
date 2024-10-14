using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (var contact in contacts)
            {
                Console.WriteLine(contact.ToString());
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

        public List<Contact> GetContactsByCity(string city)
        {
            return contacts.Where(c => c.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Contact> GetContactsByState(string state)
        {
            return contacts.Where(c => c.State.Equals(state, StringComparison.OrdinalIgnoreCase)).ToList();
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
                Console.WriteLine($"Address book '{name}' added.");
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
            foreach (var book in addressBooks)
            {
                Console.WriteLine($"- {book.Key}");
            }
        }

        public void DeleteAddressBook(string name)
        {
            if (addressBooks.ContainsKey(name))
            {
                addressBooks.Remove(name);
                Console.WriteLine($"Address book '{name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Address book '{name}' not found.");
            }
        }

        public List<Contact> SearchContactsByCityOrState(string location, bool isCity)
        {
            List<Contact> matchingContacts = new List<Contact>();

            foreach (var addressBook in addressBooks.Values)
            {
                if (isCity)
                {
                    matchingContacts.AddRange(addressBook.GetContactsByCity(location));
                }
                else
                {
                    matchingContacts.AddRange(addressBook.GetContactsByState(location));
                }
            }

            return matchingContacts;
        }
    }

    internal class AddressBookMain
    {
        static void SearchContacts(AddressBookManager manager)
        {
            Console.WriteLine("Search by:\n1. City\n2. State");
            int searchOption = int.Parse(Console.ReadLine());

            Console.Write("Enter the name of the City or State: ");
            string location = Console.ReadLine();

            bool isCity = (searchOption == 1);

            List<Contact> results = manager.SearchContactsByCityOrState(location, isCity);

            if (results.Count > 0)
            {
                Console.WriteLine($"Contacts found in {location}:");
                foreach (var contact in results)
                {
                    Console.WriteLine(contact.ToString());
                }
            }
            else
            {
                Console.WriteLine("No contacts found.");
            }
        }

        static void Main(string[] args)
        {
            AddressBookManager manager = new AddressBookManager();

            while (true)
            {
                Console.WriteLine("\n--- Address Book System ---");
                Console.WriteLine("1. Add Address Book");
                Console.WriteLine("2. Display Address Books");
                Console.WriteLine("3. Select Address Book ");
                Console.WriteLine("4. Delete Address Book");
                Console.WriteLine("5. Search Contacts");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Write("Enter the name of the Address Book: ");
                        string addressBookName = Console.ReadLine();
                        manager.AddAddressBook(addressBookName);
                        break;

                    case 2:
                        manager.DisplayAddressBooks();
                        break;

                    case 3:
                        Console.Write("Enter the name of the Address Book: ");
                        string bookName = Console.ReadLine();
                        AddressBook addressBook = manager.GetAddressBook(bookName);
                        bool exitBookOperations = false;
                        if (addressBook != null)
                        {
                            while (!exitBookOperations)
                            {
                                Console.WriteLine("\n--- Address Book Operations ---");
                                Console.WriteLine("1. Add Contact");
                                Console.WriteLine("2. Display Contacts");
                                Console.WriteLine("3. Edit Contact");
                                Console.WriteLine("4. Delete Contact");
                                Console.WriteLine("5. Go Back");
                                Console.Write("Select an option: ");
                                int bookOption = int.Parse(Console.ReadLine());

                                switch (bookOption)
                                {
                                    case 1:
                                        addressBook.AddContact();
                                        break;

                                    case 2:
                                        addressBook.DisplayContacts();
                                        break;

                                    case 3:
                                        Console.Write("Enter the First Name of the contact to edit: ");
                                        string editFirstName = Console.ReadLine();
                                        Console.Write("Enter the Last Name of the contact to edit: ");
                                        string editLastName = Console.ReadLine();
                                        addressBook.EditContact(editFirstName, editLastName);
                                        break;

                                    case 4:
                                        Console.Write("Enter the First Name of the contact to delete: ");
                                        string deleteFirstName = Console.ReadLine();
                                        Console.Write("Enter the Last Name of the contact to delete: ");
                                        string deleteLastName = Console.ReadLine();
                                        addressBook.DeleteContact(deleteFirstName, deleteLastName);
                                        break;

                                    case 5:
                                        exitBookOperations = true;
                                        Console.WriteLine("Going back to main menu...");
                                        break; 

                                    default:
                                        Console.WriteLine("Invalid option. Please try again.");
                                        break;
                                }
                            }
                        }
                        break;

                    case 4:
                        Console.Write("Enter the name of the Address Book to delete: ");
                        string deleteBookName = Console.ReadLine();
                        manager.DeleteAddressBook(deleteBookName);
                        break;

                    case 5:
                        SearchContacts(manager);
                        break;

                    case 6:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
