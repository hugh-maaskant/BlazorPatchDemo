using System.ComponentModel.DataAnnotations;
using BlazorPatchDemo.Shared.Entities;

namespace BlazorPatchDemo.Shared.Dtos;

/// <summary>
/// A value based representation of an <see cref="Item"/> to be updated
/// </summary>
public sealed record ItemToUpdateDto(
    
    [Required]
    [MaxLength(Item.MaxNameLength)] 
    string Name,
    
    [MaxLength(Item.MaxDescriptionLength)]
    string Description, 
    
    [Range(Item.MinPrice, Item.MaxPrice)] 
    decimal Price
);
