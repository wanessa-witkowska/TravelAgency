using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Windows.Input;
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

        private ICommand? _back;
        private ICommand? _save;

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
            get => _customer?.FirstName ?? string.Empty;  // Zabezpieczenie przed null
            set
            {
                if (_customer != null)
                {
                    _customer.FirstName = value;
                    OnPropertyChanged();
                }
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

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.CustomersSubView = new CustomersViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);

        private void SaveChanges(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            _context.Customers.Update(_customer);
            _context.SaveChanges();

            Response = "Customer details successfully updated";
        }

        private bool IsValid()
        {
            // Dodaj walidację, jeśli potrzebujesz
            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(PhoneNumber);
        }

        public string Response { get; set; }
    }
}
