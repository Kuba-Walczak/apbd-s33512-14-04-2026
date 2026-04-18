using Microsoft.AspNetCore.Mvc;
using task5.Enums;
using task5.Models;
using task5.Services;

namespace task5.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController(IReservationService reservationService) : ControllerBase {

    [HttpGet]
    public IActionResult GetReservations([FromQuery]DateOnly? date, [FromQuery]string? status, [FromQuery]int? roomId) {
        var reservations = reservationService.GetFiltered(date, status, roomId);
        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id) {
        var reservation = reservationService.GetById(id);
        if (reservation == null) {
            return NotFound("The specified reservation does not exist.");
        }
        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Add([FromBody]Reservation reservation) {
        var result = reservationService.Add(reservation, out var created);
        return result switch {
            ResponseResult.Success => CreatedAtAction(nameof(GetById), new { created!.Id }, created),
            ResponseResult.BadRequest => BadRequest("The specified room is inactive."),
            ResponseResult.Conflict => Conflict("The time slot overlaps with an existing reservation."),
            _ => NotFound("The specified room does not exist.")
        };
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody]Reservation reservation) {
        var result = reservationService.Update(id, reservation);
        return result switch {
            ResponseResult.Success => NoContent(),
            ResponseResult.BadRequest => BadRequest("The specified room is inactive."),
            ResponseResult.Conflict => Conflict("The time slot overlaps with an existing reservation."),
            _ => NotFound("The reservation or the specified room does not exist.")
        };
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) {
        if (!reservationService.Delete(id)) {
            return NotFound("The specified reservation does not exist.");
        }
        return NoContent();
    }
}