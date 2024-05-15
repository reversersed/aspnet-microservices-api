using Microsoft.EntityFrameworkCore;
using NewsApi.src.Data;
using NewsApi.src.Data.Entities;
using NewsApi.src.Repositories.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Repositories;

public class ArticleRepository(DataContext _context) : IArticleRepository
{
    private readonly DataContext context = _context ?? throw new ArgumentNullException(nameof(_context));

    public async Task<Article> Add(Article article)
    {
        var response = await context.Articles.AddAsync(article) ?? throw new ArgumentException("Произошла ошибка при создании статьи");
        await context.SaveChangesAsync();
        return response.Entity;
    }

    public async Task ChangeUsername(string oldname, string newname)
    {
        context.Articles.Where(x => x.Author.Equals(oldname)).ExecuteUpdate(x => x.SetProperty(x => x.Author, newname));
        await context.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
        var response = await context.Articles.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (response == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Данной статьи не существует");
        await context.SaveChangesAsync();
    }

    public async Task<Article> Get(long id)
    {
        var article = await context.Articles.FindAsync(id) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанной статьи не существует");
        return article;
    }
    public async Task<IEnumerable<Article>> Get(int offset = 0, int count = 10)
    {
        return await context.Articles.OrderByDescending(x => x.Created).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<Article> Patch(Article article)
    {
        context.Articles.Attach(article);
        var result = context.Articles.Update(article);
        if (await context.SaveChangesAsync() == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotUpdated, "Статья не изменена");

        return result.Entity;
    }
}