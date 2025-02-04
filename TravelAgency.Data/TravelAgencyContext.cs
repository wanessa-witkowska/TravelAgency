using TravelAgency.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TravelAgency.Data
{
    public class travelAgencyContext : DbContext
    {
        public travelAgencyContext()
        {
        }

        public travelAgencyContext(DbContextOptions<travelAgencyContext> options) : base(options)
        {
        }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("travelAgencyDb");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tour>().Ignore(t => t.Guide);

            modelBuilder.Entity<Guide>().HasData(
                new Guide { Id = 1, FirstName = "Jan", LastName = "Kowalski", Specialization = "History", ExperienceYears = 5, Languages = "Polish, English" },
                new Guide { Id = 2, FirstName = "Anna", LastName = "Nowak", Specialization = "Nature", ExperienceYears = 3, Languages = "Polish, German" },
                new Guide { Id = 3, FirstName = "Marek", LastName = "Wiśniewski", Specialization = "Architecture", ExperienceYears = 7, Languages = "Polish, French" },
                new Guide { Id = 4, FirstName = "Ewa", LastName = "Dąbrowska", Specialization = "History, Architecture", ExperienceYears = 4, Languages = "Polish, Spanish" }
            );

            modelBuilder.Entity<Tour>().HasData(
                new Tour { Id = 1, Name = "Wycieczka po Warszawie", Description = "Zwiedzanie stolicy", StartDate = new DateTime(2025, 02, 01), EndDate = new DateTime(2025, 02, 02), Price = 150, Destination = "Warszawa", GuideId = 1 },
                new Tour { Id = 2, Name = "Wędrówki w Tatrach", Description = "Górska wycieczka", StartDate = new DateTime(2025, 03, 01), EndDate = new DateTime(2025, 03, 02), Price = 200, Destination = "Tatry", GuideId = 2 },
                new Tour { Id = 3, Name = "Zwiedzanie Krakowa", Description = "Zabytki i kultura", StartDate = new DateTime(2025, 04, 01), EndDate = new DateTime(2025, 04, 02), Price = 180, Destination = "Kraków", GuideId = 3 },
                new Tour { Id = 4, Name = "Przygoda na Mazurach", Description = "Jeziora i żeglarstwo", StartDate = new DateTime(2025, 05, 10), EndDate = new DateTime(2025, 05, 12), Price = 250, Destination = "Mazury", GuideId = 4 }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FirstName = "Adam", LastName = "Kowalski", Email = "adam@example.com", PhoneNumber = "123456789" },
                new Customer { Id = 2, FirstName = "Beata", LastName = "Nowak", Email = "beata@example.com", PhoneNumber = "987654321" },
                new Customer { Id = 3, FirstName = "Karol", LastName = "Nowicki", Email = "karol@example.com", PhoneNumber = "555666777" },
                new Customer { Id = 4, FirstName = "Magda", LastName = "Lewandowska", Email = "magda@example.com", PhoneNumber = "111222333" }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, TourId = 1, CustomerId = 1, BookingDate = new DateTime(2025, 01, 15), TourDate = new DateTime(2025, 02, 01), NumberOfParticipants = 2, TotalPrice = 300, Status = "Confirmed" },
                new Booking { Id = 2, TourId = 2, CustomerId = 2, BookingDate = new DateTime(2025, 01, 10), TourDate = new DateTime(2025, 03, 01), NumberOfParticipants = 1, TotalPrice = 200, Status = "Pending" },
                new Booking { Id = 3, TourId = 3, CustomerId = 3, BookingDate = new DateTime(2025, 01, 20), TourDate = new DateTime(2025, 04, 01), NumberOfParticipants = 3, TotalPrice = 540, Status = "Confirmed" },
                new Booking { Id = 4, TourId = 4, CustomerId = 4, BookingDate = new DateTime(2025, 02, 05), TourDate = new DateTime(2025, 05, 10), NumberOfParticipants = 2, TotalPrice = 500, Status = "Pending" }
            );

            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Muzeum Narodowe", Description = "Muzeum sztuki", Address = "Warszawa, Al. Jerozolimskie 3", PlaceType = "Museum" },
                new Location { Id = 2, Name = "Tatry", Description = "Pasmo górskie", Address = "Tatry, Zakopane", PlaceType = "Mountain" },
                new Location { Id = 3, Name = "Wawel", Description = "Zamek królewski", Address = "Kraków, Wawel 5", PlaceType = "Castle" },
                new Location { Id = 4, Name = "Mazury", Description = "Kraina wielkich jezior", Address = "Mazury, Giżycko", PlaceType = "Lake" }
            );
        }


        public void SaveData(string filePath)
        {
            var data = new
            {
                Tours = Tours.ToList(),
                Guides = Guides.ToList(),
                Customers = Customers.ToList(),
                Bookings = Bookings.ToList(),
                Locations = Locations.ToList()
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var jsonData = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, jsonData);
        }

        public void LoadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var jsonData = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<BookingData>(jsonData, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });

            if (data != null)
            {
                Tours.AddRange(data.Tours);
                Guides.AddRange(data.Guides);
                Customers.AddRange(data.Customers);
                Bookings.AddRange(data.Bookings);
                Locations.AddRange(data.Locations);
                SaveChanges();
            }
        }

        private class BookingData
        {
            public List<Tour> Tours { get; set; }
            public List<Guide> Guides { get; set; }
            public List<Customer> Customers { get; set; }
            public List<Booking> Bookings { get; set; }
            public List<Location> Locations { get; set; }
        }

    }
}
