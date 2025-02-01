using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;

        public ObservableCollection<Customer> Customers { get; set; }

        public CustomersViewModel(
        travelAgencyContext context,
        IDialogService dialogService,
        ICustomerService bookingService)
        {
            _context = new travelAgencyContext();
            Customers = new ObservableCollection<Customer>(_context.Customers);
        }
    }
}
