using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.ViewModels;

namespace TravelAgency.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private int _selectedTab;
        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }

        private object? _searchSubView;
        public object? SearchSubView
        {
            get => _searchSubView;
            set
            {
                _searchSubView = value;
                OnPropertyChanged(nameof(SearchSubView));
            }
        }

        private object? _bookingsSubView;
        public object? BookingsSubView
        {
            get => _bookingsSubView;
            set
            {
                _bookingsSubView = value;
                OnPropertyChanged(nameof(BookingsSubView));
            }
        }

        private object? _customersSubView;
        public object? CustomersSubView
        {
            get => _customersSubView;
            set
            {
                _customersSubView = value;
                OnPropertyChanged(nameof(CustomersSubView));
            }
        }

        private object? _guidesSubView;
        public object? GuidesSubView
        {
            get => _guidesSubView;
            set
            {
                _guidesSubView = value;
                OnPropertyChanged(nameof(GuidesSubView));
            }
        }

        private object? _locationsSubView;
        public object? LocationsSubView
        {
            get => _locationsSubView;
            set
            {
                _locationsSubView = value;
                OnPropertyChanged(nameof(LocationsSubView));
            }
        }

        private object? _toursSubView;
        public object? ToursSubView
        {
            get => _toursSubView;
            set
            {
                _toursSubView = value;
                OnPropertyChanged(nameof(ToursSubView));
            }
        }

        private object? _bookingReportSubView;
        public object? BookingReportSubView
        {
            get => _bookingReportSubView;
            set
            {
                _bookingReportSubView = value;
                OnPropertyChanged(nameof(BookingReportSubView));
            }
        }

        private object? _customerReportSubView;
        public object? CustomerReportSubView
        {
            get => _customerReportSubView;
            set
            {
                _customerReportSubView = value;
                OnPropertyChanged(nameof(CustomerReportSubView));
            }
        }

        private static MainWindowViewModel? _instance = null;
        public static MainWindowViewModel? Instance()
        {
            return _instance;
        }

        public MainWindowViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            // Inicjalizacja widoków
            SearchSubView = new SearchViewModel(_context, _dialogService);
            BookingsSubView = new BookingsViewModel(_context, _dialogService);
            CustomersSubView = new CustomersViewModel(_context, _dialogService);
            GuidesSubView = new GuidesViewModel(_context, _dialogService);
            LocationsSubView = new LocationsViewModel(_context, _dialogService);
            ToursSubView = new ToursViewModel(_context, _dialogService);


            BookingReportSubView = new BookingReportViewModel(_context);
            CustomerReportSubView = new CustomerReportViewModel(_context);
        }
    }
}