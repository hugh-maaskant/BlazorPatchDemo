using System.Net;
using System.Net.Http.Json;
using BlazorPatchDemo.Shared;
using BlazorPatchDemo.Shared.Dtos;
using BlazorPatchDemo.Shared.Entities;

namespace BlazorPatchDemo.Client.Infra;

internal sealed class SafeItemClient
{
    private const string ApplicationJson = "application/json";

    private readonly ILogger<SafeItemClient> _logger;
    private readonly HttpClient _client;

    
    public SafeItemClient(ILogger<SafeItemClient> logger, HttpClient client)
    {
        _logger = logger;
        _client = client;

        _logger.LogInformation("BaseAddress is {BaseAddress}", _client.BaseAddress);
    }

    /// <summary>
    /// Use HTTP GET to get all <see cref="Item"/>s from the Server
    /// </summary>
    /// <param name="token">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> of <see cref="ApiResult{T}"/> of a <see cref="List{T}"/> of <see cref="Item"/>s.
    /// </returns>
    internal async Task<ApiResult<List<Item>>> GetItemsAsync(CancellationToken token = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/items");
            request.Headers.Add("Accept", ApplicationJson);

            HttpResponseMessage response = await _client.SendAsync(request, token);

            if (response.IsSuccessStatusCode)
            {
                List<ItemDto>? dtos = await response.Content.ReadFromJsonAsync<List<ItemDto>>(cancellationToken: token);

                if (dtos is null)
                {
                    _logger.LogError(
                        "Error deserializing response to List<ItemDto>; StatusCode = {StatusCode}",
                        response.StatusCode);
                    return ApiResult<List<Item>>.Fail(
                        "Cannot deserialize response to List<ItemDto>", response.StatusCode);
                }
                
                // Convert every received ItemDto to an Item
                List<Item> items = dtos.Select(dto => dto.ToItem()).ToList();
                
                _logger.LogInformation("Retrieved {Count} Items", items.Count);
                return ApiResult<List<Item>>.Success(items, response.StatusCode);
            }

            _logger.LogError(
                "Error retrieving Items from the Server; StatusCode = {StatusCode}", response.StatusCode);
            return ApiResult<List<Item>>.Fail("Error Getting Items from the Server", response.StatusCode);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("Exception retrieving Items from Server, Message=\"{Message}\"; StatusCode={StatusCode}", 
                e.Message, e.StatusCode);
            return ApiResult<List<Item>>.Fail($"Exception retrieving Items from Server, Message=\"{e.Message}\"", e.StatusCode);
        }
    }

    /// <summary>
    /// Use HTTP POST to create an <see cref="Item"/> on the Server
    /// </summary>
    /// <param name="item">The <see cref="Item"/> to be created.</param>
    /// <param name="token">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>An <see cref="ApiResult{T}"/> of <see cref="Item"/>.</returns>
    /// <remarks>
    /// Even though the parameter is a fully formed <see cref="Item"/>, the Server may assign an <see cref="Item.Id"/>
    /// and <see cref="Item.CreatedDate"/>.
    /// Upon successful return, the <see cref="ApiResult{T}"/>.<see cref="ApiResult{T}.Value"/> represents a copy of the
    /// Server's <see cref="Item"/>, containing the final <see cref="Item.Id"/> and <see cref="Item.CreatedDate"/>.
    /// </remarks>
    internal async Task<ApiResult<Item>> PostItemAsync(Item item, CancellationToken token = default)
    {
        try
        {
            ItemToCreateDto itemToCreateDto = item.AsCreateItemDto();
            
            var response = await _client.PostAsJsonAsync("/items", itemToCreateDto, token);
            if (response.IsSuccessStatusCode)
            {
                ItemDto? dto = await response.Content.ReadFromJsonAsync<ItemDto>(cancellationToken: token);

                if (dto is not null)
                {
                    item = dto.ToItem();
                    
                    _logger.LogInformation(
                        "Item \"{Name}\" with Id {Id} created at Server on {CreatedDate} at {CreatedTime}",
                        dto.Name, 
                        item.Id,
                        item.CreatedDate.ToString("yyyy-MM-dd"),
                        item.CreatedDate.ToString("hh:mm:ss"));
                    return ApiResult<Item>.Success(item, response.StatusCode);
                }

                _logger.LogError("Error deserializing Item \"{Name}\"; Status = {StatusCode},",
                    itemToCreateDto.Name, response.StatusCode);
                return ApiResult<Item>.Fail($"Error deserializing Item \"{itemToCreateDto.Name}\"", response.StatusCode);
            }

            _logger.LogError("Error creating Item \"{Name}\"; Status = {StatusCode},",
                itemToCreateDto.Name, response.StatusCode);
            return ApiResult<Item>.Fail($"Error creating Item \"{itemToCreateDto.Name}\"", response.StatusCode);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("Exception creating Item \"{Name}\"; Message = \"{Message}\"; Status = {StatusCode}",
                item.Name, e.Message, e.StatusCode ?? HttpStatusCode.Unused);
            return ApiResult<Item>.Fail($"Exception creating {item.Name}; Message=\"{e.Message}\".", e.StatusCode);
        }
    }
    
    /// <summary>
    /// Use HTTP PUT to update an <see cref="Item"/> on the Server
    /// </summary>
    /// <param name="item">The updated <see cref="Item"/></param>
    /// <param name="token">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>An <see cref="ApiResult{T}"/> of <see cref="Item"/>.</returns>
    internal async Task<ApiResult<Item>> PutItemAsync(Item item, CancellationToken token = default)
    {
        ItemToUpdateDto dto = item.AsUpdateItemDto();

        try
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"items/{item.Id}", dto, cancellationToken: token);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Item \"{Name}\" has been updated at {Path}",
                    dto.Name, $"items/{item.Id}");
                return ApiResult<Item>.Success(item, response.StatusCode);
            }

            _logger.LogError("Error updating Item \"{Name}\" at {Path}, Status = {StatusCode},",
                dto.Name, $"items/{item.Id}", response.StatusCode);
            return ApiResult<Item>.Fail($"Error Putting Item \"{dto.Name}\" at items/{item.Id}", response.StatusCode);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("Error updating Item \"{Name}\" at {Path}, Message = {Message}; Status = {StatusCode}",
                dto.Name, $"items/{item.Id}", e.Message, e.StatusCode);
            return ApiResult<Item>.Fail(
                $"Exception updating {dto.Name} at at items/{item.Id}, Message=\"{e.Message}\".", e.StatusCode);
        }
    }

    /// <summary>
    /// Use HTTP DELETE to remove an <see cref="Item"/> from the Server
    /// </summary>
    /// <param name="item">The <see cref="Item"/>to be removed</param>
    /// <param name="token">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>An <see cref="ApiResult{T}"/> of <see cref="Item"/>.</returns>
    /// <remarks>
    /// If the <see cref="Item"/> does not exist on the Server, it may either return a
    /// <see cref="HttpStatusCode.NotFound"/> or an <see cref="HttpStatusCode.NoContent"/>.
    /// This operation returns the (deleted) <paramref name="item"/> as a convenience for the user.
    /// </remarks>
    internal async Task<ApiResult<Item>> DeleteItemAsync(Item item, CancellationToken token = default)
    {
        try
        {
            var response = await _client.DeleteAsync($"items/{item.Id}", token);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Item \"{Name}\" with Id {Id} has been deleted", item.Name, item.Id);
                return ApiResult<Item>.Success(item, response.StatusCode);
            }

            _logger.LogError("Error deleting Item \"{Name}\" with Id {Id}; Status = {StatusCode}",
                 item.Name, item.Id, response.StatusCode);
            return ApiResult<Item>.Fail($"Error deleting Item \"{item.Name}\" ({item.Id})", response.StatusCode);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("Exception deleting Item \"{Name}\" with Id {Id}, Message = {Message}; Status = {StatusCode}",
                 item.Name, item.Id, e.Message, e.StatusCode);
            return ApiResult<Item>.Fail(
                $"Exception deleting Item \"{item.Name}\" with {nameof(Item.Id)} {item.Id}); Message=\"{e.Message}\".",
                e.StatusCode);
        }
    }
}
