using NewsApi.src.Repositories.Interfaces;
using NewsApi.src.Services.Interfaces;
using NewsApi.src.Services.Entities;
using Mapster;
using Extensions.HttpExtension.Interfaces;
using ResponsePackage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewsApi.src.Services;

public class ArticleService(IArticleRepository _articleRepository, IInternalHttpClient _httpClient) : IArticleService
{
    private readonly IArticleRepository articleRepository = _articleRepository ?? throw new ArgumentNullException(nameof(_articleRepository));
    private readonly IInternalHttpClient httpClient = _httpClient ?? throw new ArgumentNullException(nameof(_httpClient));

    public async Task<ArticleDTO> Add(ArticleDTO article)
    {
        var data = article.Adapt<Data.Entities.Article>();

        var response = await httpClient.SendGet<UserResponse>("identityapi", "byname", ("name", article.Author));
        if (response.Code == ResponseCodes.DataFound && response.Data is not null)
            data.AuthorId = response.Data.Id;
        else
            throw new CustomExceptionResponse(ResponseCodes.UserNotFound, response.Message ?? string.Empty);

        var result = await articleRepository.Add(data);
        return result.Adapt<ArticleDTO>();
    }

    public async Task ChangeUsername(string oldname, string newname) => await articleRepository.ChangeUsername(oldname, newname);

    public async Task Delete(long id) => await articleRepository.Delete(id);

    public async Task<ArticleDTO> Get(long id)
    {
        var data = await articleRepository.Get(id) ?? throw new ArgumentException("Указанной статьи не существует");
        return data.Adapt<ArticleDTO>();
    }
    public async Task<IEnumerable<ArticleDTO>> Get(int offset = 0, int count = 10)
    {
        var data = await articleRepository.Get(offset, count);
        if (!data.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Данных не найдено");
        return data.Adapt<IEnumerable<ArticleDTO>>();
    }

    public async Task<ArticleDTO> Patch(long id, string? Title, string? Content)
    {
        var article = await articleRepository.Get(id);

        article.Title = Title ?? article.Title;
        article.Content = Content ?? article.Content;

        var updated_data = await articleRepository.Patch(article);
        return updated_data.Adapt<ArticleDTO>();
    }
}