using System.ComponentModel.DataAnnotations;
using task5.Enums;

namespace task5.Models;

public class Reservation : IValidatableObject {
    public int Id { get; set; }
    public int RoomId { get; set; }

    [Required]
    public string OrganizerName { get; set; } = null!;

    [Required]
    public string Topic { get; set; } = null!;

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public ReservationStatus Status { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
            yield return new ValidationResult("EndTime must be later than StartTime");
    }
}