using CoworkingApi.src.Data.Entities;
using CoworkingApi.src.Repositories;
using CoworkingApi.src.Repositories.Interfaces;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using Extensions.HttpExtension.Interfaces;
using Mapster;
using ResponsePackage;

namespace CoworkingApi.src.Services;

public class ReservationService(IReservationRepository reservationRepository, IInternalHttpClient httpClient, IHolderRepository holderRepository) : IReservationService
{
    private readonly IReservationRepository _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
    private readonly IInternalHttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly IHolderRepository _holderRepository = holderRepository ?? throw new ArgumentNullException(nameof(holderRepository));

    public async Task<ReservationDTO> Add(ReservationDTO reservation)
    {
        var data = reservation.Adapt<Reservation>();

        var userData = await _httpClient.SendGet<UserResponse>("identityapi", "byname", ("name", reservation.Holder));
        if (userData.Code != ResponseCodes.DataFound || userData.Data is null)
            throw new CustomExceptionResponse(ResponseCodes.UserNotFound, userData.Message ?? string.Empty);
        await _holderRepository.TryAddHolder(userData.Data.Id, userData.Data.UserName);

        var entity = await _reservationRepository.Add(data, reservation.Room.Id, userData.Data.Id);
        return entity.Adapt<ReservationDTO>();
    }
    public async Task<bool[]> GetReservationHours(long roomid, DateOnly date)
    {
        var response = await _reservationRepository.GetReservationHours(roomid, date);
        bool[] reservations = new bool[24];
        foreach (var item in response)
            for(int hour = item.ReservationStart.Hour; hour < item.ReservationEnd.Hour; hour++)
                reservations[hour] = true;

        return reservations;
    }
    public async Task Delete(long id)
    {
        await _reservationRepository.Delete(id);
    }

    public async Task<ReservationDTO> Get(long id)
    {
        return (await _reservationRepository.Get(id)).Adapt<ReservationDTO>();
    }

    public async Task<IEnumerable<ReservationDTO>> Get(long roomid, DateTime start, DateTime end)
    {
        var response =  (await _reservationRepository.Get(roomid, start, end)).Adapt<IEnumerable<ReservationDTO>>();
        if (!response.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Получена пустая коллекция");
        return response;
    }

    public async Task<IEnumerable<ReservationDTO>> GetByRoom(long room_id)
    {
        var response = (await _reservationRepository.GetByRoom(room_id)).Adapt<IEnumerable<ReservationDTO>>();
        if (!response.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Получена пустая коллекция");
        return response;

    }

    public async Task CreateReservation(ReservationDTO reservation, long roomid)
    {
        var res = await reservationRepository.Get(roomid, reservation.ReservationStart, reservation.ReservationEnd);
        if (res.Any())
            throw new CustomExceptionResponse(ResponseCodes.NotUnique, "На данное время уже есть бронирования");

        var data = reservation.Adapt<Reservation>() ?? throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, "Не удается сконвертировать объекты");

        var userData = await _httpClient.SendGet<UserResponse>("identityapi", "byname", ("name", reservation.Holder));
        if (userData.Code != ResponseCodes.DataFound || userData.Data is null)
            throw new CustomExceptionResponse(ResponseCodes.UserNotFound, userData.Message ?? string.Empty);
        await _holderRepository.TryAddHolder(userData.Data.Id, userData.Data.UserName);

        await _reservationRepository.Add(data, roomid, userData.Data.Id);
    }
}