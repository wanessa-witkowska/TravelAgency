using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class AddLocationViewModel : ViewModelBase
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
                if (columnName == "Name" && string.IsNullOrEmpty(Name))
                {
                    return "Name is required";
                }
                if (columnName == "Description" && string.IsNullOrEmpty(Description))
                {
                    return "Description is required";
                }
                if (columnName == "Address" && string.IsNullOrEmpty(Address))
                {
                    return "Address is required";
                }
                if (columnName == "PlaceType" && string.IsNullOrEmpty(PlaceType))
                {
                    return "Place Type is required";
                }
                return string.Empty;
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private string _placeType = string.Empty;
        public string PlaceType
        {
            get { return _placeType; }
            set
            {
                _placeType = value;
                OnPropertyChanged(nameof(PlaceType));
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

        private ICommand? _backCommand = null;
        public ICommand? BackCommand
        {
            get
            {
                if (_backCommand is null)
                {
                    _backCommand = new RelayCommand<object>(NavigateBack);
                }
                return _backCommand;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.LocationsSubView = new LocationsViewModel(_context, _dialogService);
            }
        }

        private ICommand? _saveCommand = null;
        public ICommand? SaveCommand
        {
            get
            {
                if (_saveCommand is null)
                {
                    _saveCommand = new RelayCommand<object>(SaveLocation);
                }
                return _saveCommand;
            }
        }

        private void SaveLocation(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            Location location = new Location
            {
                Name = this.Name,
                Description = this.Description,
                Address = this.Address,
                PlaceType = this.PlaceType
            };

            _context.Locations.Add(location);
            _context.SaveChanges();

            Response = "Location data saved successfully";
        }

        public AddLocationViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "Name", "Description", "Address", "PlaceType" };
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