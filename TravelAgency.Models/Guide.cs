using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    
    public class Guide
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Specjalizacja jest wymagana.")]
        [StringLength(100, ErrorMessage = "Specjalizacja nie może przekraczać 100 znaków.")]
        public string Specialization { get; set; }

        [Range(0, 100, ErrorMessage = "Lata doświadczenia muszą być w zakresie 0-100.")]
        public int ExperienceYears { get; set; }

        [Required(ErrorMessage = "Języki są wymagane.")]
        [StringLength(200, ErrorMessage = "Lista języków nie może przekraczać 200 znaków.")]
        public string Languages { get; set; }
    }


}
