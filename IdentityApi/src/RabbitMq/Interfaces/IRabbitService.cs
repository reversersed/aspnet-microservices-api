namespace IdentityApi.src.RabbitMq.Interfaces;

public interface IRabbitService
{
    void SendUsernameChangedEvent(string oldname, string newname);
}
