using task5.Enums;

namespace task5.Models;

public class Reservation {
    int Id { get; set; }
    int RoomId { get; set; }
    string OrganizerName { get; set; }
    string Topic { get; set; }
    DateTime Date { get; set; }
    TimeOnly StartTime { get; set; }
    TimeOnly EndTime { get; set; }
    ReservationStatus Status { get; set; }
}