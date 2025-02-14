﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    public class Location
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Nazwa lokalizacji jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie może przekraczać 100 znaków.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis jest wymagany.")]
        [StringLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [StringLength(200, ErrorMessage = "Adres nie może przekraczać 200 znaków.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ miejsca jest wymagany.")]
        [RegularExpression("^(Museum|Mountain|Castle)$", ErrorMessage = "Typ miejsca musi być jednym z: Museum, Mountain, Castle.")]
        public string PlaceType { get; set; } = string.Empty;

        public int GuideId { get; set; }
        public virtual Guide? Guide { get; set; } 
    }

}
