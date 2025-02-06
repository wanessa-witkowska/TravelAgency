using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{

    public class Guide
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specjalizacja jest wymagana.")]
        [StringLength(100, ErrorMessage = "Specjalizacja nie może przekraczać 100 znaków.")]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Lata doświadczenia muszą być w zakresie 0-100.")]
        public int ExperienceYears { get; set; } = 0;

        [Required(ErrorMessage = "Języki są wymagane.")]
        [StringLength(200, ErrorMessage = "Lista języków nie może przekraczać 200 znaków.")]
        public string Languages { get; set; } = string.Empty;

        public virtual ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
    }


}
