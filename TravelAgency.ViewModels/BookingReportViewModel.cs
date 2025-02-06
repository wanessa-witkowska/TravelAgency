using CommunityToolkit.Mvvm.ComponentModel;
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
    public class BookingReportViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;

        private ObservableCollection<Booking> _bookings = new ObservableCollection<Booking>();
        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        private string _searchStatus = string.Empty;
        public string SearchStatus
        {
            get => _searchStatus;
            set
            {
                _searchStatus = value;
                OnPropertyChanged(nameof(SearchStatus));
                FilterBookings();
            }
        }

        private DateTime _searchBookingDate = DateTime.Today;
        public DateTime SearchBookingDate
        {
            get => _searchBookingDate;
            set
            {
                _searchBookingDate = value;
                OnPropertyChanged(nameof(SearchBookingDate));
                FilterBookings();
            }
        }

        public BookingReportViewModel(travelAgencyContext context)
        {
            _context = context;
            LoadBookings();
        }

        private void LoadBookings()
        {
            _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.Customer)
                .Load();
            Bookings = _context.Bookings.Local.ToObservableCollection();
        }

        private void FilterBookings()
        {
            var filteredBookings = _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.Customer)
                .Where(b => string.IsNullOrEmpty(SearchStatus) || b.Status.Contains(SearchStatus))
                .Where(b => b.BookingDate.Date == SearchBookingDate.Date)
                .ToList();

            Bookings = new ObservableCollection<Booking>(filteredBookings);
        }
    }
}