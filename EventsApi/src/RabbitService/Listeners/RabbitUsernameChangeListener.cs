using System.Text;
using System.Text.Json;
using EventsApi.src.Repositories.Interfaces;
using Extensions.RabbitExtension;
using Extensions.RabbitExtension.EventModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventsApi.src.RabbitService.Listeners;

public class RabbitUsernameChangeListener : BackgroundService
{
    private readonly ILogger<RabbitUsernameChangeListener> logger;
    private readonly IServiceProvider serviceScopeFactory;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string queueName;
    public RabbitUsernameChangeListener(IServiceProvider serviceScopeFactory, ILogger<RabbitUsernameChangeListener> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        logger.LogInformation($"[EventsRabbit] Connecting to {RabbitNames.RabbitHostname}...");
        var factory = new ConnectionFactory() { Uri = new Uri(RabbitNames.RabbitHostname) };
        this.connection = factory.CreateConnection();
        this.channel = connection.CreateModel();
        this.queueName = channel.QueueDeclare().QueueName;
        this.channel.ExchangeDeclare(RabbitNames.OnUsernameUpdatedExchange, ExchangeType.Fanout);
        this.channel.QueueBind(queueName, RabbitNames.OnUsernameUpdatedExchange, string.Empty, null);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += OnUsernameChanged;

        channel.BasicConsume(queueName, false, consumer);
        return Task.CompletedTask;
    }

    private async void OnUsernameChanged(object? sender, BasicDeliverEventArgs e)
    {
#pragma warning disable CA2208
        var changeData = JsonSerializer.Deserialize<UsernameChangeModel>(Encoding.UTF8.GetString(e.Body.ToArray())) ?? throw new ArgumentNullException("[RabbitMq] RabbitMQ слушатель получил пустой объект");
#pragma warning restore CA2208
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var creatorRepository = scope.ServiceProvider.GetRequiredService<ICreatorRepository>();

            await creatorRepository.ChangeName(changeData.OldName, changeData.NewName);
        }

        logger.LogInformation("[EventsRabbit] Username {oldname} has been changed to {newname}", changeData.OldName, changeData.NewName);
    }
    public override void Dispose()
    {
        channel.Close();
        connection.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}