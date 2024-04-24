using MassTransit;

namespace grenius_rating.Infrastructure
{
    public static class RabbitConfiguration
    {
        public static IServiceCollection AddRabbitMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(mass =>
            {
                mass.SetKebabCaseEndpointNameFormatter();

                var assembly = typeof(Program).Assembly;
                mass.AddConsumers(assembly);

                var rabbitMqConfig = configuration.GetSection("MessageBroker");
                mass.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConfig["Host"], h =>
                    {
                        h.Username(rabbitMqConfig["Username"]);
                        h.Password(rabbitMqConfig["Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
