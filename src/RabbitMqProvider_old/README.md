How to Use

Exmaple:

1. Define in the Di container
```<language>
     var setting = new RabbitMqConfigurationSettings
            {
                ... settings here
            };

            services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(setting));
            services.AddSingleton<IMessageProducer, RabbitMqProducer>();
```


2. Define one or more Pipelines, you can exchange the steps
```<language>
    public class DemoPipeline : AsyncPipeline<int, int>
    {
        public DemoPipeline(
            Step1 step1,
            Step2 step2,
            DoNothingStep doNothingStep)
        {

            PipelineSteps = input => input
                .Step(step1)
                .Step(Step2)
        }
    }

    public class ConditionalDemoPipeline : AsyncPipeline<int, int>
    {
        public ConditionalDemoPipeline(
            Step1 step1,
            Step2 step2,
            DoNothingStep doNothingStep)
        {

            PipelineSteps = input => input
                .Step(step1)
                .When(() => input > 0, step2, doNothingStep)
        }
    }

```

3. Register multiple instance in the Container
```<language>
    // Register Pipelines Steps
    services.AddSingleton(sp => ActivatorUtilities.CreateInstance<DoNothingStep>(sp));
    services.AddSingleton(sp => ActivatorUtilities.CreateInstance<Step1>(sp));
    services.AddSingleton(sp => ActivatorUtilities.CreateInstance<Step2>(sp));

    // Register Pipelines
    services.AddSingleton(sp => ActivatorUtilities.CreateInstance<ConditionalDemoPipeline>(sp));
    services.AddSingleton(sp => ActivatorUtilities.CreateInstance<DemoPipeline>(sp));

```

3. Use the Pipeline in your class
```<language>
    public class PipelineProcessor
        {
            private readonly ConditionalDemoPipeline _pipeline;
            
            public PipelineProcessor(ConditionalDemoPipeline pipeline)
            {
                _pipeline = pipeline;
            }

            public async Task Handle(CancellationToken cancellationToken)
            {
                var result = await _pipeline.ProcessAsync(5);
            }
        }
    }
```
