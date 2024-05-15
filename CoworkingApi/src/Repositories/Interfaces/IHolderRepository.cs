namespace CoworkingApi.src.Repositories.Interfaces;

public interface IHolderRepository
{
    Task TryAddHolder(long creatorId, string creatorName);
    Task ChangeName(string oldName, string newName);
}
