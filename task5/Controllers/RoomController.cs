using Microsoft.AspNetCore.Mvc;
using task5.Enums;
using task5.Models;
using task5.Services;

namespace task5.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController(IRoomService roomService) : ControllerBase {

    [HttpGet]
    public IActionResult GetRooms([FromQuery]int? minCapacity, [FromQuery]bool? hasProjector, [FromQuery]bool? activeOnly) {
        var rooms = roomService.GetFiltered(minCapacity, hasProjector, activeOnly);
        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id) {
        var room = roomService.GetById(id);
        if (room == null) {
            return NotFound();
        }
        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuildingCode(string buildingCode) {
        var rooms = roomService.GetByBuildingCode(buildingCode);
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Add([FromBody]Room room) {
        roomService.Add(room);
        return CreatedAtAction(nameof(GetById), new { room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody]Room room) {
        if (!roomService.Update(id, room)) {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) {
        var result = roomService.Delete(id);
        return result switch {
            ResponseResult.Success => NoContent(),
            ResponseResult.Conflict => Conflict("Cannot delete a room that has active reservations."),
            _ => NotFound()
        };
    }
}