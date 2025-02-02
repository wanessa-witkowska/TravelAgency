using System;
using System.ComponentModel.DataAnnotations;
using TravelAgency.Models;

public class Tour
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa wycieczki jest wymagana.")]
    [StringLength(100, ErrorMessage = "Nazwa nie może przekraczać 100 znaków.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Opis jest wymagany.")]
    [StringLength(1000, ErrorMessage = "Opis nie może przekraczać 1000 znaków.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Data rozpoczęcia jest wymagana.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Data zakończenia jest wymagana.")]
    [DateGreaterThan(nameof(StartDate), ErrorMessage = "Data zakończenia musi być późniejsza niż data rozpoczęcia.")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Cena jest wymagana.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Miejsce docelowe jest wymagane.")]
    [StringLength(150, ErrorMessage = "Miejsce docelowe nie może przekraczać 150 znaków.")]
    public string Destination { get; set; }

    [Required(ErrorMessage = "Przewodnik jest wymagany.")]
    public int GuideId { get; set; }

    public virtual Guide Guide { get; set; }
}

public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
            return new ValidationResult($"Nieznana właściwość: {_comparisonProperty}");

        var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
        if (value is DateTime currentValue && currentValue <= comparisonValue)
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}