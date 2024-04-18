using System.ComponentModel.DataAnnotations;

public class RoomReservationDto
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public CreditCardInfo CreditCardInfo { get; set; }

    [Required]
    public int CustomerId { get; set; }
}

public class CreditCardInfo
{
    [Required, CreditCard]
    public string Number { get; set; }

    [Required, DataType(DataType.Date)]
    public string ExpiryDate { get; set; }

    [Required, Range(100, 999)]
    public string CVV { get; set; }
}
