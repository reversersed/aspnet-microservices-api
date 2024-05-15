using Extensions.RabbitExtension;
using Extensions.RabbitExtension.EventModels;
using IdentityApi.src.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace IdentityApi.src.RabbitMq;

public class RabbitService : IRabbitService
{
    private readonly ConnectionFactory connectionFactory;
    private readonly ILogger<RabbitService> logger;
    public RabbitService(ILogger<RabbitService> logger)
    {
        connectionFactory = new ConnectionFactory() { Uri = new Uri(RabbitNames.RabbitHostname) };
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SendUsernameChangedEvent(string oldname, string newname)
    {
        var message = JsonSerializer.Serialize(new UsernameChangeModel { OldName = oldname, NewName = newname });

        using var connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(RabbitNames.OnUsernameUpdatedExchange, ExchangeType.Fanout);
        channel.BasicPublish(
            RabbitNames.OnUsernameUpdatedExchange, 
            string.Empty, 
            null, 
            Encoding.UTF8.GetBytes(message)
        );
        logger.LogInformation("[IdentityRabbit] Sended message of username change from {oldname} to {newname}", oldname, newname);
    }
}
