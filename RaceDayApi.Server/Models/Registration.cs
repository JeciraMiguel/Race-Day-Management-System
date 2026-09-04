using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayApi.Models;

public class Registration
{
    [Key]
    public int RegistrationId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    public int RaceNumber { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public TimingResult? TimingResult { get; set; }
}