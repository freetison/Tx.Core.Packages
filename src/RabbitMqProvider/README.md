Basic implementation

Exmaple:

1. Define connection in the DI container
```<language>
    public static async Task<IServiceCollection> AddRabbitMqProviderAsync(this IServiceCollection services, RabbitMqSettings rabbitMqSettings)
        {
            services.AddSingleton(rabbitMqSettings);
            services.AddSingleton<IConnectionFactory>(_ => CreateConnectionFactory(rabbitMqSettings));

            var connectionFactory = CreateConnectionFactory(rabbitMqSettings);
            var connection = await connectionFactory.CreateConnectionAsync();
            services.AddSingleton(connection);

            //services.AddSingleton<ModelFactory>();
            services.AddSingleton<IRabbitMqClientProvider, RabbitMqClientProvider>();

            return services;
        }

        private static ConnectionFactory CreateConnectionFactory(RabbitMqSettings settings)
        {
            return new ConnectionFactory
            {
                UserName = settings.RabbitMqUserName,
                Password = settings.RabbitMqPassword,
                HostName = settings.RabbitMqHostName,
                Port = settings.RabbitMqPort,
                AutomaticRecoveryEnabled = true,
                ConsumerDispatchConcurrency = settings.RabbitMqConcurrency
            };
        }

```


2.  Send a message to que Queue
```<language>
     public class MessageSender
{
    private readonly IRabbitMqClientProvider _rabbitMqClient;

    public MessageSender(IRabbitMqClientProvider rabbitMqClient)
    {
        _rabbitMqClient = rabbitMqClient;
    }

    public async Task SendMessageAsync()
    {
        var message = new { Id = Guid.NewGuid(), Text = "Hello, RabbitMQ!" };
        await _rabbitMqClient.PublishAsync("my-exchange", "my-routing-key", message);
    }
}
```

3. Receive messages from Queue
```
public class MessageReceiver : BackgroundService
{
    private readonly IRabbitMqClientProvider _rabbitMqClient;
    private readonly ILogger<MessageReceiver> _logger;

    public MessageReceiver(IRabbitMqClientProvider rabbitMqClient, ILogger<MessageReceiver> logger)
    {
        _rabbitMqClient = rabbitMqClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await _rabbitMqClient.ConsumeAsync<MessageDto>("my-queue");

            if (message != null)
            {
                _logger.LogInformation("Received message: {Id} - {Text}", message.Id, message.Text);
            }
        }
    }
}

public record MessageDto(Guid Id, string Text);
```