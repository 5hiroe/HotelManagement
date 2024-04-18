public class RoomReservation
{
    public int RoomReservationId { get; set; }
    public int RoomReservationRoomId { get; set; }
    public Room RoomReservationRoom { get; set; }
    public int RoomReservationReservationId { get; set; }
    public Reservation RoomReservationReservation { get; set; }
}