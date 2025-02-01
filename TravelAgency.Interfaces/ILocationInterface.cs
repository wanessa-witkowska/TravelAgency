using TravelAgency.Models;

namespace TravelAgency.Interfaces
{
    /// <summary>
    /// Interface for the Location Service
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Add a new location to the database
        /// </summary>
        /// <param name="name">Name of the location</param>
        /// <param name="description">Description of the location</param>
        /// <param name="address">Address of the location</param>
        /// <param name="placeType">Type of the place (e.g., Museum, Mountain, Castle)</param>
        void AddLocation(string name, string description, string address, string placeType);

        /// <summary>
        /// Edit an existing location in the database
        /// </summary>
        /// <param name="location">Location to be edited</param>
        /// <param name="name">Name of the location</param>
        /// <param name="description">Description of the location</param>
        /// <param name="address">Address of the location</param>
        /// <param name="placeType">Type of the place (e.g., Museum, Mountain, Castle)</param>
        void EditLocation(Location location, string name, string description, string address, string placeType);

        /// <summary>
        /// Delete a location from the database
        /// </summary>
        /// <param name="locationId">Id of the location to be deleted</param>
        void DeleteLocation(int locationId);
    }
}
