using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Receptionist")]
public class ReceptionistController : ControllerBase
{
    private readonly RoomService _roomService;
    private readonly ReservationService _reservationService;
    private readonly EmailService _emailService;

    public ReceptionistController(RoomService roomService, ReservationService reservationService, EmailService emailService)
    {
        _roomService = roomService;
        _reservationService = reservationService;
        _emailService = emailService;
    }

    [HttpGet("available-rooms")]
    public async Task<ActionResult<IEnumerable<Room>>> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var rooms = await _roomService.GetAvailableRoomsDetailedAsync(startDate, endDate);
        return Ok(rooms);
    }

    [HttpPut("cancel-reservation/{reservationId}")]
    public async Task<IActionResult> CancelReservation(int reservationId, [FromQuery] bool refund)
    {
        var result = await _reservationService.CancelReservationByReceptionist(reservationId, refund);
        if (!result)
        {
            return BadRequest("Unable to cancel reservation or it's too late to cancel without refund.");
        }
        return Ok("Reservation cancelled successfully.");
    }

    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] int reservationId)
    {
        await _reservationService.CheckIn(reservationId);
        return Ok("Check-in processed.");
    }

    [HttpPost("check-out")]
    public async Task<IActionResult> CheckOut([FromBody] int reservationId)
    {
        await _reservationService.CheckOut(reservationId);
        return Ok("Check-out processed and room marked for cleaning.");
    }

    [HttpPost("send-post-stay-email")]
    public async Task<IActionResult> SendPostStayEmail([FromBody] string customerEmail)
    {
        _emailService.SendPostStayEmail(customerEmail, "Thank you for staying with us! Please give us your feedback.");
        return Ok("Email sent.");
    }
}
