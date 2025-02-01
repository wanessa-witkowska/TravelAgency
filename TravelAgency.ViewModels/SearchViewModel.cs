using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                OnPropertyChanged(nameof(DialogResult));
            }
        }

        private string _firstCondition = string.Empty;
        public string FirstCondition
        {
            get => _firstCondition;
            set
            {
                _firstCondition = value;
                OnPropertyChanged(nameof(FirstCondition));
            }
        }

        private string _secondCondition = string.Empty;
        public string SecondCondition
        {
            get => _secondCondition;
            set
            {
                _secondCondition = value;
                OnPropertyChanged(nameof(SecondCondition));
            }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private bool _areCustomersVisible;
        public bool AreCustomersVisible
        {
            get => _areCustomersVisible;
            set
            {
                _areCustomersVisible = value;
                OnPropertyChanged(nameof(AreCustomersVisible));
            }
        }

        private bool _areGuidesVisible;
        public bool AreGuidesVisible
        {
            get => _areGuidesVisible;
            set
            {
                _areGuidesVisible = value;
                OnPropertyChanged(nameof(AreGuidesVisible));
            }
        }

        private bool _areToursVisible;
        public bool AreToursVisible
        {
            get => _areToursVisible;
            set
            {
                _areToursVisible = value;
                OnPropertyChanged(nameof(AreToursVisible));
            }
        }

        private bool _areLocationsVisible;
        public bool AreLocationsVisible
        {
            get => _areLocationsVisible;
            set
            {
                _areLocationsVisible = value;
                OnPropertyChanged(nameof(AreLocationsVisible));
            }
        }

        private bool _areBookingsVisible;
        public bool AreBookingsVisible
        {
            get => _areBookingsVisible;
            set
            {
                _areBookingsVisible = value;
                OnPropertyChanged(nameof(AreBookingsVisible));
            }
        }

        private ObservableCollection<Customer>? _customers = null;
        public ObservableCollection<Customer>? Customers
        {
            get
            {
                if (_customers is null)
                {
                    _customers = new ObservableCollection<Customer>();
                }
                return _customers;
            }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private ObservableCollection<Guide>? _guides = null;
        public ObservableCollection<Guide>? Guides
        {
            get
            {
                if (_guides is null)
                {
                    _guides = new ObservableCollection<Guide>();
                }
                return _guides;
            }
            set
            {
                _guides = value;
                OnPropertyChanged(nameof(Guides));
            }
        }

        private ObservableCollection<Tour>? _tours = null;
        public ObservableCollection<Tour>? Tours
        {
            get
            {
                if (_tours is null)
                {
                    _tours = new ObservableCollection<Tour>();
                }
                return _tours;
            }
            set
            {
                _tours = value;
                OnPropertyChanged(nameof(Tours));
            }
        }

        private ObservableCollection<Location>? _locations = null;
        public ObservableCollection<Location>? Locations
        {
            get
            {
                if (_locations is null)
                {
                    _locations = new ObservableCollection<Location>();
                }
                return _locations;
            }
            set
            {
                _locations = value;
                OnPropertyChanged(nameof(Locations));
            }
        }

        private ObservableCollection<Booking>? _bookings = null;
        public ObservableCollection<Booking>? Bookings
        {
            get
            {
                if (_bookings is null)
                {
                    _bookings = new ObservableCollection<Booking>();
                }
                return _bookings;
            }
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        private ICommand? _comboBoxSelectionChanged = null;
        public ICommand? ComboBoxSelectionChanged
        {
            get
            {
                if (_comboBoxSelectionChanged is null)
                {
                    _comboBoxSelectionChanged = new RelayCommand<object>(UpdateCondition);
                }
                return _comboBoxSelectionChanged;
            }
        }

        private void UpdateCondition(object? obj)
        {
            if (obj is string selectedValue)
            {
                IsVisible = true;
                SecondCondition = string.Empty;

                if (selectedValue == "Customers")
                {
                    FirstCondition = "whose name contains";
                }
                else if (selectedValue == "Guides")
                {
                    FirstCondition = "with specialization";
                }
                else if (selectedValue == "Tours")
                {
                    FirstCondition = "with destination";
                }
                else if (selectedValue == "Locations")
                {
                    FirstCondition = "located at";
                }
                else if (selectedValue == "Bookings")
                {
                    FirstCondition = "booked by Customer ID";
                }
            }
        }

        private ICommand? _search = null;
        public ICommand? Search
        {
            get
            {
                if (_search is null)
                {
                    _search = new RelayCommand<object>(SelectData);
                }
                return _search;
            }
        }

        private void SelectData(object? obj)
        {
            if (FirstCondition == "whose name contains")
            {
                _context.Database.EnsureCreated();
                var customers = _context.Customers
                    .Where(c => c.FirstName.Contains(SecondCondition) || c.LastName.Contains(SecondCondition))
                    .ToList();

                Customers = new ObservableCollection<Customer>(customers);
                AreCustomersVisible = true;
                AreGuidesVisible = false;
                AreToursVisible = false;
                AreLocationsVisible = false;
                AreBookingsVisible = false;
            }
            else if (FirstCondition == "with specialization")
            {
                _context.Database.EnsureCreated();
                var guides = _context.Guides
                    .Where(g => g.Specialization.Contains(SecondCondition))
                    .ToList();

                Guides = new ObservableCollection<Guide>(guides);
                AreCustomersVisible = false;
                AreGuidesVisible = true;
                AreToursVisible = false;
                AreLocationsVisible = false;
                AreBookingsVisible = false;
            }
            else if (FirstCondition == "with destination")
            {
                _context.Database.EnsureCreated();
                var tours = _context.Tours
                    .Where(t => t.Destination.Contains(SecondCondition))
                    .ToList();

                Tours = new ObservableCollection<Tour>(tours);
                AreCustomersVisible = false;
                AreGuidesVisible = false;
                AreToursVisible = true;
                AreLocationsVisible = false;
                AreBookingsVisible = false;
            }
            else if (FirstCondition == "located at")
            {
                _context.Database.EnsureCreated();
                var locations = _context.Locations
                    .Where(l => l.Address.Contains(SecondCondition))
                    .ToList();

                Locations = new ObservableCollection<Location>(locations);
                AreCustomersVisible = false;
                AreGuidesVisible = false;
                AreToursVisible = false;
                AreLocationsVisible = true;
                AreBookingsVisible = false;
            }
            else if (FirstCondition == "booked by Customer ID")
            {
                _context.Database.EnsureCreated();
                var bookings = _context.Bookings
                    .Where(b => b.CustomerId.ToString() == SecondCondition)
                    .Include(b => b.Customer)
                    .ToList();

                Bookings = new ObservableCollection<Booking>(bookings);
                AreCustomersVisible = false;
                AreGuidesVisible = false;
                AreToursVisible = false;
                AreLocationsVisible = false;
                AreBookingsVisible = true;
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditItem);
                }
                return _edit;
            }
        }

        private void EditItem(object? obj)
        {
            if (obj is not null)
            {
                if (FirstCondition == "whose name contains")
                {
                    int customerId = (int)obj;
                    EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel(_context, _dialogService)
                    {
                        CustomerId = customerId
                    };
                    var instance = MainWindowViewModel.Instance();
                    if (instance is not null)
                    {
                        instance.CustomersSubView = editCustomerViewModel;
                        instance.SelectedTab = 0;
                    }
                }
                else if (FirstCondition == "with specialization")
                {
                    int guideId = (int)obj;
                    EditGuideViewModel editGuideViewModel = new EditGuideViewModel(_context, _dialogService)
                    {
                        GuideId = guideId
                    };
                    var instance = MainWindowViewModel.Instance();
                    if (instance is not null)
                    {
                        instance.GuidesSubView = editGuideViewModel;
                        instance.SelectedTab = 2;
                    }
                }
                else if (FirstCondition == "with destination")
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
                        instance.SelectedTab = 1;
                    }
                }
                else if (FirstCondition == "located at")
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
                        instance.SelectedTab = 3;
                    }
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
                    _remove = new RelayCommand<object>(RemoveItem);
                }
                return _remove;
            }
        }

        private void RemoveItem(object? obj)
        {
            if (obj is not null)
            {
                if (FirstCondition == "whose name contains")
                {
                    int customerId = (int)obj;
                    var customer = _context.Customers.Find(customerId);
                    if (customer != null)
                    {
                        DialogResult = _dialogService.Show($"{customer.FirstName} {customer.LastName}");
                        if (DialogResult == false)
                        {
                            return;
                        }
                        _context.Customers.Remove(customer);
                        _context.SaveChanges();
                    }
                }
                else if (FirstCondition == "with specialization")
                {
                    int guideId = (int)obj;
                    var guide = _context.Guides.Find(guideId);
                    if (guide != null)
                    {
                        DialogResult = _dialogService.Show(guide.FirstName);
                        if (DialogResult == false)
                        {
                            return;
                        }
                        _context.Guides.Remove(guide);
                        _context.SaveChanges();
                    }
                }
                else if (FirstCondition == "with destination")
                {
                    int tourId = (int)obj;
                    var tour = _context.Tours.Find(tourId);
                    if (tour != null)
                    {
                        DialogResult = _dialogService.Show(tour.Name);
                        if (DialogResult == false)
                        {
                            return;
                        }
                        _context.Tours.Remove(tour);
                        _context.SaveChanges();
                    }
                }
                else if (FirstCondition == "located at")
                {
                    int locationId = (int)obj;
                    var location = _context.Locations.Find(locationId);
                    if (location != null)
                    {
                        DialogResult = _dialogService.Show(location.Name);
                        if (DialogResult == false)
                        {
                            return;
                        }
                        _context.Locations.Remove(location);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public SearchViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            IsVisible = false;
            AreCustomersVisible = false;
            AreGuidesVisible = false;
            AreToursVisible = false;
            AreLocationsVisible = false;
            AreBookingsVisible = false;
        }
    }

}
