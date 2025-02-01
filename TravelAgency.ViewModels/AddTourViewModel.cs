using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class AddTourViewModel : ViewModelBase
    {
        private string _name;
        private string _description;
        private DateTime _startDate;
        private DateTime _endDate;
        private decimal _price;
        private string _destination;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        public void SaveTour()
        {
            using (var context = new travelAgencyContext())
            {
                var newTour = new Tour
                {
                    Name = Name,
                    Description = Description,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Price = Price,
                    Destination = Destination
                };
                context.Tours.Add(newTour);
                context.SaveChanges();
            }
        }
    }
}
