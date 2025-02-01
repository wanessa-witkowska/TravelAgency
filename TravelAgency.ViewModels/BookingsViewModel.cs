using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class BookingsViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                _dialogResult = value;
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
                    return _bookings;
                }
                return _bookings;
            }
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        private ICommand? _add = null;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewBooking);
                }
                return _add;
            }
        }

        private void AddNewBooking(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.BookingsSubView = new AddBookingViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditBooking);
                }
                return _edit;
            }
        }

        private void EditBooking(object? obj)
        {
            if (obj is not null)
            {
                int bookingId = (int)obj;
                EditBookingViewModel editBookingViewModel = new EditBookingViewModel(_context, _dialogService)
                {
                    BookingId = bookingId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.BookingsSubView = editBookingViewModel;
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
                    _remove = new RelayCommand<object>(RemoveBooking);
                }
                return _remove;
            }
        }

        private void RemoveBooking(object? obj)
        {
            if (obj is not null)
            {
                int bookingId = (int)obj;
                Booking? booking = _context.Bookings.Find(bookingId);
                if (booking is not null)
                {
                    DialogResult = _dialogService.Show("Do you want to remove the booking for " + booking.Customer.FirstName + " " + booking.Customer.LastName + "?");
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.Bookings.Remove(booking);
                    _context.SaveChanges();
                }
            }
        }

        public BookingsViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Bookings.Load();
            Bookings = _context.Bookings.Local.ToObservableCollection();
        }
    }
}
