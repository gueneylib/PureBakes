namespace PureBakes.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ProductViewModel
{
    public Product Product { get; set; } = new();

    [ValidateNever]
    public IEnumerable<SelectListItem>? CategoryList { get; set; }
}