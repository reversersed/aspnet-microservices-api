using CoworkingApi.src.Data.Entities;

namespace CoworkingApi.src.Repositories.Interfaces;

public interface IRoomRepository
{
    Task<Room> Get(long id);
    Task<IEnumerable<Room>> Get(string building, int floor);
    Task<Room> Add(Room room);
    Task<Room> Update(Room room);
    Task Delete(long id);
}
