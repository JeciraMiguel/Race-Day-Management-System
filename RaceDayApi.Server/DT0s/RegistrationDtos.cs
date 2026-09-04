namespace RaceDayApi.DTOs;

public record CreateRegistrationDto(int CategoryId);
public record RegistrationResponseDto(int RegistrationId, int UserId, string ParticipantName, int CategoryId, string CategoryName, int RaceNumber);