using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Housekeeper")]
public class HousekeepingController : ControllerBase
{
    private readonly RoomService _roomService;

    public HousekeepingController(RoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("rooms-to-clean")]
    public async Task<ActionResult<IEnumerable<Room>>> GetRoomsToClean()
    {
        var rooms = await _roomService.GetRoomsToCleanAsync();
        return Ok(rooms);
    }

    [HttpPost("mark-room-cleaned/{roomId}")]
    public async Task<IActionResult> MarkRoomCleaned(int roomId)
    {
        var result = await _roomService.MarkRoomAsCleaned(roomId);
        if (!result)
        {
            return BadRequest("Failed to mark the room as cleaned.");
        }
        return Ok("Room marked as cleaned.");
    }
}
