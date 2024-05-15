using Extensions.HttpExtension.Interfaces;
using Mapster;
using NewsApi.src.Repositories;
using NewsApi.src.Repositories.Interfaces;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Services;

public class CommentService(ICommentRepository _commentRepository, IInternalHttpClient _httpClient) : ICommentService
{
    private readonly ICommentRepository commentRepository = _commentRepository ?? throw new ArgumentNullException(nameof(_commentRepository));
    private readonly IInternalHttpClient httpClient = _httpClient ?? throw new ArgumentNullException(nameof(_httpClient));

    public async Task<CommentDTO> Add(long article_id, CommentDTO comment)
    {
        var data = comment.Adapt<Data.Entities.Comment>();

        var response = await httpClient.SendGet<UserResponse>("identityapi", "byname", ("name", comment.Author));
        if (response.Code == ResponseCodes.DataFound && response.Data is not null)
            data.AuthorId = response.Data.Id;
        else
            throw new CustomExceptionResponse(ResponseCodes.UserNotFound, response.Message ?? string.Empty);

        var result = await commentRepository.Add(article_id, data);
        return result.Adapt<CommentDTO>();
    }
    public async Task ChangeUsername(string oldname, string newname) => await commentRepository.ChangeUsername(oldname, newname);
    public async Task Delete(long id) => await commentRepository.Delete(id);

    public async Task<CommentDTO> Get(long id)
    {
        var data = await commentRepository.Get(id) ?? throw new ArgumentException("Указанного комментария не существует");
        return data.Adapt<CommentDTO>();
    }

    public async Task<IEnumerable<CommentDTO>> Get(long article_id, int offset = 0, int count = 10)
    {
        var data = await commentRepository.Get(article_id, offset, count);
        if (!data.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Данных не найдено");
        return data.Adapt<IEnumerable<CommentDTO>>();
    }

    public async Task<CommentDTO> Patch(long id, string? Content)
    {
        var comment = await commentRepository.Get(id);

        comment.Content = Content ?? comment.Content;

        var updated_data = await commentRepository.Patch(comment);
        return updated_data.Adapt<CommentDTO>();
    }
}