public class Reservation {
    public int ReservationId { get; set; }
    public int ReservationCustomerId { get; set; }
    public Customer ReservationCustomer { get; set; }
    public DateTime ReservationCheckInDate { get; set; }
    public DateTime ReservationCheckOutDate { get; set; }
    public bool ReservationIsCheckedIn { get; set; }
    public bool ReservationIsCheckedOut { get; set; }
    public List<RoomReservation> ReservationRoomReservations { get; set; } = new List<RoomReservation>();
    public decimal ReservationTotalPrice { get; set; }
    public bool ReservationIsCancelled { get; set; }
    public int ReservationNumberOfGuests { get; set; }
}