using TravelAgency.Models;

namespace TravelAgency.Interfaces
{
    /// <summary>
    /// Interface for the Booking Service
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Add a new booking to the database
        /// </summary>
        /// <param name="tourId">Tour ID of the booking</param>
        /// <param name="customerId">Customer ID for the booking</param>
        /// <param name="tourDate">The date of the tour</param>
        /// <param name="numberOfParticipants">Number of participants in the booking</param>
        /// <param name="totalPrice">Total price of the booking</param>
        /// <param name="status">Booking status</param>
        void AddBooking(int tourId, int customerId, DateTime tourDate, int numberOfParticipants, decimal totalPrice, string status);

        /// <summary>
        /// Edit an existing booking in the database
        /// </summary>
        /// <param name="booking">The booking to be edited</param>
        /// <param name="tourId">Tour ID of the booking</param>
        /// <param name="customerId">Customer ID for the booking</param>
        /// <param name="tourDate">The date of the tour</param>
        /// <param name="numberOfParticipants">Number of participants in the booking</param>
        /// <param name="totalPrice">Total price of the booking</param>
        /// <param name="status">Booking status</param>
        void EditBooking(Booking booking, int tourId, int customerId, DateTime tourDate, int numberOfParticipants, decimal totalPrice, string status);

        /// <summary>
        /// Delete a booking from the database
        /// </summary>
        /// <param name="bookingId">Id of the booking to be deleted</param>
        void DeleteBooking(int bookingId);
    }
}
