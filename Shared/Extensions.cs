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
    public static ItemToUpdateDto AsUpdateItemDto(this Item item) =>
    new ItemToUpdateDto(item.Name, item.Description, item.Price);

    /// <summary>
    /// Returns a new <see cref="ItemToCreateDto"/> corresponding with this <see cref="Item"/>
    /// </summary>
    public static ItemToCreateDto AsCreateItemDto(this Item item) =>
        new ItemToCreateDto(item.Name, item.Description, item.Price);
    
    /// <summary>
    /// Constructs a new <see cref="Item"/> from this <see cref="ItemToCreateDto"/>
    /// </summary>
    /// <param name="dto">The <see cref="ItemToCreateDto"/> from which the <see cref="Item"/> must be constructed.</param>
    /// <param name="id">The <see cref="Guid"/> to be used as the new <see cref="Item"/>'s <see cref="Item.Id"/>.</param>
    /// <param name="createdDate">The <see cref="DateTimeOffset"/> to be used as the new <see cref="Item"/>'s <see cref="Item.CreatedDate"/>.</param>
    public static Item ToItem(this ItemToCreateDto dto, Guid id, DateTimeOffset createdDate) =>
        new Item(id, dto.Name, dto.Description, dto.Price, createdDate);
}
