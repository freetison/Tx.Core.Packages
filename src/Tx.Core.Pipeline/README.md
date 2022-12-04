Thanks to JEREMY DAVIS for this articule: [https://jermdavis.wordpress.com/2016/10/03/an-alternative-approach-to-pipelines/](http://example.com)

Exmaple:

1. Define some steps
```<language>
    public class DoNothingStep : IAsyncPipelineStep<int, int>
    {
        public string StepName => $"{GetType().Name}";
        public Task<int> ProcessAsync(int input) => Task.FromResult(input);
    }

    public class Step1: IAsyncPipelineStep<int, int>
    {
        public string StepName => "Step one";
        public Task<int> ProcessAsync(int input)
        {
            Console.WriteLine($"Processing {StepName}");
            ++input;
            return Task.FromResult(input);
        }
    }

    public class Step2: IAsyncPipelineStep<int, int>
    {
        public string StepName => "Step two";
        public Task<int> ProcessAsync(int input)
        {
            Console.WriteLine($"Processing {StepName}");
            --input;
            return Task.FromResult(input);
        }
    }
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
