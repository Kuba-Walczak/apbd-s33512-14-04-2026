using task5.Enums;
using task5.Models;

namespace task5.Services;

public interface IReservationService {
    Reservation? GetById(int id);
    IEnumerable<Reservation> GetFiltered(DateOnly? date, ReservationStatus? status, int? roomId);
    ResponseResult Add(Reservation reservation, out Reservation? created);
    ResponseResult Update(int id, Reservation reservation);
    bool Delete(int id);
}