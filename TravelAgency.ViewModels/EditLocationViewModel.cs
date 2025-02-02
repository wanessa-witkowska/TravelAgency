using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections;

namespace TravelAgency.ViewModels
{
    public class EditLocationViewModel : ViewModelBase
    {
        private Location _location;
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private ICommand? _back;
        private ICommand? _save;

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

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.LocationsSubView = new LocationsViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);

        private void SaveChanges(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            _context.Locations.Update(_location);
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

        public string Response { get; set; }
    }
}
