using TravelAgency.Models;

namespace TravelAgency.Interfaces
{
    /// <summary>
    /// Interface for the Customer Service
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Add a new customer to the database
        /// </summary>
        /// <param name="firstName">First name of the customer</param>
        /// <param name="lastName">Last name of the customer</param>
        /// <param name="email">Email of the customer</param>
        /// <param name="phoneNumber">Phone number of the customer</param>
        void AddCustomer(string firstName, string lastName, string email, string phoneNumber);

        /// <summary>
        /// Edit an existing customer in the database
        /// </summary>
        /// <param name="customer">Customer to be edited</param>
        /// <param name="firstName">First name of the customer</param>
        /// <param name="lastName">Last name of the customer</param>
        /// <param name="email">Email of the customer</param>
        /// <param name="phoneNumber">Phone number of the customer</param>
        void EditCustomer(Customer customer, string firstName, string lastName, string email, string phoneNumber);

        /// <summary>
        /// Delete a customer from the database
        /// </summary>
        /// <param name="customerId">Id of the customer to be deleted</param>
        void DeleteCustomer(int customerId);
    }
}
