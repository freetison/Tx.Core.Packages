Exmaple:

1. Implement ICustomServiceWrapper<> for the multi-instance service
    
```<language>
    public class CustomTopicClient : ICustomServiceWrapper<ITopicClient>
    {
        public string Name { get; }
        public ITopicClient CustomService { get; }

        public CustomTopicClient(string name, ITopicClient customService)
        {
            Name = name;
            CustomService = customService;
        }
    }
```

2. Register multiple instance in the Container

```<language>
    services.AddSingleton<ICustomServiceWrapper<ITopicClient>>(opt => {
        var client = new TopicClient("Some ConnStr", "Topic-1");
        var customService = new CustomTopicClient("Instance-1", client);
        return customService;
    });

    services.AddSingleton<ICustomServiceWrapper<ITopicClient>>(opt => {
        var client = new TopicClient("Some ConnStr", "Topic-2");
        var customService = new CustomTopicClient("Instance-2", client);
        return customService;
    });
```

3. Resolve the Instance in your class

```<language>
    public class QueueReport
        {
            private readonly CustomServiceResolver<ITopicClient, ICustomServiceWrapper<ITopicClient>> _serviceResolver;
            private readonly ITopicClient _client
        
            public QueueReport(CustomServiceResolver<ITopicClient, ICustomServiceWrapper<ITopicClient>> serviceResolver)
            {
                _serviceResolver = serviceResolver;
                _client = _serviceResolver.GetServiceByName("Instance-1");
            }

            public async Task Handle(AccessVerifiedEvent notification, CancellationToken cancellationToken)
            {
                await _client.SendAsync(Message);
           
            }
        }
    }
```
