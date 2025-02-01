using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class AddCustomerViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                    {
                        return "First Name is required";
                    }
                }
                if (columnName == "LastName")
                {
                    if (string.IsNullOrEmpty(LastName))
                    {
                        return "Last Name is required";
                    }
                }
                if (columnName == "Email")
                {
                    if (string.IsNullOrEmpty(Email))
                    {
                        return "Email is required";
                    }
                }
                if (columnName == "PhoneNumber")
                {
                    if (string.IsNullOrEmpty(PhoneNumber))
                    {
                        return "Phone Number is required";
                    }
                }
                return string.Empty;
            }
        }

        private string _firstName = string.Empty;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get { return _response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        private ICommand? _back = null;
        public ICommand? Back
        {
            get
            {
                if (_back is null)
                {
                    _back = new RelayCommand<object>(NavigateBack);
                }
                return _back;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.CustomersSubView = new CustomersViewModel(_context, _dialogService);
            }
        }

        private ICommand? _save = null;
        public ICommand? Save
        {
            get
            {
                if (_save is null)
                {
                    _save = new RelayCommand<object>(SaveCustomer);
                }
                return _save;
            }
        }

        private void SaveCustomer(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            Customer customer = new Customer
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            Response = "Customer data saved successfully";
        }

        public AddCustomerViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "FirstName", "LastName", "Email", "PhoneNumber" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
