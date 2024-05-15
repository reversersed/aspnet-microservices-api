using CoworkingApi.src.Services.Entities;

namespace CoworkingApi.src.Services.Interfaces;

public interface IRoomService
{
    Task<RoomDTO> Get(long id);
    Task<IEnumerable<RoomDTO>> Get(string building, int floor);
    Task<RoomDTO> Add(RoomDTO room);
    Task<RoomDTO> Update(RoomDTO room, bool replace_image = false);
    Task Delete(long id);
}
