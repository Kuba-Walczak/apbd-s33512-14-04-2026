using task5.Enums;
using task5.Models;

namespace task5.Services;

public interface IRoomService {
    Room? GetById(int id);
    IEnumerable<Room> GetByBuildingCode(string buildingCode);
    IEnumerable<Room> GetFiltered(int? minCapacity, bool? hasProjector, bool? activeOnly);
    void Add(Room room);
    bool Update(int id, Room room);
    DeleteResult Delete(int id);
}