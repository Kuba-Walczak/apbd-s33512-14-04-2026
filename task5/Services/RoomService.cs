using task5.Enums;
using task5.Models;
using task5.Repositories;

namespace task5.Services;

public class RoomService : IRoomService {
    public Room? GetById(int id) {
        return DataStorage.Rooms.FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Room> GetByBuildingCode(string buildingCode) {
        return DataStorage.Rooms.Where(r => r.BuildingCode == buildingCode);
    }

    public IEnumerable<Room> GetFiltered(int? minCapacity, bool? hasProjector, bool? activeOnly) {
        var query = DataStorage.Rooms.AsEnumerable();
        if (minCapacity.HasValue) {
            query = query.Where(r => r.Capacity >= minCapacity.Value);
        }
        if (hasProjector.HasValue) {
            query = query.Where(r => r.HasProjector == hasProjector.Value);
        }
        if (activeOnly.HasValue) {
            query = query.Where(r => r.IsActive == activeOnly.Value);
        }
        return query;
    }

    public void Add(Room room) {
        room.Id = DataStorage.NextRoomId;
        DataStorage.Rooms.Add(room);
    }

    public bool Update(int id, Room room) {
        var existing = DataStorage.Rooms.FirstOrDefault(r => r.Id == id);
        if (existing == null) {
            return false;
        }
        existing.Name = room.Name;
        existing.BuildingCode = room.BuildingCode;
        existing.Floor = room.Floor;
        existing.Capacity = room.Capacity;
        existing.HasProjector = room.HasProjector;
        existing.IsActive = room.IsActive;
        return true;
    }

    public ResponseResult Delete(int id) {
        var existing = DataStorage.Rooms.FirstOrDefault(r => r.Id == id);
        if (existing == null) {
            return ResponseResult.NotFound;
        }
        var hasReservations = DataStorage.Reservations.Any(r =>
            r.RoomId == id && r.Status != ReservationStatus.Cancelled);
        if (hasReservations) {
            return ResponseResult.Conflict;
        }
        DataStorage.Rooms.Remove(existing);
        return ResponseResult.Success;
    }
}