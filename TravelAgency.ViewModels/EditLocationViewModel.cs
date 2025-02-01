using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditLocationViewModel : ViewModelBase
    {
        private Location _location;
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        public int LocationId { get; set; }

        public EditLocationViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        public EditLocationViewModel(Location location)
        {
            _location = location;
            Name = location.Name;
            Description = location.Description;
            Address = location.Address;
            PlaceType = location.PlaceType;
        }

        public string Name
        {
            get => _location.Name;
            set
            {
                _location.Name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _location.Description;
            set
            {
                _location.Description = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _location.Address;
            set
            {
                _location.Address = value;
                OnPropertyChanged();
            }
        }

        public string PlaceType
        {
            get => _location.PlaceType;
            set
            {
                _location.PlaceType = value;
                OnPropertyChanged();
            }
        }

        public void SaveChanges()
        {
            using (var context = new travelAgencyContext())
            {
                context.Locations.Update(_location);
                context.SaveChanges();
            }
        }
    }
}
