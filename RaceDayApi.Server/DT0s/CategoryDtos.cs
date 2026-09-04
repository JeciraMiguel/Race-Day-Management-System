namespace RaceDayApi.DTOs;

public record CreateCategoryDto(string Name, decimal EntryFee, int EventId);
public record CategoryResponseDto(int CategoryId, string Name, decimal EntryFee, int EventId);