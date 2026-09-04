using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.DTOs;
using RaceDayApi.Models;

namespace RaceDayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public CategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories([FromQuery] int? eventId)
    {
        var query = _context.Categories.AsQueryable();

        if (eventId.HasValue)
            query = query.Where(c => c.EventId == eventId.Value);

        var categories = await query
            .Select(c => new CategoryResponseDto(c.CategoryId, c.Name, c.EntryFee, c.EventId))
            .ToListAsync();

        return Ok(categories);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var eventExists = await _context.Events.AnyAsync(e => e.EventId == dto.EventId);
        if (!eventExists) return BadRequest(new { message = "Associated Event does not exist." });

        var category = new Category
        {
            Name = dto.Name,
            EntryFee = dto.EntryFee,
            EventId = dto.EventId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var response = new CategoryResponseDto(category.CategoryId, category.Name, category.EntryFee, category.EventId);
        return StatusCode(201, response);
    }
}