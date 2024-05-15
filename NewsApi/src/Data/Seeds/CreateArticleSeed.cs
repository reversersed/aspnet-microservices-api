using NewsApi.src.Data.Entities;

namespace NewsApi.src.Data.Seeds;

public static class CreateArticleSeed
{
    public static async Task SeedAsync(DataContext context, ILogger logger)
    {
        if (context.Articles.Any())
            return;
        logger.LogInformation("[NewsSeed] Starting seeding news...");

        var articles = new Article[]
        {
            new()
            {
                Title = "Опубликовали этапы хакатона «Код БИМ»",
                Content = "📆 Этапы Хакатон Tele2 «Код БИМ»:\r\n\r\n✅ 8 мая – Торжественное открытие и презентация кейсов для разработки;\r\n\r\n✅ 11 мая – Чек-поинт №1 Онлайн встреча с экспертами, консультация;\r\n\r\n✅ 13 мая – Чек-поинт №2 Онлайн встреча с экспертами, промежуточные результаты;\r\n\r\n✅ 15 мая – Очная защита проектов в формате презентации или видео. Награждение.\r\n\r\n⏰Точное время и ссылки  на трансляцию 🔗  будут опубликованы накануне чек-поинтов.",
                AuthorId = 1,
                Author = "admin"
            },
            new()
            {
                Title = "Кто же будет оценивать работы в хакатоне?",
                Content = "👨🏼‍💻Определит победителей Хакатаона экспертное жюри, состоящие из IT-специалистов ведущих компаний Ивановской области.\r\n\r\n❗️ Работы участников будут оцениваться по следующим критериям: \r\n\r\n✅ Степень готовности продукта;\r\n\r\n✅ Внешний вид и оформление приложения/IT решения;\r\n\r\n✅ Удобство и логика использования;\r\n\r\n✅ Реализация 3-х и более рекомендованных элементов (https://t.me/codeBIM/7);\r\n\r\n✅ Командная работа;\r\n\r\n✅ Качество презентации продукта и ответов на вопросы;\r\n\r\n✅ Применимость и перспективы.\r\n\r\n💵 Автор лучшей работы сможет забрать весь призовой фонд в размере 200 тысяч рублей.",
                AuthorId = 1,
                Comments = new List<Comment>
                {
                    new () { Content = "Ну и кто такую бредятину мог написать?", AuthorId = 1, Author = "admin" },
                    new () { Content = "Мда, техзадание лучше не придумаешь", AuthorId = 1, Author = "admin" }
                },
                Author = "admin"
            }
        };
        foreach (var article in articles)
            await context.Articles.AddAsync(article);
        await context.SaveChangesAsync();

        logger.LogInformation("[NewsSeed] Articles created");
    }
}