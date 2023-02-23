using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using BlazorPatchDemo.Shared.Entities;

namespace BlazorPatchDemo.Shared.Dtos;

/// <summary>
/// A value based representation of an <see cref="Item"/> for transport between systems
/// </summary>
public sealed record ItemDto(

    [Required]
    Guid Id,

    [Required]
    [StringLength(Item.MaxNameLength, MinimumLength = Item.MinNameLength)]
    string Name,

    [MaxLength(Item.MaxDescriptionLength)]
    string Description,

    [Required]
    [Range(Item.MinPrice, Item.MaxPrice)]
    decimal Price,

    [Required]
    DateTimeOffset CreatedDate
);
