using CoworkingApi.src.Data.Entities;

namespace CoworkingApi.src.Data.Seeds;

public static class DataSeed
{
    public static async Task SeedRooms(DataContext context, ILogger logger)
    {
        if (context.Rooms.Any())
            return;
        logger.LogInformation("[CoworkingApi] Starting seeding rooms...");

        for(int i = 1; i <= 22; i++)
        {
            await context.Rooms.AddAsync(new Room
            {
                Building = "А",
                Floor = 1,
                RoomNumber = i.ToString(),
                Name = "Комната",
                Seats = 10,
                RoomImage = "",
                Description = "Комната для собраний группы людей"
            });
        }
        await context.Rooms.AddAsync(new Room
        {
            Building = "А",
            Floor = 1,
            RoomNumber = "23",
            Name = "Конференц-зал",
            Seats = 150,
            RoomImage = "",
            Description = "Зал для проведения собраний на большую аудиторию"
        });

        int num = await context.SaveChangesAsync();
        logger.LogInformation("[CoworkingApi] Added {num} room entities...", num);
    }
}
