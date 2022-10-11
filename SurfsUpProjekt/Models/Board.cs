using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SurfsUpProjekt.Models;

public class Board
{
    public int Id { get; set; }
    
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Name { get; set; }

    
    [Range(1, 100)]
    [Display(Name = "Length(Feet)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public double Length { get; set; }
    
    [Range(1, 100)]
    [Display(Name = "Width(Inches)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public double Width { get; set; }
    
    [Range(1, 10000)]
    [Display(Name = "Thickness(Inches)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public double Thickness { get; set; }
    
    
    [Range(1.00, 1000.00)]
    [Display(Name = "Volume(L)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public double Volume { get; set; }
    
    [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
    [StringLength(30)]
    [Required]
    public string Type { get; set; }

    [Display(Name = "Price(€)")]   
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    //[DataType(DataType.Currency)]
    public double Price { get; set; }
    public string? Equipment { get; set; }

    public string? Image { get; set; }

    public virtual Rent? Rent { get; set; }

    // Concurrency locking
    [Timestamp]
    public byte[] RowVersion { get; set; }

    public bool IsRented { get; set; } // Default is false

    [ForeignKey("User")]
    public string? UserID { get; set; }

    public bool Premium { get; set; }

}