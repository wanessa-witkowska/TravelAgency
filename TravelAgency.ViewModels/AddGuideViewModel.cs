using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class AddGuideViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName" && string.IsNullOrWhiteSpace(FirstName))
                    return "First Name is required";
                if (columnName == "LastName" && string.IsNullOrWhiteSpace(LastName))
                    return "Last Name is required";
                if (columnName == "Specialization" && string.IsNullOrWhiteSpace(Specialization))
                    return "Specialization is required";
                if (columnName == "ExperienceYears" && ExperienceYears < 0)
                    return "Experience Years must be a non-negative number";
                if (columnName == "Languages" && string.IsNullOrWhiteSpace(Languages))
                    return "Languages field is required";
                return string.Empty;
            }
        }

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _specialization = string.Empty;
        public string Specialization
        {
            get => _specialization;
            set => SetProperty(ref _specialization, value);
        }

        private int _experienceYears = 0;
        public int ExperienceYears
        {
            get => _experienceYears;
            set => SetProperty(ref _experienceYears, value);
        }

        private string _languages = string.Empty;
        public string Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set => SetProperty(ref _response, value);
        }

        public AddGuideViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private ICommand? _save;
        public ICommand Save => _save ??= new RelayCommand<object>(SaveGuide);

        private void SaveGuide(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var newGuide = new Guide
            {
                FirstName = FirstName,
                LastName = LastName,
                Specialization = Specialization,
                ExperienceYears = ExperienceYears,
                Languages = Languages
            };

            _context.Guides.Add(newGuide);
            _context.SaveChanges();

            Response = "Guide successfully added";
        }

        private ICommand? _back;
        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.GuidesSubView = new GuidesViewModel(_context, _dialogService);
            }
        }

        private bool IsValid()
        {
            string[] properties = { "FirstName", "LastName", "Specialization", "ExperienceYears", "Languages" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
