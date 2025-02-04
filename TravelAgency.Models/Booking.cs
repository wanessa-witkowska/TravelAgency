using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{

    public class Booking
        {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "ID wycieczki jest wymagane.")]
        public int TourId { get; set; } = 0;

        [Required(ErrorMessage = "ID klienta jest wymagane.")]
        public int CustomerId { get; set; } = 0;

        [Required(ErrorMessage = "Data rezerwacji jest wymagana.")]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Data wycieczki jest wymagana.")]
        [DataType(DataType.Date, ErrorMessage = "Niepoprawny format daty.")]
        public DateTime TourDate { get; set; } = DateTime.Today;

        [Range(1, int.MaxValue, ErrorMessage = "Liczba uczestników musi być większa niż 0.")]
        public int NumberOfParticipants { get; set; } = 0;

        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0.")]
        public decimal TotalPrice { get; set; } = 0;

        [Required(ErrorMessage = "Status rezerwacji jest wymagany.")]
        [RegularExpression("^(Confirmed|Pending|Cancelled)$", ErrorMessage = "Niepoprawny status rezerwacji.")]
        public string Status { get; set; } = string.Empty;

        public virtual Tour Tour { get; set; } 
        public virtual Customer Customer { get; set; }
        }

    }

