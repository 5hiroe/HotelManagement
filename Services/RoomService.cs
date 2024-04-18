using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class RoomService {
    private readonly HotelDbContext _context;

    public RoomService(HotelDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut) {
        return await _context.Rooms
            .Include(r => r.RoomReservations)
            .ThenInclude(rr => rr.RoomReservationReservation)
            .Where(r => !r.RoomReservations.Any(rr => rr.RoomReservationReservation.ReservationCheckInDate < checkOut && rr.RoomReservationReservation.ReservationCheckOutDate > checkIn))
            .ToListAsync();
    }

    public async Task<Room> GetRoomByIdAsync(int id) {
        Console.WriteLine("GetRoomByIdAsync");
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<Room> AddRoomAsync(Room room) {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<Room> UpdateRoomAsync(Room room) {
        _context.Entry(room).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task DeleteRoomAsync(int roomId) {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room != null) {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut) {
        var reservations = await _context.RoomReservations
            .Include(rr => rr.RoomReservationReservation)
            .Where(rr => rr.RoomReservationRoomId == roomId)
            .ToListAsync();
        
        return reservations.All(rr => rr.RoomReservationReservation.ReservationCheckInDate >= checkOut || rr.RoomReservationReservation.ReservationCheckOutDate <= checkIn);
    }

    public async Task<IEnumerable<RoomDetailDto>> GetAvailableRoomsDetailedAsync(DateTime checkIn, DateTime checkOut) {
        return await _context.Rooms
            .Where(r => !r.RoomReservations.Any(rr => rr.RoomReservationReservation.ReservationCheckInDate < checkOut && rr.RoomReservationReservation.ReservationCheckOutDate > checkIn))
            .Select(r => new RoomDetailDto {
                RoomId = r.RoomId,
                RoomType = r.RoomType,
                RoomPrice = r.RoomPrice,
                RoomCapacity = r.RoomCapacity,
            })
            .ToListAsync();
    }

    public async Task CheckIn(int reservationId) {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        reservation.ReservationIsCheckedIn = true;
        await _context.SaveChangesAsync();
    }

    public async Task CheckOut(int reservationId){
        var reservation = await _context.Reservations.FindAsync(reservationId);
        reservation.ReservationIsCheckedOut = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Room>> GetRoomsToCleanAsync() {
        return await _context.Rooms
            .Where(r => !r.RoomIsClean)
            .ToListAsync();
    }

    public async Task<bool> MarkRoomAsCleaned(int roomId) {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) {
            return false;
        }
        room.RoomIsClean = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkRoomAsDirty(int roomId) {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) {
            return false;
        }
        room.RoomIsClean = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateRoomCapacity(List<RoomReservation> roomReservation, int numberOfGuests) {
        foreach (var rr in roomReservation) {
            var room = await _context.Rooms.FindAsync(rr.RoomReservationRoomId);
            if (room == null || room.RoomCapacity < numberOfGuests) {
                return false;
            }
        }
        return true;
    }

}