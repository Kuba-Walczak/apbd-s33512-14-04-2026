using task5.Enums;
using task5.Models;
using task5.Repositories;

namespace task5.Services;

public class ReservationService : IReservationService {
    public Reservation? GetById(int id) {
        return DataStorage.Reservations.FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Reservation> GetFiltered(DateOnly? date, ReservationStatus? status, int? roomId) {
        var query = DataStorage.Reservations.AsEnumerable();
        if (date.HasValue) {
            query = query.Where(r => r.Date == date.Value);
        }
        if (status.HasValue) {
            query = query.Where(r => r.Status == status.Value);
        }
        if (roomId.HasValue) {
            query = query.Where(r => r.RoomId == roomId.Value);
        }
        return query;
    }

    public ResponseResult Add(Reservation reservation, out Reservation? created) {
        created = null;
        var room = DataStorage.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room == null) {
            return ResponseResult.NotFound;
        }
        if (!room.IsActive) {
            return ResponseResult.Conflict;
        }
        var hasOverlap = DataStorage.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.Status != ReservationStatus.Cancelled &&
            r.StartTime < reservation.EndTime &&
            reservation.StartTime < r.EndTime);
        if (hasOverlap) {
            return ResponseResult.Conflict;
        }
        reservation.Id = DataStorage.NextReservationId;
        DataStorage.Reservations.Add(reservation);
        created = reservation;
        return ResponseResult.Success;
    }

    public ResponseResult Update(int id, Reservation reservation) {
        var existing = DataStorage.Reservations.FirstOrDefault(r => r.Id == id);
        if (existing == null) {
            return ResponseResult.NotFound;
        }
        var room = DataStorage.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room == null) {
            return ResponseResult.NotFound;
        }
        if (!room.IsActive) {
            return ResponseResult.Conflict;
        }
        var hasOverlap = DataStorage.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.Status != ReservationStatus.Cancelled &&
            r.StartTime < reservation.EndTime &&
            reservation.StartTime < r.EndTime);
        if (hasOverlap) {
            return ResponseResult.Conflict;
        }
        existing.RoomId = reservation.RoomId;
        existing.OrganizerName = reservation.OrganizerName;
        existing.Topic = reservation.Topic;
        existing.Date = reservation.Date;
        existing.StartTime = reservation.StartTime;
        existing.EndTime = reservation.EndTime;
        existing.Status = reservation.Status;
        return ResponseResult.Success;
    }

    public bool Delete(int id) {
        var existing = DataStorage.Reservations.FirstOrDefault(r => r.Id == id);
        if (existing == null) {
            return false;
        }
        DataStorage.Reservations.Remove(existing);
        return true;
    }
}