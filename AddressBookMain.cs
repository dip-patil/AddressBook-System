using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AddressBookSystem
{
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
            AddressBook addressBook = new AddressBook();

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

            // Create a new Contact
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
                Console.WriteLine($"{contact.FirstName} {contact.LastName}");
            }
        }
        public void EditContact(string firstName, string lastName)
        {
            Contact contact = contacts.Find(c => c.FirstName == firstName && c.LastName == lastName);

            if (contact != null)
            {
                Console.WriteLine("Enter new Address: ");
                contact.Address = Console.ReadLine();

                Console.WriteLine("Enter new City: ");
                contact.City = Console.ReadLine();

                Console.WriteLine("Enter new State: ");
                contact.State = Console.ReadLine();

                Console.WriteLine("Enter new Zip: ");
                contact.Zip = Console.ReadLine();

                Console.WriteLine("Enter new Phone Number: ");
                contact.PhoneNumber = Console.ReadLine();

                Console.WriteLine("Enter new Email: ");
                contact.Email = Console.ReadLine();

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
                // Remove the address book from the dictionary
                addressBooks.Remove(name);
                Console.WriteLine($"Address book '{name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Address book '{name}' not found.");
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

            //Edit,delete a contact
        static void ManageAddressBook(AddressBook selectedBook)
            {
                bool keepRunning = true;

                while (keepRunning)
                {

                    Console.WriteLine("Choose an option:");

                    Console.WriteLine("Enter 1 to add a contact: ");
                    Console.WriteLine("Enter 2 to edit a contact: ");

                    Console.WriteLine("Enter 3 to delete a contact: ");
                    Console.WriteLine("Enter 4 to display contacts: ");
                    Console.WriteLine("Enter 5 to exit: ");
                    char c = Convert.ToChar(Console.ReadLine());
                    switch (c)
                    {
                        case '1':
                            selectedBook.AddContact();
                            break;
                        case '2':
                            Console.WriteLine("Enter the first name of the contact to edit: ");
                            string editFirstName = Console.ReadLine();
                            Console.WriteLine("Enter the last name of the contact to edit: ");
                            string editLastName = Console.ReadLine();
                            selectedBook.EditContact(editFirstName, editLastName);
                            break;

                        case '3':
                            Console.WriteLine("Enter the first name of the contact to delete: ");
                            string deleteFirstName = Console.ReadLine();
                            Console.WriteLine("Enter the last name of the contact to delete: ");
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
                            Console.WriteLine("Invalid input");
                            break;

                    }
                }



                

                Console.ReadLine();
            }
        
    }
}
