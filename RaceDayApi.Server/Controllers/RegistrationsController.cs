using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.DTOs;
using RaceDayApi.Models;

namespace RaceDayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RegistrationsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public RegistrationsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<RegistrationResponseDto>> RegisterParticipant([FromBody] CreateRegistrationDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null) return NotFound(new { message = "Category not found." });

        var existingRegistration = await _context.Registrations
            .AnyAsync(r => r.UserId == userId && r.CategoryId == dto.CategoryId);

        if (existingRegistration)
            return BadRequest(new { message = "User is already registered for this category." });

        int nextRaceNumber = (await _context.Registrations
            .Where(r => r.CategoryId == dto.CategoryId)
            .MaxAsync(r => (int?)r.RaceNumber) ?? 100) + 1;

        var registration = new Registration
        {
            UserId = userId,
            CategoryId = dto.CategoryId,
            RaceNumber = nextRaceNumber
        };

        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);

        var response = new RegistrationResponseDto(
            registration.RegistrationId,
            userId,
            user!.FullName,
            category.CategoryId,
            category.Name,
            registration.RaceNumber
        );

        return StatusCode(201, response);
    }
}