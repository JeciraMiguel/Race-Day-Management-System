using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.Server.DTOs;
using RaceDayApi.Models;

namespace RaceDayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEvents()
    {
        var events = await _context.Events
            .Include(e => e.Venue)
            .Select(e => new EventResponseDto(e.EventId, e.Title, e.Description, e.EventDate, e.VenueId, e.Venue!.Name))
            .ToListAsync();

        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponseDto>> GetEvent(int id)
    {
        var e = await _context.Events.Include(ev => ev.Venue).FirstOrDefaultAsync(ev => ev.EventId == id);
        if (e == null) return NotFound(new { message = $"Event with ID {id} not found." });

        return Ok(new EventResponseDto(e.EventId, e.Title, e.Description, e.EventDate, e.VenueId, e.Venue!.Name));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<ActionResult<EventResponseDto>> CreateEvent([FromBody] CreateEventDto dto)
    {
        var venueExists = await _context.Venues.AnyAsync(v => v.VenueId == dto.VenueId);
        if (!venueExists) return BadRequest(new { message = "Invalid VenueId provided." });

        var raceEvent = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            EventDate = dto.EventDate,
            VenueId = dto.VenueId
        };

        _context.Events.Add(raceEvent);
        await _context.SaveChangesAsync();

        var venue = await _context.Venues.FindAsync(dto.VenueId);

        var response = new EventResponseDto(raceEvent.EventId, raceEvent.Title, raceEvent.Description, raceEvent.EventDate, raceEvent.VenueId, venue!.Name);
        return CreatedAtAction(nameof(GetEvent), new { id = raceEvent.EventId }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto dto)
    {
        var raceEvent = await _context.Events.FindAsync(id);
        if (raceEvent == null) return NotFound(new { message = $"Event with ID {id} not found." });

        raceEvent.Title = dto.Title;
        raceEvent.Description = dto.Description;
        raceEvent.EventDate = dto.EventDate;
        raceEvent.VenueId = dto.VenueId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var raceEvent = await _context.Events.FindAsync(id);
        if (raceEvent == null) return NotFound(new { message = $"Event with ID {id} not found." });

        _context.Events.Remove(raceEvent);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}