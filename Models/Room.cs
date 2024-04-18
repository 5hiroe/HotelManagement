public class Room {
    public int RoomId { get; set; }
    public string RoomType { get; set; }
    public int RoomCapacity { get; set; }
    public decimal RoomRate { get; set; }
    public string RoomStatus { get; set; }
    public double RoomPrice { get; set; }
    public bool RoomIsClean { get; set; }
    public List<RoomReservation> RoomReservations { get; set; } = new List<RoomReservation>();
}