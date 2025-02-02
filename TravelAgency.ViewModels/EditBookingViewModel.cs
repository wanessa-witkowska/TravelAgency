using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditBookingViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private int _bookingId;
        public int BookingId
        {
            get => _bookingId;
            set
            {
                _bookingId = value;
                OnPropertyChanged(nameof(BookingId));
                LoadBooking();
            }
        }

        private Booking? _booking;
        public Booking? Booking
        {
            get => _booking;
            set
            {
                _booking = value;
                OnPropertyChanged(nameof(Booking));
                if (_booking != null)
                {
                    CustomerId = _booking.CustomerId;
                    TourId = _booking.TourId;
                    NumberOfParticipants = _booking.NumberOfParticipants;
                    BookingDate = _booking.BookingDate;
                    TourDate = _booking.TourDate;
                    TotalPrice = _booking.TotalPrice;
                    Status = _booking.Status;
                }
            }
        }

        public int CustomerId { get; set; }
        public int TourId { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? TourDate { get; set; }
        public decimal TotalPrice { get; set; }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public List<string> Statuses { get; } = new List<string>
        {
            "Pending",
            "Confirmed",
            "Cancelled",
            "Completed"
        };

        public string Response { get; set; }

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);
        private ICommand? _back;

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.BookingsSubView = new BookingsViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);
        private ICommand? _save;

        private void SaveChanges(object? obj)
        {
            if (Booking == null)
            {
                Response = "No booking selected";
                return;
            }

            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var existingBooking = _context.Bookings.FirstOrDefault(b => b.Id == Booking.Id);
            if (existingBooking != null)
            {
                existingBooking.CustomerId = CustomerId;
                existingBooking.TourId = TourId;

                if (BookingDate == null || TourDate == null)
                {
                    Response = "Please select both booking date and tour date.";
                    return;
                }

                existingBooking.BookingDate = BookingDate.Value;
                existingBooking.TourDate = TourDate.Value;
                existingBooking.NumberOfParticipants = NumberOfParticipants;
                existingBooking.TotalPrice = TotalPrice;
                existingBooking.Status = Status;
            }
            else
            {
                var newBooking = new Booking
                {
                    CustomerId = CustomerId,
                    TourId = TourId,
                    BookingDate = BookingDate ?? DateTime.MinValue,
                    TourDate = TourDate ?? DateTime.MinValue,
                    NumberOfParticipants = NumberOfParticipants,
                    TotalPrice = TotalPrice,
                    Status = Status
                };
                _context.Bookings.Add(newBooking);
            }

            _context.SaveChanges();
            Response = "Booking details successfully updated";
        }

        private bool IsValid()
        {
            return CustomerId > 0 &&
                   TourId > 0 &&
                   NumberOfParticipants > 0 &&
                   BookingDate != null &&
                   TourDate != null &&
                   TotalPrice > 0 &&
                   !string.IsNullOrEmpty(Status);
        }

        public EditBookingViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private void LoadBooking()
        {
            Booking = _context.Bookings.FirstOrDefault(b => b.Id == BookingId);
        }
    }
}
