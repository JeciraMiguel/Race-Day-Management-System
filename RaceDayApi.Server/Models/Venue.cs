using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models;

public class Venue
{
    [Key]
    public int VenueId { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string Location { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public ICollection<Event> Events { get; set; } = new List<Event>();
}