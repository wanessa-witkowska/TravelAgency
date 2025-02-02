using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditLocationViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private int _locationId;
        public int LocationId
        {
            get => _locationId;
            set
            {
                _locationId = value;
                OnPropertyChanged(nameof(LocationId));
                LoadLocation();
            }
        }

        private Location? _location;
        public Location? Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
                if (_location != null)
                {
                    Name = _location.Name;
                    Description = _location.Description;
                    Address = _location.Address;
                    PlaceType = _location.PlaceType;
                }
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PlaceType { get; set; }

        public string Response { get; set; }

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);
        private ICommand? _back;

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.LocationsSubView = new LocationsViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);
        private ICommand? _save;

        private void SaveChanges(object? obj)
        {
            if (Location == null)
            {
                Response = "No location selected";
                return;
            }

            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var existingLocation = _context.Locations.FirstOrDefault(l => l.Id == Location.Id);
            if (existingLocation != null)
            {
                existingLocation.Name = Name;
                existingLocation.Description = Description;
                existingLocation.Address = Address;
                existingLocation.PlaceType = PlaceType;
            }
            else
            {
                var newLocation = new Location
                {
                    Name = Name,
                    Description = Description,
                    Address = Address,
                    PlaceType = PlaceType
                };
                _context.Locations.Add(newLocation);
            }

            _context.SaveChanges();
            Response = "Location details successfully updated";
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Description) &&
                   !string.IsNullOrEmpty(Address) &&
                   !string.IsNullOrEmpty(PlaceType);
        }

        public EditLocationViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private void LoadLocation()
        {
            Location = _context.Locations.FirstOrDefault(l => l.Id == LocationId);
        }
    }
}
