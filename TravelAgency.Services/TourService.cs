using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.Services
{
    public class TourService : ITourService
    {
        private readonly travelAgencyContext _context;

        public TourService(travelAgencyContext context)
        {
            _context = context;
        }

        public void AddTour(string name, string description, DateTime startDate, DateTime endDate, decimal price, string destination, int guideId)
        {
            var newTour = new Tour
            {
                Name = name,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                Price = price,
                Destination = destination,
                GuideId = guideId
            };

            ValidateTour(newTour);

            _context.Tours.Add(newTour);
            _context.SaveChanges();
        }

        public void EditTour(Tour tour, string name, string description, DateTime startDate, DateTime endDate, decimal price, string destination, int guideId)
        {
            tour.Name = name;
            tour.Description = description;
            tour.StartDate = startDate;
            tour.EndDate = endDate;
            tour.Price = price;
            tour.Destination = destination;
            tour.GuideId = guideId;

            ValidateTour(tour);

            _context.Tours.Update(tour);
            _context.SaveChanges();
        }

        public void DeleteTour(int tourId)
        {
            var tour = _context.Tours.FirstOrDefault(t => t.Id == tourId);
            if (tour == null)
                throw new ArgumentException("Podana wycieczka nie istnieje.");

            _context.Tours.Remove(tour);
            _context.SaveChanges();
        }

        private void ValidateTour(Tour tour)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(tour);

            if (!Validator.TryValidateObject(tour, validationContext, validationResults, true))
                throw new ValidationException(string.Join("; ", validationResults.Select(vr => vr.ErrorMessage)));
        }
    }
}
