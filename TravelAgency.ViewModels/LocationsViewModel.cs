using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class LocationsViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get => _dialogResult;
            set => _dialogResult = value;
        }

        private ObservableCollection<Location>? _locations = null;
        public ObservableCollection<Location>? Locations
        {
            get
            {
                if (_locations is null)
                {
                    _locations = new ObservableCollection<Location>();
                    return _locations;
                }
                return _locations;
            }
            set
            {
                _locations = value;
                OnPropertyChanged(nameof(Locations));
            }
        }

        private ICommand? _add = null;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewLocation);
                }
                return _add;
            }
        }

        private void AddNewLocation(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.LocationsSubView = new AddLocationViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditLocation);
                }
                return _edit;
            }
        }

        private void EditLocation(object? obj)
        {
            if (obj is not null)
            {
                int locationId = (int)obj;
                EditLocationViewModel editLocationViewModel = new EditLocationViewModel(_context, _dialogService)
                {
                    LocationId = locationId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.LocationsSubView = editLocationViewModel;
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
                    _remove = new RelayCommand<object>(RemoveLocation);
                }
                return _remove;
            }
        }

        private void RemoveLocation(object? obj)
        {
            if (obj is not null)
            {
                int locationId = (int)obj;
                Location? location = _context.Locations.Find(locationId);
                if (location is not null)
                {
                    DialogResult = _dialogService.Show($"Do you want to remove the location {location.Name}?");
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.Locations.Remove(location);
                    _context.SaveChanges();
                }
            }
        }

        public LocationsViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Locations.Load();
            Locations = _context.Locations.Local.ToObservableCollection();
        }
    }
}
