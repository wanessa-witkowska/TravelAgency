using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.Services
{
    public class GuideService : IGuideService
    {
        private readonly travelAgencyContext _context;

        public GuideService(travelAgencyContext context)
        {
            _context = context;
        }

        public void AddGuide(string firstName, string lastName, string specialization, int experienceYears, string languages)
        {
            var newGuide = new Guide
            {
                FirstName = firstName,
                LastName = lastName,
                Specialization = specialization,
                ExperienceYears = experienceYears,
                Languages = languages
            };

            ValidateGuide(newGuide);

            _context.Guides.Add(newGuide);
            _context.SaveChanges();
        }

        public void EditGuide(Guide guide, string firstName, string lastName, string specialization, int experienceYears, string languages)
        {
            guide.FirstName = firstName;
            guide.LastName = lastName;
            guide.Specialization = specialization;
            guide.ExperienceYears = experienceYears;
            guide.Languages = languages;

            ValidateGuide(guide);

            _context.Guides.Update(guide);
            _context.SaveChanges();
        }

        public void DeleteGuide(int guideId)
        {
            var guide = _context.Guides.FirstOrDefault(g => g.Id == guideId);
            if (guide == null)
                throw new ArgumentException("Podany przewodnik nie istnieje.");

            _context.Guides.Remove(guide);
            _context.SaveChanges();
        }

        private void ValidateGuide(Guide guide)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(guide);

            if (!Validator.TryValidateObject(guide, validationContext, validationResults, true))
                throw new ValidationException(string.Join("; ", validationResults.Select(vr => vr.ErrorMessage)));

            if (string.IsNullOrWhiteSpace(guide.Languages))
                throw new ValidationException("Przewodnik musi znać przynajmniej jeden język.");
        }
    }
}
