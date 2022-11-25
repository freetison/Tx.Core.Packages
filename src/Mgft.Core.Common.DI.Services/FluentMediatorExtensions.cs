namespace App.Api.DependencyInjection
{
    //using Application.Features.Config.Commands.CreateApplication;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class FluentMediatorExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).Assembly);
            //services.AddTransient<IRequestHandler<CreateApplicationCommand, string>, CreateApplicationCommand.CreateAppCommandHandler>(); // Mediator dependency injection request

            return services;
        }
    }
}
