using BlazorPatchDemo.Server.Interfaces;
using BlazorPatchDemo.Shared;
using BlazorPatchDemo.Shared.Dtos;
using BlazorPatchDemo.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPatchDemo.Server.Controllers;

[ApiController]
[Route("items")]
public sealed class ItemsController : ControllerBase
{
    private static readonly Random StaticRandom = new Random();
    private const int MaxRandom = 5;
    
    private static void FailRandomly()
    {
        if (StaticRandom.Next(MaxRandom) == 0) 
            throw new Exception("Random server fail (used for testing only)");
    }

    private readonly IRepository<Item> _itemsRepository;

    public ItemsController(IRepository<Item> itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
        FailRandomly();
        
        var items = (await _itemsRepository.GetAllAsync())
            .Select(item => item.AsDto());

        return Ok(items);
    }

    // GET /items/{id}
    [HttpGet("{id:guid}")]
    [ActionName("GetByIdAsync")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        FailRandomly();

        var item = await _itemsRepository.GetAsync(id);

        if (item == null)
            return NotFound();

        return item.AsDto();
    }

    // POST /items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(ItemToCreateDto itemToCreateDto)
    {
        FailRandomly();

        var item = itemToCreateDto.ToItem(Guid.NewGuid(), DateTimeOffset.UtcNow);

        await _itemsRepository.CreateAsync(item);
            
        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    // PUT /items/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, ItemToUpdateDto itemToUpdateDto)
    {
        FailRandomly();

        var existingItem = await _itemsRepository.GetAsync(id);

        if (existingItem is null)
        {
            return NotFound();
        }

        existingItem.Name = itemToUpdateDto.Name;
        existingItem.Description = itemToUpdateDto.Description;
        existingItem.Price = itemToUpdateDto.Price;

        await _itemsRepository.UpdateAsync(existingItem);
            
        return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        FailRandomly();

        await _itemsRepository.RemoveAsync(id);

        return NoContent();
    }
}
