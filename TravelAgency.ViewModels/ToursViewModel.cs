using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data;
using TravelAgency.Models;
using TravelAgency.Interfaces;

namespace TravelAgency.ViewModels
{
    public class ToursViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;

        public ObservableCollection<Tour> Tours { get; set; }

        public ToursViewModel(
        travelAgencyContext context,
        IDialogService dialogService,
        ITourService bookingService)
        {
            _context = new travelAgencyContext();
            Tours = new ObservableCollection<Tour>(_context.Tours);
        }
    }
}
