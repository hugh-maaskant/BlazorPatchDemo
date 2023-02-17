using System.ComponentModel.DataAnnotations;
using BlazorPatchDemo.Shared.Entities;

namespace BlazorPatchDemo.Shared.Dtos;

/// <summary>
/// A value based representation of an <see cref="Item"/> to be created
/// </summary>
public sealed record ItemForCreateDto(
    
    [Required]
    [MaxLength(Item.MaxNameLength)] 
    string Name,
    
    [MaxLength(Item.MaxDescriptionLength)]
    string Description, 
    
    [Range(Item.MinPrice, Item.MaxPrice)] 
    decimal Price
);
