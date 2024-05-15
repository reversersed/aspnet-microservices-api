using Microsoft.EntityFrameworkCore;
using NewsApi.src.Data;
using NewsApi.src.Data.Entities;
using NewsApi.src.Repositories.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Repositories;

public class CommentRepository(DataContext _context) : ICommentRepository
{
    private readonly DataContext context = _context ?? throw new ArgumentNullException(nameof(_context));

    public async Task<Comment> Add(long article_id, Comment comment)
    {
        var article = await context.Articles.FindAsync(article_id) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанного комментария не существует");
        comment.Article = article;
        var result = context.Comments.Add(comment) ?? throw new ArgumentException("Ошибка во время добавления комментария");
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task ChangeUsername(string oldname, string newname)
    {
        context.Comments.Where(x => x.Author.Equals(oldname)).ExecuteUpdate(x => x.SetProperty(x => x.Author, newname));
        await context.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
        var response = await context.Comments.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (response == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанного комментария не существует");
        await context.SaveChangesAsync();
    }

    public async Task<Comment> Get(long id)
    {
        var article = await context.Comments.FindAsync(id) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанного комментария не существует");
        return article;
    }

    public async Task<IEnumerable<Comment>> Get(long article_id, int offset = 0, int count = 10)
    {
        return await context.Comments.Include(i => i.Article).Where(i => i.Article.Id == article_id).OrderByDescending(i => i.Created).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<Comment> Patch(Comment comment)
    {
        context.Comments.Attach(comment);
        var result = context.Comments.Update(comment);
        if (await context.SaveChangesAsync() == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotUpdated, "Комментарий не изменен");

        return result.Entity;
    }
}