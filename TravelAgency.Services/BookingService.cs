using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.Services
{
    public class BookingService : IBookingService
    {
        private readonly travelAgencyContext _context;

        public BookingService(travelAgencyContext context)
        {
            _context = context;
        }

        public void AddBooking(int tourId, int customerId, DateTime tourDate, int numberOfParticipants, decimal totalPrice, string status)
        {
            if (!_context.Tours.Any(t => t.Id == tourId))
                throw new ArgumentException("Podana wycieczka nie istnieje.");

            if (!_context.Customers.Any(c => c.Id == customerId))
                throw new ArgumentException("Podany klient nie istnieje.");

            var newBooking = new Booking
            {
                TourId = tourId,
                CustomerId = customerId,
                BookingDate = DateTime.UtcNow,
                TourDate = tourDate,
                NumberOfParticipants = numberOfParticipants,
                TotalPrice = totalPrice,
                Status = status
            };

            ValidateBooking(newBooking);

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();
        }

        public void EditBooking(Booking booking, int tourId, int customerId, DateTime tourDate, int numberOfParticipants, decimal totalPrice, string status)
        {
            if (!_context.Tours.Any(t => t.Id == tourId))
                throw new ArgumentException("Podana wycieczka nie istnieje.");

            if (!_context.Customers.Any(c => c.Id == customerId))
                throw new ArgumentException("Podany klient nie istnieje.");

            booking.TourId = tourId;
            booking.CustomerId = customerId;
            booking.TourDate = tourDate;
            booking.NumberOfParticipants = numberOfParticipants;
            booking.TotalPrice = totalPrice;
            booking.Status = status;

            ValidateBooking(booking);

            _context.Bookings.Update(booking);
            _context.SaveChanges();
        }

        public void DeleteBooking(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
                throw new ArgumentException("Podana rezerwacja nie istnieje.");

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
        }

        private void ValidateBooking(Booking booking)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(booking);

            if (!Validator.TryValidateObject(booking, validationContext, validationResults, true))
                throw new ValidationException(string.Join("; ", validationResults.Select(vr => vr.ErrorMessage)));

            if (booking.TourDate < booking.BookingDate)
                throw new ValidationException("Data wycieczki nie może być wcześniejsza niż data rezerwacji.");

            if (booking.BookingDate < DateTime.UtcNow.Date)
                throw new ValidationException("Data rezerwacji nie może być w przeszłości.");
        }
    }
}
