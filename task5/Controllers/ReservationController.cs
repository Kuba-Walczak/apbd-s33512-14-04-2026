using Microsoft.AspNetCore.Mvc;
using task5.Enums;
using task5.Models;
using task5.Services;

namespace task5.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController(IReservationService reservationService) : ControllerBase {

    [HttpGet]
    public IActionResult GetReservations([FromQuery] DateOnly? date, [FromQuery] ReservationStatus? status, [FromQuery] int? roomId) {
        var reservations = reservationService.GetFiltered(date, status, roomId);
        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id) {
        var reservation = reservationService.GetById(id);
        if (reservation is null) {
            return NotFound();
        }
        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Reservation reservation) {
        var created = reservationService.Add(reservation);
        if (created is null) {
            return BadRequest();
        }
        return CreatedAtAction(nameof(GetById), new { created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Reservation reservation) {
        if (!reservationService.Update(id, reservation)) {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) {
        if (!reservationService.Delete(id)) {
            return NotFound();
        }
        return NoContent();
    }
}