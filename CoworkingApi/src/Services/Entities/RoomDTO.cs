using System.ComponentModel.DataAnnotations;

namespace CoworkingApi.src.Services.Entities;

public class RoomDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Building { get; set; }
    public int Floor { get; set; }
    public string RoomNumber { get; set; }
    public int Seats { get; set; }
    public string RoomImage { get; set; }
    public string Description { get; set; }
}
