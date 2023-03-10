using System.ComponentModel.DataAnnotations;
using BlazorPatchDemo.Shared.Interfaces;

namespace BlazorPatchDemo.Shared.Entities;

public sealed class Item : IEntity
{
    // !!! when changing these values, also change the validation ErrorMessage !!!
    
    public const int MinNameLength =  2;
    public const int MaxNameLength = 64;
    
    public const int MaxDescriptionLength = 512;

    public const int MinPrice =     0;
    public const int MaxPrice = 1_000;

    public Guid Id { get; init; }   // Note: init is required for MongoDB !!!
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(MaxNameLength, MinimumLength = MinNameLength, 
        ErrorMessage = "Name must be between 2 and 64 characters long")]
    public string Name { get; set; }
    
    [MaxLength(MaxDescriptionLength,
        ErrorMessage = "Description must be at most 512 characters long")]
    public string Description { get; set; }
    
    [Range(MinPrice, MaxPrice,
        ErrorMessage = "Price must be between 0 and 1000 inclusive")] 
    public decimal Price { get; set; }
    
    public DateTimeOffset CreatedDate { get; init; }    // Note: init is required for MongoDB !!!
    
    /// <summary>
    /// Construct a new <see cref="Item"/> with the initial Id and CreatedDate values
    /// and all other properties set to their default value.
    /// </summary>
    /// <param name="id">The Id of the Item</param>
    /// <param name="createdAt">The <see cref="DateTimeOffset"/> at which the Item was created</param>
    public Item(Guid id, DateTimeOffset createdAt) : 
        this(id, string.Empty, string.Empty, 0, createdAt)
    {
        // Nothing to do
    }
    
    /// <summary>
    /// Construct a new <see cref="Item"/> with the initial property values passed in.
    /// </summary>
    /// <param name="id">The Id of the Item.</param>
    /// <param name="name">The Name of the Item, max string length is 64 characters.</param>
    /// <param name="description">The Name of the Item, max string length is 512 characters.</param>
    /// <param name="price">The Price of the Item, must be between 0 and 1000.</param>
    /// <param name="createdAtUtc">The <see cref="DateTimeOffset"/> at which the Item was created.</param>
    /// <exception cref="ArgumentException">
    /// when <paramref name="name"/> or <paramref name="description"/> string length is outside its limit.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// when <paramref name="price"/> is outside its limit.
    /// </exception>
    public Item(Guid id, string name, string description, decimal price, DateTimeOffset createdAtUtc)
    {
        if (name.Length > MaxNameLength)
            throw new ArgumentException($"{nameof(Name)} cannot be longer than 64 characters ({name.Length}).");
        if (description.Length > MaxDescriptionLength)
            throw new ArgumentException($"{nameof(Description)} cannot be longer than 64 characters ({description.Length}).");
        switch (price)
        {
            case < MinPrice:
                throw new ArgumentOutOfRangeException(nameof(price), price, $"{nameof(Price)} too low (minimum = {MinPrice}.");
            case > MaxPrice:
                throw new ArgumentOutOfRangeException(nameof(price), price, $"{nameof(Price)} too high (maximum = {MaxPrice}.");
        }

        Id = id;
        Name = name;
        Description = description;
        Price = price;
        CreatedDate = createdAtUtc.ToUniversalTime();
    }

    /// <summary>
    /// Creates a full copy of this <see cref="Item"/>.
    /// </summary>
    /// <returns>A new <see cref="Item"/> instance with the same property values as this one</returns>
    public Item Clone() => new Item(Id, Name, Description, Price, CreatedDate);
}
