using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Models;
using TravelAgency.Data;
using TravelAgency.Interfaces;

namespace TravelAgency.ViewModels
{
    public class LocationsViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;

        public ObservableCollection<Location> Locations { get; set; }

        public LocationsViewModel(
        travelAgencyContext context,
        IDialogService dialogService,
        ILocationService bookingService)
        {
            _context = new travelAgencyContext();
            Locations = new ObservableCollection<Location>(_context.Locations);
        }
    }

}
