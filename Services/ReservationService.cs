using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ReservationService {
    private readonly HotelDbContext _context;
    private readonly EmailService _emailService;
    private readonly RoomService _roomService;
    private readonly PaymentService _paymentService;

    public ReservationService(HotelDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync() {
        return await _context.Reservations
        .Include(r => r.ReservationCustomer)
        .Include(r => r.ReservationRoomReservations)
        .ThenInclude(rr => rr.RoomReservationRoom)
        .ToListAsync();
    }

    public async Task<Reservation> GetReservationByIdAsync(int reservationId) {
        return await _context.Reservations
        .Include(r => r.ReservationCustomer)
        .Include(r => r.ReservationRoomReservations)
        .ThenInclude(rr => rr.RoomReservationRoom)
        .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation, CreditCardInfo creditCardInfo) {
        if (!await _roomService.ValidateRoomCapacity(reservation.ReservationRoomReservations, reservation.ReservationNumberOfGuests)) {
            throw new InvalidOperationException("Room capacity is insufficient for the number of guests.");
        }

        var paymentSuccessful = _paymentService.SimulatePayment(creditCardInfo);
        if (!paymentSuccessful) {
            throw new InvalidOperationException("Payment failed.");
        }

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }


    public async Task<Reservation> UpdateReservationAsync(Reservation reservation) {
        _context.Entry(reservation).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task DeleteReservationAsync(int reservationId) {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation != null) {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> CancelReservation(int reservationId) {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation == null) {
            return false;
        }
        if((reservation.ReservationCheckInDate - DateTime.UtcNow).TotalHours < 48)
        {
            return false;
        }
        reservation.ReservationIsCancelled = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelReservationByReceptionist(int reservationId, bool refund) {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation == null) return false;

        reservation.ReservationIsCancelled = true;
        if (refund || (reservation.ReservationCheckInDate - DateTime.UtcNow).TotalHours >= 48)
        {
            // Refund Process
        }
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task CheckOut(int reservationId) {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        reservation.ReservationIsCheckedOut = true;
        await _context.SaveChangesAsync();

        _emailService.SendPostStayEmail(reservation.ReservationCustomer.CustomerEmail, "Thank you for staying with us!");
    }

    public async Task CheckIn(int reservationId) {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        reservation.ReservationIsCheckedIn = true;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateRoomCapacity(int roomId, int numberOfGuests)
{
    var room = await _context.Rooms.FindAsync(roomId);
    return room != null && room.RoomCapacity >= numberOfGuests;
}

}