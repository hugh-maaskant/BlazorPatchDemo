using BlazorPatchDemo.Shared.Dtos;
using BlazorPatchDemo.Shared.Entities;

namespace BlazorPatchDemo.Shared;

public static class Extensions
{
    //
    //  Convert from Item (Entity) to Dto
    //
    
    /// <summary>
    /// Returns a new <see cref="ItemDto"/> corresponding with this <see cref="Item"/>
    /// </summary>
    public static ItemDto ToItemDto(this Item item) =>
        new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    
    /// <summary>
    /// Returns a new <see cref="ItemForUpdateDto"/> corresponding with this <see cref="Item"/>
    /// </summary>
    public static ItemForUpdateDto ToItemForUpdateDto(this Item item) =>
    new ItemForUpdateDto(item.Name, item.Description, item.Price);

    /// <summary>
    /// Returns a new <see cref="ItemForCreateDto"/> corresponding with this <see cref="Item"/>
    /// </summary>
    public static ItemForCreateDto ToItemForCreateDto(this Item item) =>
        new ItemForCreateDto(item.Name, item.Description, item.Price);
    
    //
    // Convert from Dto to Item
    //
    
    /// <summary>
    /// Constructs a new <see cref="Item"/> from this <see cref="ItemDto"/>
    /// </summary>
    public static Item ToItem(this ItemDto dto) =>
        new Item(dto.Id, dto.Name, dto.Description, Math.Truncate(dto.Price), dto.CreatedDate);
    
    /// <summary>
    /// Constructs a new <see cref="Item"/> from this <see cref="ItemForCreateDto"/>
    /// </summary>
    /// <param name="dto">The <see cref="ItemForCreateDto"/> from which the <see cref="Item"/> must be constructed.</param>
    /// <param name="id">The <see cref="Guid"/> to be used as the new <see cref="Item"/>'s <see cref="Item.Id"/>.</param>
    /// <param name="createdDate">The <see cref="DateTimeOffset"/> to be used as the new <see cref="Item"/>'s <see cref="Item.CreatedDate"/>.</param>
    public static Item ToItem(this ItemForCreateDto dto, Guid id, DateTimeOffset createdDate) =>
        new Item(id, dto.Name, dto.Description, Math.Truncate(dto.Price), createdDate);
}
