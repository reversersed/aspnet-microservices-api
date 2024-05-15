using System.ComponentModel.DataAnnotations;

namespace CoworkingApi.src.Data.Entities;

public class Room
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Building { get; set; }
    [Required]
    public int Floor { get; set; }
    [Required]
    public string RoomNumber { get; set; }
    [Required]
    public int Seats { get; set; }
    [Required]
    public string RoomImage { get; set; }
    [Required]
    public string Description { get; set; }
}