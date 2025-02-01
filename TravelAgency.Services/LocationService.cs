using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.Services
{
    public class LocationService : ILocationService
    {
        private readonly travelAgencyContext _context;

        public LocationService(travelAgencyContext context)
        {
            _context = context;
        }

        public void AddLocation(string name, string description, string address, string placeType)
        {
            var newLocation = new Location
            {
                Name = name,
                Description = description,
                Address = address,
                PlaceType = placeType
            };

            ValidateLocation(newLocation);

            _context.Locations.Add(newLocation);
            _context.SaveChanges();
        }

        public void EditLocation(Location location, string name, string description, string address, string placeType)
        {
            location.Name = name;
            location.Description = description;
            location.Address = address;
            location.PlaceType = placeType;

            ValidateLocation(location);

            _context.Locations.Update(location);
            _context.SaveChanges();
        }

        public void DeleteLocation(int locationId)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == locationId);
            if (location == null)
                throw new ArgumentException("Podana lokalizacja nie istnieje.");

            _context.Locations.Remove(location);
            _context.SaveChanges();
        }

        private void ValidateLocation(Location location)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(location);

            if (!Validator.TryValidateObject(location, validationContext, validationResults, true))
                throw new ValidationException(string.Join("; ", validationResults.Select(vr => vr.ErrorMessage)));
        }
    }
}
