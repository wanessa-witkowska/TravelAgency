using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class ToursViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private ObservableCollection<Tour>? _tours = null;
        public ObservableCollection<Tour>? Tours
        {
            get
            {
                if (_tours is null)
                {
                    _tours = new ObservableCollection<Tour>();
                    return _tours;
                }
                return _tours;
            }
            set
            {
                _tours = value;
                OnPropertyChanged(nameof(Tours));
            }
        }

        // Dodaj kolekcję dostępnych lokalizacji
        private ObservableCollection<Location> _availableLocations = new ObservableCollection<Location>();
        public ObservableCollection<Location> AvailableLocations
        {
            get => _availableLocations;
            set
            {
                _availableLocations = value;
                OnPropertyChanged(nameof(AvailableLocations));
            }
        }

        // Komendy
        private ICommand? _add = null;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewTour);
                }
                return _add;
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditTour);
                }
                return _edit;
            }
        }

        private ICommand? _remove = null;
        public ICommand? Remove
        {
            get
            {
                if (_remove is null)
                {
                    _remove = new RelayCommand<object>(RemoveTour);
                }
                return _remove;
            }
        }

        // Konstruktor
        public ToursViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Tours.Load();
            Tours = _context.Tours.Local.ToObservableCollection();

            // Załaduj dostępne lokalizacje z bazy danych i utwórz kopię
            _context.Locations.Load();
            foreach (var location in _context.Locations.Local)
            {
                AvailableLocations.Add(new Location
                {
                    Id = location.Id,
                    Name = location.Name,
                    Description = location.Description,
                    Address = location.Address,
                    PlaceType = location.PlaceType
                });
            }
        }

        // Metody
        private void AddNewTour(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.ToursSubView = new AddTourViewModel(_context, _dialogService);
            }
        }

        private void EditTour(object? obj)
        {
            if (obj is not null)
            {
                int tourId = (int)obj;
                EditTourViewModel editTourViewModel = new EditTourViewModel(_context, _dialogService)
                {
                    TourId = tourId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.ToursSubView = editTourViewModel;
                }
            }
        }

        private void RemoveTour(object? obj)
        {
            if (obj is not null)
            {
                int tourId = (int)obj;
                Tour? tour = _context.Tours.Find(tourId);
                if (tour is not null)
                {
                    bool? dialogResult = _dialogService.Show("Do you want to remove the tour: " + tour.Name + "?");
                    if (dialogResult == false)
                    {
                        return;
                    }

                    _context.Tours.Remove(tour);
                    _context.SaveChanges();
                }
            }
        }
    }
}
