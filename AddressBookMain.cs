using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AddressBookSystem
{
    // Custom exception class for validation errors
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
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

        public void Validate()
        {
            if (!Regex.IsMatch(FirstName, @"^[A-Za-z]{2,}$"))
                throw new ValidationException("Invalid first name!");

            if (!Regex.IsMatch(LastName, @"^[A-Za-z]{2,}$"))
                throw new ValidationException("Invalid last name!");

            if (!Regex.IsMatch(Zip, @"^\d{6}$"))
                throw new ValidationException("Invalid zip code!");

            if (!Regex.IsMatch(PhoneNumber, @"(^\d{10}$)|(^\+[0-9]{2}[0-9]{10}$)"))
                throw new ValidationException("Invalid phone number!");

            if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                throw new ValidationException("Invalid email address!");
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
            Contact contact = PromptForContactDetails();

            try
            {
                contact.Validate();
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
            catch (ValidationException ex)
            {
                Console.WriteLine($"Failed to add contact: {ex.Message}");
            }
        }

        public void DisplayContacts()
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts available.");
                return;
            }

            foreach (var contact in contacts)
            {
                Console.WriteLine(contact);
            }
        }

        public void EditContact(string firstName, string lastName)
        {
            Contact contact = contacts.Find(c => c.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) && c.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

            if (contact != null)
            {
                Console.WriteLine("Enter new details for the contact:");
                Contact updatedContact = PromptForContactDetails();

                try
                {
                    updatedContact.Validate();
                    contact.Address = updatedContact.Address;
                    contact.City = updatedContact.City;
                    contact.State = updatedContact.State;
                    contact.Zip = updatedContact.Zip;
                    contact.PhoneNumber = updatedContact.PhoneNumber;
                    contact.Email = updatedContact.Email;

                    Console.WriteLine("Contact updated successfully.");
                }
                catch (ValidationException ex)
                {
                    Console.WriteLine($"Failed to update contact: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }

        public void DeleteContact(string firstName, string lastName)
        {
            Contact contact = contacts.Find(c => c.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) && c.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

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

        private Contact PromptForContactDetails()
        {
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter City: ");
            string city = Console.ReadLine();

            Console.Write("Enter State: ");
            string state = Console.ReadLine();

            Console.Write("Enter Zip: ");
            string zip = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            return new Contact()
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
            if (addressBooks.TryGetValue(name, out var addressBook))
            {
                return addressBook;
            }

            Console.WriteLine("Address book not found.");
            return null;
        }

        public void DisplayAddressBooks()
        {
            Console.WriteLine("Available Address Books:");
            if (addressBooks.Count == 0)
            {
                Console.WriteLine("No address books available.");
                return;
            }

            foreach (var book in addressBooks.Keys)
            {
                Console.WriteLine($"- {book}");
            }
        }

        public void DeleteAddressBook(string name)
        {
            if (addressBooks.Remove(name))
            {
                Console.WriteLine($"Address book '{name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Address book '{name}' not found.");
            }
        }

        public List<Contact> SearchContactsByCityOrState(string location, bool isCity)
        {
            return addressBooks.Values.SelectMany(addressBook => isCity ? addressBook.GetContactsByCity(location) : addressBook.GetContactsByState(location)).ToList();
        }
    }

    internal class AddressBookMain
    {
        static void SearchContacts(AddressBookManager manager)
        {
            Console.WriteLine("Search by:\n1. City\n2. State");
            if (!int.TryParse(Console.ReadLine(), out int searchOption) || (searchOption < 1 || searchOption > 2))
            {
                Console.WriteLine("Invalid option. Please try again.");
                return;
            }

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
                Console.WriteLine($"No contacts found in {location}.");
            }
        }

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
                Console.WriteLine("5. Search by City or State");
                Console.WriteLine("6. Exit");
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
                        SearchContacts(addressBookManager);
                        break;

                    case '6':
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
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Add a contact");
                Console.WriteLine("2. Edit a contact");
                Console.WriteLine("3. Delete a contact");
                Console.WriteLine("4. Display contacts");
                Console.WriteLine("5. Exit");

                if (!char.TryParse(Console.ReadLine(), out char c))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                switch (c)
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
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }
    }
}
