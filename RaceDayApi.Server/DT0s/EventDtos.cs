namespace RaceDayApi.DTOs;

public record CreateEventDto(string Title, string Description, DateTime EventDate, int VenueId);
public record UpdateEventDto(string Title, string Description, DateTime EventDate, int VenueId);
public record EventResponseDto(int EventId, string Title, string Description, DateTime EventDate, int VenueId, string VenueName);