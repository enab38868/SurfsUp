using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUpProjekt.Models;

public class Board
{
    public int Id { get; set; }
    
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Name { get; set; }
   
    [Range(1, 100)]
    [Display(Name = "Length(Feet)")]
    public double Length { get; set; }
   
    [Range(1, 100)]
    [Display(Name = "Width(Inches)")]
    public double Width { get; set; }
    
    [Range(1, 10000)]
    [Display(Name = "Thickness(Inches)")]
    public double Thickness { get; set; }
    
    
    [Range(1.00, 1000.00)]
    [Display(Name = "Volume(L)")]
    public double Volume { get; set; }
    
    [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
    [StringLength(30)]
    [Required]
    public string Type { get; set; }

    [Display(Name = "Price(€)")]   
    //[DataType(DataType.Currency)]
    public double Price { get; set; }
    public string? Equipment { get; set; }

}