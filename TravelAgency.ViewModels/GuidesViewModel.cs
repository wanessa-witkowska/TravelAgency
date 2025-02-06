using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class GuidesViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private ObservableCollection<Guide>? _guides = null;
        public ObservableCollection<Guide>? Guides
        {
            get
            {
                if (_guides is null)
                {
                    _guides = new ObservableCollection<Guide>();
                    return _guides;
                }
                return _guides;
            }
            set
            {
                _guides = value;
                OnPropertyChanged(nameof(Guides));
            }
        }

        private Guide? _selectedGuide;
        public Guide? SelectedGuide
        {
            get => _selectedGuide;
            set
            {
                _selectedGuide = value;
                OnPropertyChanged(nameof(SelectedGuide));
                IsLocationsExpanded = _selectedGuide != null;
            }
        }

        private bool _isLocationsExpanded;
        public bool IsLocationsExpanded
        {
            get => _isLocationsExpanded;
            set
            {
                _isLocationsExpanded = value;
                OnPropertyChanged(nameof(IsLocationsExpanded));
            }
        }

     
        public ObservableCollection<Location>? SelectedGuideLocations => SelectedGuide?.Locations;

        private ICommand? _add = null;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewGuide);
                }
                return _add;
            }
        }

        private void AddNewGuide(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.GuidesSubView = new AddGuideViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditGuide);
                }
                return _edit;
            }
        }

        private void EditGuide(object? obj)
        {
            if (obj is not null)
            {
                int guideId = (int)obj;
                EditGuideViewModel editGuideViewModel = new EditGuideViewModel(_context, _dialogService)
                {
                    GuideId = guideId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.GuidesSubView = editGuideViewModel;
                }
            }
        }

        private ICommand? _remove = null;
        public ICommand? Remove
        {
            get
            {
                if (_remove is null)
                {
                    _remove = new RelayCommand<object>(RemoveGuide);
                }
                return _remove;
            }
        }

        private void RemoveGuide(object? obj)
        {
            if (obj is not null)
            {
                int guideId = (int)obj;
                Guide? guide = _context.Guides.Find(guideId);
                if (guide is not null)
                {
                    bool? dialogResult = _dialogService.Show($"Do you want to remove the guide {guide.FirstName} {guide.LastName}?");
                    if (dialogResult == false)
                    {
                        return;
                    }

                    _context.Guides.Remove(guide);
                    _context.SaveChanges();
                }
            }
        }

        public GuidesViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Guides.Include(g => g.Locations).Load(); // Ładowanie lokalizacji wraz z przewodnikami
            Guides = _context.Guides.Local.ToObservableCollection();
        }
    }
}