using CoworkingApi.src.Data;
using CoworkingApi.src.Data.Entities;
using CoworkingApi.src.Repositories.Interfaces;
using CoworkingApi.src.Services.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ResponsePackage;

namespace CoworkingApi.src.Repositories;

public class ReservationRepository(DataContext dataContext) : IReservationRepository
{
    private readonly DataContext _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));

    public async Task<Reservation> Add(Reservation reservation, long roomId, long userId)
    {
        reservation.Room = await _dataContext.Rooms.FindAsync(roomId) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Комнаты не существует");
        reservation.Holder = await _dataContext.Holders.FindAsync(userId) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователя не существует");
        _dataContext.Attach(reservation.Room);
        _dataContext.Attach(reservation.Holder);
        var entity = await _dataContext.Reservations.AddAsync(reservation);
        await _dataContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task Delete(long id)
    {
        int rows = await _dataContext.Reservations.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (rows == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанной брони не существует");
        await _dataContext.SaveChangesAsync();
    }

    public async Task<Reservation> Get(long id)
    {
        return await _dataContext.Reservations.Where(x => x.Id == id).Include(x => x.Room).SingleOrDefaultAsync() ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанной брони не существует");
    }

    public async Task<IEnumerable<Reservation>> Get(long roomid, DateTime start, DateTime end)
    {
        return await _dataContext.Reservations.Include(x => x.Room).Where(x => x.Room.Id == roomid && x.ReservationEnd > start && x.ReservationStart < end).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByRoom(long roomId)
    {
        return await _dataContext.Reservations.Include(x => x.Room).Where(x => x.Room.Id == roomId).ToListAsync();
    }
    public async Task<IEnumerable<Reservation>> GetReservationHours(long roomid, DateOnly date)
    {
        return await _dataContext.Reservations.Include(x => x.Room).Where(x => x.Room.Id == roomid && DateOnly.FromDateTime(x.ReservationStart) == date).ToListAsync();
    }
}