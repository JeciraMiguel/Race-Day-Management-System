using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.Server.DTOs;
using RaceDayApi.Models;


namespace RaceDayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<IActionResult> RecordTiming([FromBody] RecordTimingDto dto)
    {
        var registration = await _context.Registrations.FindAsync(dto.RegistrationId);
        if (registration == null) return NotFound(new { message = "Registration record not found." });

        var existingResult = await _context.TimingResults.FirstOrDefaultAsync(t => t.RegistrationId == dto.RegistrationId);

        if (existingResult != null)
        {
            existingResult.FinishTime = dto.FinishTime;
            existingResult.RecordedAt = DateTime.UtcNow;
        }
        else
        {
            var timing = new TimingResult
            {
                RegistrationId = dto.RegistrationId,
                FinishTime = dto.FinishTime
            };
            _context.TimingResults.Add(timing);
        }

        await _context.SaveChangesAsync();
        await RecalculateRanksAsync(registration.CategoryId);

        return Ok(new { message = "Timing result recorded and leaderboard recalculated." });
    }

    [HttpGet("leaderboard/{categoryId}")]
    public async Task<ActionResult<IEnumerable<LeaderboardDto>>> GetLeaderboard(int categoryId)
    {
        var leaderboard = await _context.TimingResults
            .Include(t => t.Registration)
                .ThenInclude(r => r!.User)
            .Include(t => t.Registration)
                .ThenInclude(r => r!.Category)
            .Where(t => t.Registration!.CategoryId == categoryId)
            .OrderBy(t => t.FinishTime)
            .Select(t => new LeaderboardDto(
                t.OverallRank ?? 0,
                t.Registration!.RaceNumber,
                t.Registration.User!.FullName,
                t.Registration.Category!.Name,
                t.FinishTime
            ))
            .ToListAsync();

        return Ok(leaderboard);
    }

    private async Task RecalculateRanksAsync(int categoryId)
    {
        var results = await _context.TimingResults
            .Where(t => t.Registration!.CategoryId == categoryId)
            .OrderBy(t => t.FinishTime)
            .ToListAsync();

        int rank = 1;
        foreach (var result in results)
        {
            result.OverallRank = rank++;
        }

        await _context.SaveChangesAsync();
    }
}