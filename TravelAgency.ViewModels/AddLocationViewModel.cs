using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.ViewModels
{
    public class AddLocationViewModel : ViewModelBase
    {
        private string _name;
        private string _description;
        private string _address;
        private string _placeType;

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

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public string PlaceType
        {
            get => _placeType;
            set => SetProperty(ref _placeType, value);
        }

        public void SaveLocation()
        {
            using (var context = new travelAgencyContext())
            {
                var newLocation = new Location
                {
                    Name = Name,
                    Description = Description,
                    Address = Address,
                    PlaceType = PlaceType
                };
                context.Locations.Add(newLocation);
                context.SaveChanges();
            }
        }
    }
}
