using System.ComponentModel.DataAnnotations;

namespace BlazorPatchDemo.Shared;

public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

public record UpdateItemDto([Required] Guid Id, [Required] string Name, string Description, [Range(0, 1000)] decimal Price);
