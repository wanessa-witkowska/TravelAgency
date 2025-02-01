using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditGuideViewModel : ViewModelBase
    {
        private Guide _guide;
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        public int GuideId { get; set; }

        public EditGuideViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        public EditGuideViewModel(Guide guide)
        {
            _guide = guide;
            FirstName = guide.FirstName;
            LastName = guide.LastName;
            Specialization = guide.Specialization;
            ExperienceYears = guide.ExperienceYears;
            Languages = guide.Languages;
        }

        public string FirstName
        {
            get => _guide.FirstName;
            set
            {
                _guide.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _guide.LastName;
            set
            {
                _guide.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Specialization
        {
            get => _guide.Specialization;
            set
            {
                _guide.Specialization = value;
                OnPropertyChanged();
            }
        }

        public int ExperienceYears
        {
            get => _guide.ExperienceYears;
            set
            {
                _guide.ExperienceYears = value;
                OnPropertyChanged();
            }
        }

        public string Languages
        {
            get => _guide.Languages;
            set
            {
                _guide.Languages = value;
                OnPropertyChanged();
            }
        }

        public void SaveChanges()
        {
            using (var context = new travelAgencyContext())
            {
                context.Guides.Update(_guide);
                context.SaveChanges();
            }
        }
    }

}
