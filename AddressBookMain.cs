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
    internal class AddressBookMain
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("Welcome to Address Book System");
            Console.WriteLine(new String('-', 50));

            AddressBook addressBook = new AddressBook();


            //Edit,delete a contact
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
            


            addressBook.DisplayContacts();

            Console.ReadLine(); 
        }
    }
}
