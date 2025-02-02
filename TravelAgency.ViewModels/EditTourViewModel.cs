using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditTourViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private int _tourId;
        public int TourId
        {
            get => _tourId;
            set
            {
                _tourId = value;
                OnPropertyChanged(nameof(TourId));
                LoadTour();
            }
        }

        private Tour? _tour;
        public Tour? Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnPropertyChanged(nameof(Tour));
                if (_tour != null)
                {
                    Name = _tour.Name;
                    Description = _tour.Description;
                    StartDate = _tour.StartDate;
                    EndDate = _tour.EndDate;
                    Price = _tour.Price;
                    Destination = _tour.Destination;
                    GuideId = _tour.GuideId;
                }
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string Destination { get; set; }
        public int GuideId { get; set; }

        public string Response { get; set; }

        // Właściwość do przechowywania listy przewodników
        public IEnumerable<Guide> Guides { get; set; }

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);
        private ICommand? _back;

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.ToursSubView = new ToursViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);
        private ICommand? _save;

        private void SaveChanges(object? obj)
        {
            if (Tour == null)
            {
                Response = "No tour selected";
                return;
            }

            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var existingTour = _context.Tours.FirstOrDefault(t => t.Id == Tour.Id);
            if (existingTour != null)
            {
                existingTour.Name = Name;
                existingTour.Description = Description;
                existingTour.StartDate = StartDate;
                existingTour.EndDate = EndDate;
                existingTour.Price = Price;
                existingTour.Destination = Destination;
                existingTour.GuideId = GuideId;
            }
            else
            {
                var newTour = new Tour
                {
                    Name = Name,
                    Description = Description,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Price = Price,
                    Destination = Destination,
                    GuideId = GuideId
                };
                _context.Tours.Add(newTour);
            }

            _context.SaveChanges();
            Response = "Tour details successfully updated";
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Description) &&
                   StartDate != null &&
                   EndDate != null &&
                   Price > 0 &&
                   !string.IsNullOrEmpty(Destination) &&
                   GuideId > 0;
        }

        public EditTourViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            // Pobranie wszystkich przewodników z bazy danych
            Guides = _context.Guides.ToList();
        }

        private void LoadTour()
        {
            Tour = _context.Tours.FirstOrDefault(t => t.Id == TourId);
        }
    }
}
