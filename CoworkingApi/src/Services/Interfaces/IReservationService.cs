using CoworkingApi.src.Data.Entities;
using CoworkingApi.src.Services.Entities;
using Org.BouncyCastle.Asn1.Ocsp;

namespace CoworkingApi.src.Services.Interfaces;

public interface IReservationService
{
    Task<ReservationDTO> Get(long id);
    Task<IEnumerable<ReservationDTO>> GetByRoom(long room_id);
    Task<IEnumerable<ReservationDTO>> Get(long roomid, DateTime start, DateTime end);
    Task<ReservationDTO> Add(ReservationDTO reservation);
    Task Delete(long id);
    Task<bool[]> GetReservationHours(long roomid, DateOnly date);
    Task CreateReservation(ReservationDTO reservation, long roomid);
}
