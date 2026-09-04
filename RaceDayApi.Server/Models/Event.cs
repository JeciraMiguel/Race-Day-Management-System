using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayApi.Models;

public class Event
{
    [Key]
    public int EventId { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    public int VenueId { get; set; }

    [ForeignKey(nameof(VenueId))]
    public Venue? Venue { get; set; }

    public ICollection<Category> Categories { get; set; } = new List<Category>();
}