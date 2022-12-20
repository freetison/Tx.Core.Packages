Exmaple:

1. Implement INamedService<T, K>  service
    
```<language>
public class CashOnDelivery : INamedService<PaymentResult, PaymentRequest>
{
    public string Name { get; private set; } = "Cash";

    public PaymentResult Process(PaymentRequest request)
    {
        Console.WriteLine("Pay with cash on delivery");
        return new PaymentResult("OK");
    }
        
}

public class CreditCardPayment : INamedService<PaymentResult, PaymentRequest>
{
    public string Name { get; private set; } = "CreditCard";

    public PaymentResult Process(PaymentRequest request)
    {
        Console.WriteLine("Pay with credit card");
        return new PaymentResult("OK");
    }
}

```

2. Register services instance in the Container

```<language>
serviceCollection.AddTransient<IServiceResolver<PaymentResult, PaymentRequest>, ServiceResolver<PaymentResult, PaymentRequest>>();
serviceCollection.AddSingleton<IStrategyResolver<PaymentResult, PaymentRequest>, StrategyResolver<PaymentResult, PaymentRequest>>();
Assembly.GetExecutingAssembly().GetTypesAssignableFrom<INamedService<PaymentResult, PaymentRequest>>().ForEach((t) =>
{
    serviceCollection.AddTransient(typeof(INamedService<PaymentResult, PaymentRequest>), t);
});
```

3. Resolve the Instance in your class

```<language>
    public class QueueReport
        {
            private IServiceResolver<PaymentResult, PaymentRequest> _serviceResolver;
        
            public QueueReport(IServiceResolver<PaymentResult, PaymentRequest> serviceResolver)
            {
                _serviceResolver = serviceResolver;
            }

            public async Task Handle(PaymentRequest request, CancellationToken cancellationToken)
            {
                var result =  await _serviceResolver.Execute("CreditCard", paymentRequest);
           
                return Task.CompletedTask;
            }
        }
    }
```
