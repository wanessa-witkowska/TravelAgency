using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TravelAgency.Models;
using TravelAgency.Interfaces;
using TravelAgency.Data;

namespace TravelAgency.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly travelAgencyContext _context;

        public CustomerService(travelAgencyContext context)
        {
            _context = context;
        }

        public void AddCustomer(string firstName, string lastName, string email, string phoneNumber)
        {
            var newCustomer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            ValidateCustomer(newCustomer);

            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
        }

        public void EditCustomer(Customer customer, string firstName, string lastName, string email, string phoneNumber)
        {
            if (!_context.Customers.Any(c => c.Id == customer.Id))
                throw new ArgumentException("Podany klient nie istnieje.");

            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.Email = email;
            customer.PhoneNumber = phoneNumber;

            ValidateCustomer(customer);

            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                throw new ArgumentException("Podany klient nie istnieje.");

            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        private void ValidateCustomer(Customer customer)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(customer);

            if (!Validator.TryValidateObject(customer, validationContext, validationResults, true))
                throw new ValidationException(string.Join("; ", validationResults.Select(vr => vr.ErrorMessage)));

            if (_context.Customers.Any(c => c.Email == customer.Email && c.Id != customer.Id))
                throw new ValidationException("Podany adres email jest już używany przez innego klienta.");
        }
    }
}
