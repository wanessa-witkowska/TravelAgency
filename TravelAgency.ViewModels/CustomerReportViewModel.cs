using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class CustomerReportViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;

        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private string _searchFirstName = string.Empty;
        public string SearchFirstName
        {
            get => _searchFirstName;
            set
            {
                _searchFirstName = value;
                OnPropertyChanged(nameof(SearchFirstName));
                FilterCustomers();
            }
        }

        private string _searchLastName = string.Empty;
        public string SearchLastName
        {
            get => _searchLastName;
            set
            {
                _searchLastName = value;
                OnPropertyChanged(nameof(SearchLastName));
                FilterCustomers();
            }
        }

        public CustomerReportViewModel(travelAgencyContext context)
        {
            _context = context;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            _context.Customers.Load();
            Customers = _context.Customers.Local.ToObservableCollection();
        }

        private void FilterCustomers()
        {
            var filteredCustomers = _context.Customers
                .Where(c => string.IsNullOrEmpty(SearchFirstName) || c.FirstName.Contains(SearchFirstName))
                .Where(c => string.IsNullOrEmpty(SearchLastName) || c.LastName.Contains(SearchLastName))
                .ToList();

            Customers = new ObservableCollection<Customer>(filteredCustomers);
        }
    }
}