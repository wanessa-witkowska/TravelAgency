using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditLocationViewModel : ObservableObject
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private int _locationId;
        public int LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId != value)
                {
                    _locationId = value;
                    OnPropertyChanged(nameof(LocationId));
                    LoadLocation();  // Załaduj lokalizację po zmianie ID
                }
            }
        }

        private Location? _location;
        public Location? Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                    if (_location != null)
                    {
                        // Ustaw dane w polach po załadowaniu lokalizacji
                        Name = _location.Name;
                        Description = _location.Description;
                        Address = _location.Address;
                        PlaceType = _location.PlaceType;
                    }
                }
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        private string _placeType = string.Empty;
        public string PlaceType
        {
            get => _placeType;
            set
            {
                if (_placeType != value)
                {
                    _placeType = value;
                    OnPropertyChanged(nameof(PlaceType));
                }
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set
            {
                if (_response != value)
                {
                    _response = value;
                    OnPropertyChanged(nameof(Response));
                }
            }
        }

        // Polecenia
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

                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(PlaceType));
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
            // Załaduj lokalizację po ID
            Location = _context.Locations.FirstOrDefault(l => l.Id == LocationId);
            if (Location == null)
            {
                Response = "Location not found"; // Informacja o błędzie, jeśli lokalizacja nie została znaleziona
            }
        }
    }
}
