Thanks to JEREMY DAVIS for this articule: [https://jermdavis.wordpress.com/2016/10/03/an-alternative-approach-to-pipelines/](http://example.com)

Exmaple:

1. Define some concrete class implementing the IGenericStrategyProcessor<>
```<language>
    public class ConcreteClassOne : IGenericStrategyProcessor<string, int>
    {
        public string Name { get; private set; } = "Some unique name";
        public string Process(int request)
        {
            Console.WriteLine($"Processing {GetType().Name}");
            return "OK";
        }
    
    }


    public class ConcreteClassTwo : IGenericStrategyProcessor<string, int>
    {
        public string Name { get; private set; } = "Some unique name";
        public string Process(int request)
        {
            Console.WriteLine($"Processing {GetType().Name}");
            return "OK";
        }
    }

```


2. Register multiple instance in the Container
```<language>
     serviceCollection.AddTransient<IGenericStrategyService<string, int>, GenericStrategyService<string, int>>();
     serviceCollection.AddSingleton<IGenericStrategyResolver<string, int>, GenericStrategyResolver<string, int>>();
     Assembly.GetExecutingAssembly().GetTypesAssignableFrom<IGenericStrategyProcessor<string, int>>().ForEach((t) =>
     {
        serviceCollection.AddTransient(typeof(IGenericStrategyProcessor<string, int>), t);
     });
```
**GetTypesAssignableFrom is from the Tx.Core.Extentions nuget package*

3. Use the IGenericStrategyService in your class
```<language>
    public class App
        {
            private IGenericStrategyService<string, int> _genericStrategyService;
            
            public App(IGenericStrategyService<string, int> genericStrategyService)
            {
                 _genericStrategyService = genericStrategyService;
            }

            public Task Handle(DataModel request,  CancellationToken cancellationToken)
            {
                var result = _genericStrategyService.Execute("Some unique name", request);
                return Task.CompletedTask;
            }
        }
    }
```
**DataModel is some model you base on*
