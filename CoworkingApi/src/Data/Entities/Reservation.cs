using System.ComponentModel.DataAnnotations;
using CoworkingApi.src.Data.Entities;

namespace CoworkingApi.src.Data.Entities;

public class Reservation
{
    [Key]
    public long Id { get; set; }
    [Required]
    public Room Room { get; set; }
    [Required]
    public Holder Holder { get; set; }
    [Required]
    public DateTime ReservationStart { get; set; }
    [Required]
    public DateTime ReservationEnd { get; set; }
    [Required]
    public string Purpose { get; set; }
}
