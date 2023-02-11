using BlazorPatchDemo.Server.Entities;
using BlazorPatchDemo.Shared;

namespace BlazorPatchDemo.Server;

public static class Extensions
{
    public static ItemDto AsDto(this Item item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }
}