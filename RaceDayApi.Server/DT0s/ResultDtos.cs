namespace RaceDayApi.DTOs;

public record RecordTimingDto(int RegistrationId, TimeSpan FinishTime);
public record LeaderboardDto(int Rank, int RaceNumber, string ParticipantName, string CategoryName, TimeSpan FinishTime);