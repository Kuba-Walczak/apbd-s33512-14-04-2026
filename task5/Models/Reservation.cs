using System.ComponentModel.DataAnnotations;

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
    public string Status { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (EndTime <= StartTime) {
            yield return new ValidationResult("EndTime must be later than StartTime");
        }
    }
}
