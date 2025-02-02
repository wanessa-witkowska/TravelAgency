using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
                }
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public string Languages { get; set; }

        public string Response { get; set; }

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);
        private ICommand? _back;

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.GuidesSubView = new GuidesViewModel(_context, _dialogService);
            }
        }

        public ICommand Save => _save ??= new RelayCommand<object>(SaveChanges);
        private ICommand? _save;

        private void SaveChanges(object? obj)
        {
            if (Guide == null)
            {
                Response = "No guide selected";
                return;
            }

            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var existingGuide = _context.Guides.FirstOrDefault(g => g.Id == Guide.Id);
            if (existingGuide != null)
            {
                existingGuide.FirstName = FirstName;
                existingGuide.LastName = LastName;
                existingGuide.Specialization = Specialization;
                existingGuide.ExperienceYears = ExperienceYears;
                existingGuide.Languages = Languages;
            }
            else
            {
                var newGuide = new Guide
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Specialization = Specialization,
                    ExperienceYears = ExperienceYears,
                    Languages = Languages
                };
                _context.Guides.Add(newGuide);
            }

            _context.SaveChanges();
            Response = "Guide details successfully updated";
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Specialization) &&
                   ExperienceYears >= 0 &&
                   !string.IsNullOrEmpty(Languages);
        }

        public EditGuideViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private void LoadGuide()
        {
            Guide = _context.Guides.FirstOrDefault(g => g.Id == GuideId);
        }
    }
}
