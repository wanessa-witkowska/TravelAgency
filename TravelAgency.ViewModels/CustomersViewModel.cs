using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; }
        }

        private ObservableCollection<Customer>? _customers = null;
        public ObservableCollection<Customer>? Customers
        {
            get
            {
                if (_customers is null)
                {
                    _customers = new ObservableCollection<Customer>();
                    return _customers;
                }
                return _customers;
            }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private ICommand? _add = null;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewCustomer);
                }
                return _add;
            }
        }

        private void AddNewCustomer(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.CustomersSubView = new AddCustomerViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditCustomer);
                }
                return _edit;
            }
        }

        private void EditCustomer(object? obj)
        {
            if (obj is not null)
            {
                int customerId = (int)obj;
                EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel(_context, _dialogService)
                {
                    CustomerId = customerId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.CustomersSubView = editCustomerViewModel;
                }
            }
        }

        private ICommand? _remove = null;
        public ICommand? Remove
        {
            get
            {
                if (_remove is null)
                {
                    _remove = new RelayCommand<object>(RemoveCustomer);
                }
                return _remove;
            }
        }

        private void RemoveCustomer(object? obj)
        {
            if (obj is not null)
            {
                int customerId = (int)obj;
                Customer? customer = _context.Customers.Find(customerId);
                if (customer is not null)
                {
                    DialogResult = _dialogService.Show("Do you want to remove the customer " + customer.FirstName + " " + customer.LastName + "?");
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.Customers.Remove(customer);
                    _context.SaveChanges();
                }
            }
        }

        public CustomersViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Customers.Load();
            Customers = _context.Customers.Local.ToObservableCollection();
        }
    }
}
