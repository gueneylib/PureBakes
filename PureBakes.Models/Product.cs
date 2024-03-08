namespace PureBakes.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public double Price {get; set; }
    public int CategoryId { get; set; }

    [ValidateNever]
    public Category? Category { get; set; }
}