using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayApi.Models;

public class TimingResult
{
    [Key]
    public int TimingResultId { get; set; }

    [Required]
    public int RegistrationId { get; set; }

    [ForeignKey(nameof(RegistrationId))]
    public Registration? Registration { get; set; }

    [Required]
    public TimeSpan FinishTime { get; set; }

    public int? OverallRank { get; set; }

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}