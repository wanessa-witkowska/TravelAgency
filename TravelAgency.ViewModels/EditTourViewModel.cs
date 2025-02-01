using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditTourViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;
        public int TourId { get; set; }
        private Tour _tour;

        public EditTourViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        public EditTourViewModel(Tour tour)
        {
            _tour = tour;
            Name = tour.Name;
            Description = tour.Description;
            StartDate = tour.StartDate;
            EndDate = tour.EndDate;
            Price = tour.Price;
            Destination = tour.Destination;
        }

        public string Name
        {
            get => _tour.Name;
            set
            {
                _tour.Name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _tour.Description;
            set
            {
                _tour.Description = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _tour.StartDate;
            set
            {
                _tour.StartDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get => _tour.EndDate;
            set
            {
                _tour.EndDate = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _tour.Price;
            set
            {
                _tour.Price = value;
                OnPropertyChanged();
            }
        }

        public string Destination
        {
            get => _tour.Destination;
            set
            {
                _tour.Destination = value;
                OnPropertyChanged();
            }
        }

        public void SaveChanges()
        {
            using (var context = new travelAgencyContext())
            {
                context.Tours.Update(_tour);
                context.SaveChanges();
            }
        }
    }
}
