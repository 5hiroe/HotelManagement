using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase {
    private readonly RoomService _roomService;
    private readonly ReservationService _reservationService;

    public RoomController(RoomService roomService) {
        _roomService = roomService;
    }


    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Room>>> GetAvailableRooms([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut) {
        var rooms = await _roomService.GetAvailableRoomsAsync(checkIn, checkOut);
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(int id) {
        var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null) {
            return NotFound();
        }
        return Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> CreateRoom(Room room) {
        var createdRoom = await _roomService.AddRoomAsync(room);
        return CreatedAtAction(nameof(GetRoom), new { id = createdRoom.RoomId }, createdRoom);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, Room room) {
        if (id != room.RoomId) {
            return BadRequest();
        }
        try {
            await _roomService.UpdateRoomAsync(room);
        } catch {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id) {
        await _roomService.DeleteRoomAsync(id);
        return NoContent();
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveRoom([FromBody] RoomReservationDto reservationDto)
    {
        var available = await _roomService.IsRoomAvailable(reservationDto.RoomId, reservationDto.CheckInDate, reservationDto.CheckOutDate);
        if (!available)
        {
            return BadRequest("Room is not available for the selected dates.");
        }

        var reservation = new Reservation
        {
            ReservationCheckInDate = reservationDto.CheckInDate,
            ReservationCheckOutDate = reservationDto.CheckOutDate,
            ReservationCustomerId = reservationDto.CustomerId,
            ReservationRoomReservations = new List<RoomReservation> {
                new RoomReservation {
                    RoomReservationRoomId = reservationDto.RoomId,
                    RoomReservationReservation = new Reservation {
                        ReservationCheckInDate = reservationDto.CheckInDate,
                        ReservationCheckOutDate = reservationDto.CheckOutDate
                    }
                }
            }
        };
        var createdReservation = await _reservationService.CreateReservationAsync(reservation, reservationDto.CreditCardInfo);
        if (createdReservation == null)
        {
            return BadRequest("Failed to create reservation.");
        }
        return CreatedAtAction(nameof(ReservationController.GetReservation), "Reservation", new { id = createdReservation.ReservationId }, createdReservation);
    }

    [HttpGet("available-detailed")]
    public async Task<IActionResult> GetAvailableRoomsDetailed([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var rooms = await _roomService.GetAvailableRoomsDetailedAsync(startDate, endDate);
        return Ok(rooms);
    }
    
}