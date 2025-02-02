using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class AddBookingViewModel : ViewModelBase, IDataErrorInfo
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
                if (columnName == "TourId")
                {
                    if (TourId <= 0)
                    {
                        return "Tour is Required";
                    }
                }
                if (columnName == "CustomerId")
                {
                    if (CustomerId <= 0)
                    {
                        return "Customer is Required";
                    }
                }
                if (columnName == "BookingDate")
                {
                    if (BookingDate == default)
                    {
                        return "Booking Date is Required";
                    }
                }
                if (columnName == "TourDate")
                {
                    if (TourDate == default)
                    {
                        return "Tour Date is Required";
                    }
                }
                if (columnName == "NumberOfParticipants")
                {
                    if (NumberOfParticipants <= 0)
                    {
                        return "Number of Participants must be greater than zero";
                    }
                }
                if (columnName == "TotalPrice")
                {
                    if (TotalPrice <= 0)
                    {
                        return "Total Price must be greater than zero";
                    }
                }
                return string.Empty;
            }
        }

        private int _tourId = 0;
        public int TourId
        {
            get { return _tourId; }
            set
            {
                _tourId = value;
                OnPropertyChanged(nameof(TourId));
            }
        }

        private int _customerId = 0;
        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                OnPropertyChanged(nameof(CustomerId));
            }
        }

        private DateTime _bookingDate = default;
        public DateTime BookingDate
        {
            get { return _bookingDate; }
            set
            {
                _bookingDate = value;
                OnPropertyChanged(nameof(BookingDate));
            }
        }

        private DateTime _tourDate = default;
        public DateTime TourDate
        {
            get { return _tourDate; }
            set
            {
                _tourDate = value;
                OnPropertyChanged(nameof(TourDate));
            }
        }

        private int _numberOfParticipants = 0;
        public int NumberOfParticipants
        {
            get { return _numberOfParticipants; }
            set
            {
                _numberOfParticipants = value;
                OnPropertyChanged(nameof(NumberOfParticipants));
            }
        }

        private decimal _totalPrice = 0;
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
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

        private ObservableCollection<Tour>? _tours = null;
        public ObservableCollection<Tour>? Tours
        {
            get
            {
                if (_tours is null)
                {
                    _tours = LoadTours();
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

        private ObservableCollection<Customer>? _customers = null;
        public ObservableCollection<Customer>? Customers
        {
            get
            {
                if (_customers is null)
                {
                    _customers = LoadCustomers();
                    return _customers;
                }
                return _customers;
            }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private ICommand? _back = null;
        public ICommand? Back
        {
            get
            {
                if (_back is null)
                {
                    _back = new RelayCommand<object>(NavigateBack);
                }
                return _back;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.BookingsSubView = new BookingsViewModel(_context, _dialogService);
            }
        }

        private ICommand? _save = null;
        public ICommand? Save
        {
            get
            {
                if (_save is null)
                {
                    _save = new RelayCommand<object>(SaveData);
                }
                return _save;
            }
        }

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            Booking booking = new Booking
            {
                TourId = this.TourId,
                CustomerId = this.CustomerId,
                BookingDate = this.BookingDate,
                TourDate = this.TourDate,
                NumberOfParticipants = this.NumberOfParticipants,
                TotalPrice = this.TotalPrice,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public AddBookingViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private ObservableCollection<Tour> LoadTours()
        {
            _context.Database.EnsureCreated();
            _context.Tours.Load();
            return _context.Tours.Local.ToObservableCollection();
        }

        private ObservableCollection<Customer> LoadCustomers()
        {
            _context.Database.EnsureCreated();
            _context.Customers.Load();
            return _context.Customers.Local.ToObservableCollection();
        }

        private bool IsValid()
        {
            string[] properties = { "TourId", "CustomerId", "BookingDate", "TourDate", "NumberOfParticipants", "TotalPrice" };
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
