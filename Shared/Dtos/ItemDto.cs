using System.ComponentModel.DataAnnotations;
using BlazorPatchDemo.Shared.Entities;

namespace BlazorPatchDemo.Shared.Dtos;

/// <summary>
/// A value based representation of an <see cref="Item"/> for transport between systems
/// </summary>
public sealed record ItemDto(
    Guid Id,

    [Required]
    [MaxLength(Item.MaxNameLength)]
    string Name,

    [MaxLength(Item.MaxDescriptionLength)] string Description,

    [Range(Item.MinPrice, Item.MaxPrice)] decimal Price,

    DateTimeOffset CreatedDate
);
