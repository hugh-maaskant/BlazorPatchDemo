using BlazorPatchDemo.Server.Interfaces;

namespace BlazorPatchDemo.Server.Entities;

public class Item : IEntity
{
    public Guid Id { get; init; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0;

    public DateTimeOffset CreatedDate { get; init; }
}
