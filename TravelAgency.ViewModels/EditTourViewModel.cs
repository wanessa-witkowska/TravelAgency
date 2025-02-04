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
                if (_tourId != value)
                {
                    _tourId = value;
                    OnPropertyChanged(nameof(TourId));
                    LoadTour();
                }
            }
        }

        private Tour? _tour;
        public Tour? Tour
        {
            get => _tour;
            set
            {
                if (_tour != value)
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
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private decimal _price = 0;
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        private string _destination = string.Empty;
        public string Destination
        {
            get => _destination;
            set
            {
                if (_destination != value)
                {
                    _destination = value;
                    OnPropertyChanged(nameof(Destination));
                }
            }
        }

        private int _guideId = 0;
        public int GuideId
        {
            get => _guideId;
            set
            {
                if (_guideId != value)
                {
                    _guideId = value;
                    OnPropertyChanged(nameof(GuideId));
                }
            }
        }

        public string Response { get; set; }

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
                   StartDate != DateTime.MinValue &&
                   EndDate != DateTime.MinValue &&
                   Price > 0 &&
                   !string.IsNullOrEmpty(Destination) &&
                   GuideId > 0;
        }

        public EditTourViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
            Guides = _context.Guides.ToList();
        }

        public void LoadTour()
        {
            Tour = _context.Tours.FirstOrDefault(t => t.Id == TourId);
        }
    }
}
