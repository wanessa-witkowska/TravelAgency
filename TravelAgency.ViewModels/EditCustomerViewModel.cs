using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditCustomerViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private int _customerId;
        public int CustomerId
        {
            get => _customerId;
            set
            {
                _customerId = value;
                OnPropertyChanged(nameof(CustomerId));
                LoadCustomer();
            }
        }

        private Customer? _customer;
        public Customer? Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
                if (_customer != null)
                {
                    FirstName = _customer.FirstName;
                    LastName = _customer.LastName;
                    Email = _customer.Email;
                    PhoneNumber = _customer.PhoneNumber;
                }
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Response { get; set; }

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);
        private ICommand? _back;

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.CustomersSubView = new CustomersViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);
        private ICommand? _save;

        private void SaveChanges(object? obj)
        {
            if (Customer == null)
            {
                Response = "No customer selected";
                return;
            }

            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Id == Customer.Id);
            if (existingCustomer != null)
            {
                existingCustomer.FirstName = FirstName;
                existingCustomer.LastName = LastName;
                existingCustomer.Email = Email;
                existingCustomer.PhoneNumber = PhoneNumber;
            }
            else
            {
                var newCustomer = new Customer
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    PhoneNumber = PhoneNumber
                };
                _context.Customers.Add(newCustomer);
            }

            _context.SaveChanges();
            Response = "Customer details successfully updated";
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(PhoneNumber);
        }

        public EditCustomerViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private void LoadCustomer()
        {
            Customer = _context.Customers.FirstOrDefault(c => c.Id == CustomerId);
        }
    }
}
