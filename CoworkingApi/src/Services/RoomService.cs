using CoworkingApi.src.Data.Entities;
using CoworkingApi.src.Repositories.Interfaces;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using Mapster;
using ResponsePackage;

namespace CoworkingApi.src.Services;

public class RoomService(IRoomRepository roomRepository) : IRoomService
{
    private readonly IRoomRepository _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));

    public async Task<RoomDTO> Add(RoomDTO room)
    {
        var entity = await _roomRepository.Add(room.Adapt<Room>());
        return entity.Adapt<RoomDTO>();
    }

    public async Task Delete(long id)
    {
        var entity = await _roomRepository.Get(id);
        if(File.Exists(entity.RoomImage))
            File.Delete(entity.RoomImage);
        await _roomRepository.Delete(id);
    }
    public async Task<RoomDTO> Get(long id) => (await _roomRepository.Get(id)).Adapt<RoomDTO>();

    public async Task<IEnumerable<RoomDTO>> Get(string building, int floor)
    {
        var response = (await _roomRepository.Get(building, floor));
        if (!response.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Получена пустая коллекция");
        return response.Adapt<IEnumerable<RoomDTO>>();
    }

    public async Task<RoomDTO> Update(RoomDTO room, bool replace_image = false)
    {
        if(replace_image)
        {
            var entity = await _roomRepository.Get(room.Id);
            if (File.Exists(entity.RoomImage))
                File.Delete(entity.RoomImage);
        }
        return (await _roomRepository.Update(room.Adapt<Room>())).Adapt<RoomDTO>();
    }
}