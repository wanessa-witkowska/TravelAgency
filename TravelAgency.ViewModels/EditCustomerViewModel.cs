using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditCustomerViewModel : ViewModelBase
    {
        private Customer _customer;
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        public int CustomerId { get; set; }

        public EditCustomerViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }


        public EditCustomerViewModel(Customer customer)
        {
            _customer = customer;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
            PhoneNumber = customer.PhoneNumber;
        }

        public string FirstName
        {
            get => _customer.FirstName;
            set
            {
                _customer.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _customer.LastName;
            set
            {
                _customer.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _customer.Email;
            set
            {
                _customer.Email = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _customer.PhoneNumber;
            set
            {
                _customer.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        public void SaveChanges()
        {
            using (var context = new travelAgencyContext())
            {
                context.Customers.Update(_customer);
                context.SaveChanges();
            }
        }
    }

}
