using System;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace TravelAgency.ViewModels
{
    public class EditTourViewModel : ViewModelBase
    {
        private Tour _tour;
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private ICommand? _back;
        private ICommand? _save;

        public int TourId { get; set; }

        public EditTourViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        public EditTourViewModel(Tour tour)
        {
            _tour = tour;
            TourId = tour.Id;
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

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.ToursSubView = new ToursViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);

        private void SaveChanges(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            _context.Tours.Update(_tour);
            _context.SaveChanges();

            Response = "Tour details successfully updated";
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Description) &&
                   StartDate <= EndDate &&
                   Price > 0 &&
                   !string.IsNullOrEmpty(Destination);
        }

        public string Response { get; set; }
    }
}
