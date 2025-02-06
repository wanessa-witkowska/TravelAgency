using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class EditGuideViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private int _guideId;
        public int GuideId
        {
            get => _guideId;
            set
            {
                _guideId = value;
                OnPropertyChanged(nameof(GuideId));
                LoadGuide();
            }
        }

        private Guide? _guide;
        public Guide? Guide
        {
            get => _guide;
            set
            {
                _guide = value;
                OnPropertyChanged(nameof(Guide));
                if (_guide != null)
                {
                    FirstName = _guide.FirstName;
                    LastName = _guide.LastName;
                    Specialization = _guide.Specialization;
                    ExperienceYears = _guide.ExperienceYears;
                    Languages = _guide.Languages;
                    AssignedLocations = new ObservableCollection<Location>(_guide.Locations);
                }
            }
        }

        // Właściwości dla danych przewodnika
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; } = 0;
        public string Languages { get; set; } = string.Empty;

        // Właściwość Response
        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        // Właściwości dla lokalizacji
        private ObservableCollection<Location> _assignedLocations = new ObservableCollection<Location>();
        public ObservableCollection<Location> AssignedLocations
        {
            get => _assignedLocations;
            set
            {
                _assignedLocations = value;
                OnPropertyChanged(nameof(AssignedLocations));
            }
        }

        private ObservableCollection<Location> _availableLocations = new ObservableCollection<Location>();
        public ObservableCollection<Location> AvailableLocations
        {
            get => _availableLocations;
            set
            {
                _availableLocations = value;
                OnPropertyChanged(nameof(AvailableLocations));
            }
        }

        // Komendy
        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);
        private ICommand? _back;

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);
        private ICommand? _save;

        public ICommand AddLocationCommand => _addLocationCommand ??= new RelayCommand<object>(AddLocation);
        private ICommand? _addLocationCommand;

        public ICommand RemoveLocationCommand => _removeLocationCommand ??= new RelayCommand<object>(RemoveLocation);
        private ICommand? _removeLocationCommand;

        // Konstruktor
        public EditGuideViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            // Załaduj dostępne lokalizacje z bazy danych
            _context.Locations.Load();
            AvailableLocations = new ObservableCollection<Location>(_context.Locations.Local);
        }

        // Metoda do ładowania przewodnika
        private void LoadGuide()
        {
            Guide = _context.Guides
                .Include(g => g.Locations)
                .FirstOrDefault(g => g.Id == GuideId);

            if (Guide != null)
            {
                // Usuń przypisane lokalizacje z dostępnych
                foreach (var location in Guide.Locations)
                {
                    var availableLocation = AvailableLocations.FirstOrDefault(l => l.Id == location.Id);
                    if (availableLocation != null)
                    {
                        AvailableLocations.Remove(availableLocation);
                    }
                }
            }
            else
            {
                Response = "Guide not found"; // Użyj właściwości Response
            }
        }

        // Metoda do zapisywania zmian
        private void SaveChanges(object? obj)
        {
            if (Guide == null)
            {
                Response = "No guide selected"; // Użyj właściwości Response
                return;
            }

            if (!IsValid())
            {
                Response = "Please complete all required fields"; // Użyj właściwości Response
                return;
            }

            var existingGuide = _context.Guides
                .Include(g => g.Locations)
                .FirstOrDefault(g => g.Id == Guide.Id);

            if (existingGuide != null)
            {
                existingGuide.FirstName = FirstName;
                existingGuide.LastName = LastName;
                existingGuide.Specialization = Specialization;
                existingGuide.ExperienceYears = ExperienceYears;
                existingGuide.Languages = Languages;

                // Zaktualizuj przypisane lokalizacje
                existingGuide.Locations.Clear();
                foreach (var location in AssignedLocations)
                {
                    existingGuide.Locations.Add(location);
                }
            }

            _context.SaveChanges();
            Response = "Guide details successfully updated"; // Użyj właściwości Response
        }

        // Metoda do dodawania lokalizacji
        private void AddLocation(object? obj)
        {
            if (obj is Location location)
            {
                AvailableLocations.Remove(location);
                AssignedLocations.Add(location);
            }
        }

        // Metoda do usuwania lokalizacji
        private void RemoveLocation(object? obj)
        {
            if (obj is Location location)
            {
                AssignedLocations.Remove(location);
                AvailableLocations.Add(location);
            }
        }

        // Metoda do nawigacji wstecz
        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.GuidesSubView = new GuidesViewModel(_context, _dialogService);
            }
        }

        // Walidacja danych
        private bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Specialization) &&
                   ExperienceYears >= 0 &&
                   !string.IsNullOrEmpty(Languages);
        }
    }
}