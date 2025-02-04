using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
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

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

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
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            if (Customer == null)
            {
                Customer = new Customer();
                _context.Customers.Add(Customer);
            }
            else
            {
                var existingCustomer = _context.Customers.Find(Customer.Id);
                if (existingCustomer != null)
                {
                    existingCustomer.FirstName = FirstName;
                    existingCustomer.LastName = LastName;
                    existingCustomer.Email = Email;
                    existingCustomer.PhoneNumber = PhoneNumber;
                    _context.Entry(existingCustomer).State = EntityState.Modified;
                }
                else
                {
                    Customer.FirstName = FirstName;
                    Customer.LastName = LastName;
                    Customer.Email = Email;
                    Customer.PhoneNumber = PhoneNumber;
                    _context.Customers.Add(Customer);
                }
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
            Customer = _context.Customers.AsNoTracking().FirstOrDefault(c => c.Id == CustomerId);
            if (Customer != null)
            {
                FirstName = Customer.FirstName;
                LastName = Customer.LastName;
                Email = Customer.Email;
                PhoneNumber = Customer.PhoneNumber;
            }
        }
    }
}
