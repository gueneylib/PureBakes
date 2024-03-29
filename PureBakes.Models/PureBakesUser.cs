namespace PureBakes.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class PureBakesUser : IdentityUser
{
    [Required] public string Name { get; set; } = string.Empty;

    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
}