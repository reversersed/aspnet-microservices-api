using CoworkingApi.src.Data;
using CoworkingApi.src.Data.Entities;
using CoworkingApi.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ResponsePackage;

namespace CoworkingApi.src.Repositories;

public class RoomRepository(DataContext dataContext) : IRoomRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<Room> Add(Room room)
    {
        var entity = await _dataContext.Rooms.AddAsync(room);
        await _dataContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task Delete(long id)
    {
        var result = await _dataContext.Rooms.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (result == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Комнаты не существует");
        await _dataContext.SaveChangesAsync();
    }

    public async Task<Room> Get(long id)
    {
        return await _dataContext.Rooms.FindAsync(id) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Комнаты не существует");
    }

    public async Task<IEnumerable<Room>> Get(string building, int floor)
    {
        return await _dataContext.Rooms.Where(x => x.Building.Equals(building) && x.Floor == floor).ToListAsync();
    }

    public async Task<Room> Update(Room room)
    {
        var entity = _dataContext.Rooms.Update(room);
        await _dataContext.SaveChangesAsync();
        return entity.Entity;
    }
}
