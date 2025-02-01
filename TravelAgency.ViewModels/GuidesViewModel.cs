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
    public class GuidesViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;

        public ObservableCollection<Guide> Guides { get; set; }

        public GuidesViewModel(
        travelAgencyContext context,
        IDialogService dialogService,
        IGuideService bookingService)
        {
            _context = new travelAgencyContext();
            Guides = new ObservableCollection<Guide>(_context.Guides);
        }
    }
}
