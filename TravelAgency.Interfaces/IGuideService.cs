using TravelAgency.Models;

namespace TravelAgency.Interfaces
{
    /// <summary>
    /// Interface for the Guide Service
    /// </summary>
    public interface IGuideService
    {
        /// <summary>
        /// Add a new guide to the database
        /// </summary>
        /// <param name="firstName">First name of the guide</param>
        /// <param name="lastName">Last name of the guide</param>
        /// <param name="specialization">Specialization of the guide</param>
        /// <param name="experienceYears">Years of experience of the guide</param>
        /// <param name="languages">Languages spoken by the guide</param>
        void AddGuide(string firstName, string lastName, string specialization, int experienceYears, string languages);

        /// <summary>
        /// Edit an existing guide in the database
        /// </summary>
        /// <param name="guide">Guide to be edited</param>
        /// <param name="firstName">First name of the guide</param>
        /// <param name="lastName">Last name of the guide</param>
        /// <param name="specialization">Specialization of the guide</param>
        /// <param name="experienceYears">Years of experience of the guide</param>
        /// <param name="languages">Languages spoken by the guide</param>
        void EditGuide(Guide guide, string firstName, string lastName, string specialization, int experienceYears, string languages);

        /// <summary>
        /// Delete a guide from the database
        /// </summary>
        /// <param name="guideId">Id of the guide to be deleted</param>
        void DeleteGuide(int guideId);
    }
}
