using System;
using TravelAgency.Interfaces;
using TravelAgency.Data;

namespace TravelAgency.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly travelAgencyContext _context;
    private readonly IDialogService _dialogService;

    private int _selectedTab;
    public int SelectedTab
    {
        get
        {
            return _selectedTab;
        }
        set
        {
            _selectedTab = value;
            OnPropertyChanged(nameof(SelectedTab));
        }
    }

    private object? _bookingsSubView = null;
    public object? BookingsSubView
    {
        get
        {
            return _bookingsSubView;
        }
        set
        {
            _bookingsSubView = value;
            OnPropertyChanged(nameof(BookingsSubView));
        }
    }

    private object? _customersSubView = null;
    public object? CustomersSubView
    {
        get
        {
            return _customersSubView;
        }
        set
        {
            _customersSubView = value;
            OnPropertyChanged(nameof(CustomersSubView));
        }
    }

    private object? _guidesSubView = null;
    public object? GuidesSubView
    {
        get
        {
            return _guidesSubView;
        }
        set
        {
            _guidesSubView = value;
            OnPropertyChanged(nameof(GuidesSubView));
        }
    }

    private object? _locationsSubView = null;
    public object? LocationsSubView
    {
        get
        {
            return _locationsSubView;
        }
        set
        {
            _locationsSubView = value;
            OnPropertyChanged(nameof(LocationsSubView));
        }
    }

    private object? _toursSubView = null;
    public object? ToursSubView
    {
        get
        {
            return _toursSubView;
        }
        set
        {
            _toursSubView = value;
            OnPropertyChanged(nameof(ToursSubView));
        }
    }

    private object? _searchSubView = null;
    public object? SearchSubView
    {
        get
        {
            return _searchSubView;
        }
        set
        {
            _searchSubView = value;
            OnPropertyChanged(nameof(SearchSubView));
        }
    }

    private static MainWindowViewModel? _instance = null;
    public static MainWindowViewModel? Instance()
    {
        return _instance;
    }

    public MainWindowViewModel(
        travelAgencyContext context, 
        IDialogService dialogService,
        IBookingService bookingService,
        ICustomerService customerService,
        IGuideService guideService,
        ILocationService locationService,
        ITourService tourService)
    {
        _context = context;
        _dialogService = dialogService;

        if (_instance is null)
        {
            _instance = this;
        }
        
        BookingsSubView = new BookingsViewModel(_context, _dialogService);
        CustomersSubView = new CustomersViewModel(_context, _dialogService, customerService);
        GuidesSubView = new GuidesViewModel(_context, _dialogService, guideService);
        LocationsSubView = new LocationsViewModel(_context, _dialogService, locationService);
        ToursSubView = new ToursViewModel(_context, _dialogService, tourService);
        SearchSubView = new SearchViewModel(_context, _dialogService);
    }
}
