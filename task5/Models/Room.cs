using task5.Enums;

namespace task5.Models;

public class Room {
    int Id { get; set; }
    string Name { get; set; }
    int BuildingCode { get; set; }
    int Floor { get; set; }
    int Capacity { get; set; }
    bool HasProjector  { get; set; }
    bool IsActive { get; set; }
}