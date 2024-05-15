using EventsApi.src.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.src.Data.Seeds;

public static class SeedDataContext
{
public static async Task SeedEvents(DataContext context, ILogger logger)
	{
		if (context.Events.Any())
			return;
		logger.LogInformation("[EventsApi] Starting seeding events...");
		var creator = new Creator { Id = 1, Username = "admin" };
		await context.Creators.AddAsync(creator);

        var events = new Event[]
            {
				new(){ 
					Category = await context.Categories.FirstAsync(), 
					Title = "123", 
					Description = "123 Description", 
					Creator = creator, 
					Speaker = "Глад Валакас", 
					StartDate = DateTime.UtcNow.AddDays(54), 
					Seats = 10,
					EventImage=string.Empty
				},
				new(){ 
					Category = await context.Categories.OrderBy(x => x.Id).LastAsync(), 
					Title = "456", 
					Description = "456 Description",
					Creator = creator, 
					Speaker = "Никита Пономаренко", 
					StartDate = DateTime.UtcNow.AddDays(27),
					Created = DateTime.UtcNow.AddDays(2),
					Seats = 100,
                    EventImage=string.Empty
                }
            };
        await context.Events.AddRangeAsync(events);
		var comments = new Comment[]
		{
			new(){ Content = "К нему я точно пойду!", Creator = creator, Event = events[0] },
			new(){ Content = "Да и к нему тоже!", Creator= creator, Event = events[0] },
		};
		await context.Comments.AddRangeAsync(comments);

		int num = await context.SaveChangesAsync();
		logger.LogInformation("[EventsApi] Seeded {added_entities} entities", num);
	}
	public static async Task SeedCategories(DataContext context, ILogger logger)
	{
		if (context.Categories.Any())
			return;
		logger.LogInformation("[EventsApi] Starting seeding categories...");

		var categories = new Category[]
		{
			new() { Name = "Музыка" },
			new() { Name = "Концерты" },
			new() { Name = "Искусство и культура" },
			new() { Name = "Экскурсии" },
			new() { Name = "Вечеринки" },
			new() { Name = "Бизнес" },
			new() { Name = "Психология и самопознание" },
			new() { Name = "Наука" },
			new() { Name = "ИТ и интернет" },
			new() { Name = "Другие события" },
			new() { Name = "Спорт" },
			new() { Name = "Выставки" },
			new() { Name = "Интеллектуальные игры" },
			new() { Name = "Хобби и творчество" },
			new() { Name = "Кино" },
			new() { Name = "Еда" },
			new() { Name = "Иностранные языки" },
			new() { Name = "Презентации" }
		};
		await context.Categories.AddRangeAsync(categories);
		int added = await context.SaveChangesAsync();
		logger.LogInformation("[EventsApi] Seeded {newentities} entities", added);
	}
}