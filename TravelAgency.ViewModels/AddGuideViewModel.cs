using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class AddGuideViewModel : ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        private string _specialization;
        private int _experienceYears;
        private string _languages;

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Specialization
        {
            get => _specialization;
            set => SetProperty(ref _specialization, value);
        }

        public int ExperienceYears
        {
            get => _experienceYears;
            set => SetProperty(ref _experienceYears, value);
        }

        public string Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }

        public void SaveGuide()
        {
            using (var context = new travelAgencyContext())
            {
                var newGuide = new Guide
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Specialization = Specialization,
                    ExperienceYears = ExperienceYears,
                    Languages = Languages
                };
                context.Guides.Add(newGuide);
                context.SaveChanges();
            }
        }
    }

}
