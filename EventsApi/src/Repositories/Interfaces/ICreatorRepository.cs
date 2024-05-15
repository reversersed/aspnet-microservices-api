namespace EventsApi.src.Repositories.Interfaces;

public interface ICreatorRepository
{
    Task TryAddCreator(long creatorId, string creatorName);
    Task ChangeName(string oldName, string newName);
}
