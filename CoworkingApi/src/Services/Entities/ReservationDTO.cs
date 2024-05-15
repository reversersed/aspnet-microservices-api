using System.Text.Json.Serialization;

namespace CoworkingApi.src.Services.Entities;

public class ReservationDTO
{
    public long Id { get; set; }
    [JsonIgnore]
    public RoomDTO Room { get; set; }
    public string Holder { get; set; }
    public DateTime ReservationStart { get; set; }
    public DateTime ReservationEnd { get; set; }
    public string Purpose { get; set; }
}
