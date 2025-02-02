using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using TravelAgency.Interfaces;
using TravelAgency.Models;
using TravelAgency.Data;

namespace TravelAgency.ViewModels
{
    public class EditGuideViewModel : ViewModelBase
    {
        private Guide _guide;
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        private ICommand? _back;
        private ICommand? _save;

        public int GuideId { get; set; }

        public EditGuideViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        public EditGuideViewModel(Guide guide, travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
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

        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.GuidesSubView = new GuidesViewModel(_context, _dialogService);
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

            _context.Guides.Update(_guide);
            _context.SaveChanges();

            Response = "Guide details successfully updated";
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Specialization) &&
                   !string.IsNullOrEmpty(Languages);
        }

        public string Response { get; set; }

    }
}
