using BlazorPatchDemo.Server.Entities;
using BlazorPatchDemo.Server.Interfaces;
using BlazorPatchDemo.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPatchDemo.Server.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private static readonly Random StaticRandom = new Random();
    private const int MaxRandom = 5;
    
    private readonly IRepository<Item> _itemsRepository;

    public ItemsController(IRepository<Item> itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
        int i = StaticRandom.Next(MaxRandom);
        if (i == 0) return Problem("Random failure in GetAsync");
        
        var items = (await _itemsRepository.GetAllAsync())
            .Select(item => item.AsDto());

        return Ok(items);
    }

    // GET /items/{id}
    [HttpGet("{id:guid}")]
    [ActionName("GetByIdAsync")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        int i = StaticRandom.Next(MaxRandom);
        if (i == 0) return Problem("Random failure in GetByIdAsync");

        var item = await _itemsRepository.GetAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return item.AsDto();
    }

    // POST /items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
    {
        int i = StaticRandom.Next(MaxRandom);
        if (i == 0) return Problem("Random failure in PostAsync");

        var item = new Item
        {
            Name = createItemDto.Name,
            Description = createItemDto.Description,
            Price = createItemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await _itemsRepository.CreateAsync(item);
            
        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    // PUT /items/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
    {
        int i = StaticRandom.Next(MaxRandom);
        if (i == 0) return Problem("Random failure in PutAsync");

        var existingItem = await _itemsRepository.GetAsync(id);

        if (existingItem == null)
        {
            return NotFound();
        }

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await _itemsRepository.UpdateAsync(existingItem);
            
        return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        int i = StaticRandom.Next(MaxRandom);
        if (i == 0) return Problem("Random failure in DeleteAsync");

        var item = await _itemsRepository.GetAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        await _itemsRepository.RemoveAsync(item.Id);
            
        return NoContent();
    }
}
