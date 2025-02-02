using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;
using TravelAgency.Data;
using TravelAgency.Interfaces;

namespace TravelAgency.ViewModels
{
    public class AddTourViewModel : ViewModelBase
    {
        private readonly travelAgencyContext _context;
        private readonly IDialogService _dialogService;
        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name" && string.IsNullOrEmpty(Name))
                {
                    return "Tour name is required.";
                }
                if (columnName == "Description" && string.IsNullOrEmpty(Description))
                {
                    return "Tour description is required.";
                }
                if (columnName == "StartDate" && StartDate == default)
                {
                    return "Start date is required.";
                }
                if (columnName == "EndDate" && EndDate == default)
                {
                    return "End date is required.";
                }
                if (columnName == "Price" && Price <= 0)
                {
                    return "Price must be greater than zero.";
                }
                if (columnName == "Destination" && string.IsNullOrEmpty(Destination))
                {
                    return "Destination is required.";
                }
                return string.Empty;
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _startDate = default;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate = default;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        private decimal _price = 0;
        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private string _destination = string.Empty;
        public string Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set => SetProperty(ref _response, value);
        }

        private ICommand? _backCommand = null;
        public ICommand? BackCommand
        {
            get
            {
                if (_backCommand is null)
                {
                    _backCommand = new RelayCommand<object>(NavigateBack);
                }
                return _backCommand;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.LocationsSubView = new ToursViewModel(_context, _dialogService);
            }
        }

        private ICommand? _saveCommand = null;
        public ICommand? SaveCommand
        {
            get
            {
                if (_saveCommand is null)
                {
                    _saveCommand = new RelayCommand<object>(SaveTour);
                }
                return _saveCommand;
            }
        }

        private void SaveTour(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields.";
                return;
            }

            Tour newTour = new Tour
            {
                Name = this.Name,
                Description = this.Description,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Price = this.Price,
                Destination = this.Destination
            };

            _context.Tours.Add(newTour);
            _context.SaveChanges();

            Response = "Tour saved successfully.";
        }

        private bool IsValid()
        {
            string[] properties = { "Name", "Description", "StartDate", "EndDate", "Price", "Destination" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        public AddTourViewModel(travelAgencyContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }
    }
}
