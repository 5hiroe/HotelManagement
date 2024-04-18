using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase {
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService) {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations() {
        var reservations = await _reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetReservation(int id) {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null) {
            return NotFound();
        }
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation, [FromBody] CreditCardInfo creditCardInfo) {
        try {
            var createdReservation = await _reservationService.CreateReservationAsync(reservation, creditCardInfo);
            return CreatedAtAction(nameof(GetReservation), new { id = createdReservation.ReservationId }, createdReservation);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(int id, Reservation reservation) {
        if (id != reservation.ReservationId) {
            return BadRequest();
        }
        try {
            await _reservationService.UpdateReservationAsync(reservation);
        } catch {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id) {
        await _reservationService.DeleteReservationAsync(id);
        return NoContent();
    }

    [HttpPut("cancel/{reservationId}")]
    public async Task<IActionResult> CancelReservation(int reservationId) {
        try {
            var result = await _reservationService.CancelReservation(reservationId);
            if (result) {
                return Ok("Reservation cancelled successfully");
            } else {
                return BadRequest("Unable to cancel reservation. It may be too late to cancel.");
            }
        } catch (Exception ex) {
            return StatusCode(500, "An error occurred while cancelling the reservation: "+ ex.Message);
        }
    }
}