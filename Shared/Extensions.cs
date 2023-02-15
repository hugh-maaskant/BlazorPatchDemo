using BlazorPatchDemo.Shared.Dtos;
using BlazorPatchDemo.Shared.Entities;

namespace BlazorPatchDemo.Shared;

public static class Extensions
{
    /// <summary>
    /// Returns a new <see cref="ItemDto"/> corresponding with this <see cref="Item"/>
    /// </summary>
    public static ItemDto AsDto(this Item item) =>
        new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    
    /// <summary>
    /// Constructs a new <see cref="Item"/> from this <see cref="ItemDto"/>
    /// </summary>
    public static Item ToItem(this ItemDto dto) =>
        new Item(dto.Id, dto.Name, dto.Description, dto.Price, dto.CreatedDate);
    
    /// <summary>
    /// Returns a new <see cref="ItemToUpdateDto"/> corresponding with this <see cref="Item"/>
    /// </summary>
    public static ItemToUpdateDto AsUpdateDto(this Item item) =>
    new ItemToUpdateDto(item.Name, item.Description, item.Price);

    /// <summary>
    /// Returns a new <see cref="ItemToCreateDto"/> corresponding with this <see cref="Item"/>
    /// </summary>
    public static ItemToCreateDto AsCreateDto(this Item item) =>
        new ItemToCreateDto(item.Name, item.Description, item.Price);
    
    /// <summary>
    /// Constructs a new <see cref="Item"/> from this <see cref="ItemToCreateDto"/>
    /// </summary>
    public static Item ToItem(this ItemToCreateDto dto, Guid id, DateTimeOffset createdDate) =>
        new Item(id, dto.Name, dto.Description, dto.Price, createdDate);
}
