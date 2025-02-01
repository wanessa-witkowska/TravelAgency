using TravelAgency.Models;

namespace TravelAgency.Interfaces
{
    /// <summary>
    /// Interface for the Tour Service
    /// </summary>
    public interface ITourService
    {
        /// <summary>
        /// Add a new tour to the database
        /// </summary>
        /// <param name="name">Name of the tour</param>
        /// <param name="description">Description of the tour</param>
        /// <param name="startDate">Start date of the tour</param>
        /// <param name="endDate">End date of the tour</param>
        /// <param name="price">Price of the tour</param>
        /// <param name="destination">Destination of the tour</param>
        /// <param name="guideId">ID of the assigned guide</param>
        void AddTour(string name, string description, DateTime startDate, DateTime endDate, decimal price, string destination, int guideId);

        /// <summary>
        /// Edit an existing tour in the database
        /// </summary>
        /// <param name="tour">Tour to be edited</param>
        /// <param name="name">Name of the tour</param>
        /// <param name="description">Description of the tour</param>
        /// <param name="startDate">Start date of the tour</param>
        /// <param name="endDate">End date of the tour</param>
        /// <param name="price">Price of the tour</param>
        /// <param name="destination">Destination of the tour</param>
        /// <param name="guideId">ID of the assigned guide</param>
        void EditTour(Tour tour, string name, string description, DateTime startDate, DateTime endDate, decimal price, string destination, int guideId);

        /// <summary>
        /// Delete a tour from the database
        /// </summary>
        /// <param name="tourId">Id of the tour to be deleted</param>
        void DeleteTour(int tourId);
    }
}
