using CoworkingApi.src.Data.Entities;
using CoworkingApi.src.Services.Entities;

namespace CoworkingApi.src.Repositories.Interfaces;

public interface IReservationRepository
{
    Task<Reservation> Get(long id);
    Task<IEnumerable<Reservation>> GetByRoom(long roomId);
    Task<IEnumerable<Reservation>> Get(long roomid, DateTime start, DateTime end);
    Task<Reservation> Add(Reservation reservation, long roomId, long userId);
    Task Delete(long id);
    Task<IEnumerable<Reservation>> GetReservationHours(long roomid, DateOnly date);
}
