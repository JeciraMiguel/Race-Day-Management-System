namespace RaceDayApi.Server.DTOs;

public record RegisterDto(string FullName, string Email, string Password, string Role);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, string Email, string Role);