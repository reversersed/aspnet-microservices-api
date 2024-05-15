using EventsApi.src.Data.Entities;
using EventsApi.src.Repositories;
using System.Net.Http;
using EventsApi.src.Repositories.Interfaces;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using ResponsePackage;
using Mapster;
using Extensions.HttpExtension.Interfaces;

namespace EventsApi.src.Services;

public class CommentService(ICommentRepository _commentRepository, IInternalHttpClient _httpClient, ICreatorRepository _creatorRepository) : ICommentService
{
    private readonly ICommentRepository commentRepository = _commentRepository ?? throw new ArgumentNullException(nameof(_commentRepository));
    private readonly IInternalHttpClient httpClient = _httpClient ?? throw new ArgumentNullException(nameof(_httpClient));
    private readonly ICreatorRepository creatorRepository = _creatorRepository ?? throw new ArgumentNullException(nameof(_creatorRepository));
    public async Task<CommentDTO> Add(CommentDTO comment, long eventId)
    {
        var data = comment.Adapt<Comment>();

        var response = await httpClient.SendGet<UserResponse>("identityapi", "byname", ("name", comment.Creator));
        if (response.Code != ResponseCodes.DataFound || response.Data is null)
            throw new CustomExceptionResponse(ResponseCodes.UserNotFound, response.Message ?? string.Empty);
        await creatorRepository.TryAddCreator(response.Data.Id, response.Data.UserName);

        var entity = await commentRepository.Add(data, eventId, response.Data.Id);
        return entity.Adapt<CommentDTO>();
    }

    public async Task Delete(long id) => await commentRepository.Delete(id);

    public async Task<CommentDTO> Get(long id)
    {
        var resposne = await commentRepository.Get(id);
        return resposne.Adapt<CommentDTO>();
    }

    public async Task<IEnumerable<CommentDTO>> GetByEvent(long eventId, int offset = 0, int count = 0)
    {
        var response = await commentRepository.GetByEvent(eventId, offset, count);
        if (!response.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Получена пустая коллекция");
        return response.Adapt<IEnumerable<CommentDTO>>();
    }

    public async Task<CommentDTO> Update(CommentDTO comment)
    {
        var data = await commentRepository.Get(comment.Id);
        data = comment.Adapt(data);

        var entity = await commentRepository.Update(data);
        return entity.Adapt<CommentDTO>();
    }
}
