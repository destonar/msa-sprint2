using Confluent.Kafka;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public static class DependencyInjectionExtensions
{
    public static WebApplicationBuilder AddBookingEventProducer(this WebApplicationBuilder builder)
    {
        var opts = new EventProducerOptions
        {
            IsEnabled = IsEnabled()
        };
        builder.Services.AddSingleton(opts);
        builder.Services.AddHostedService<TopicInitializer>();
        
        if (!opts.IsEnabled)
        {
            builder.Services.AddSingleton<IBookingCreatedEventProducer, NoopEventProducer>();
            return builder;
        }
        
        builder.AddKafkaProducer<string, string>("kafka", settings =>
        {
            settings.Config.Acks = Acks.All;
        });
        builder.Services.AddScoped<IBookingCreatedEventProducer, BookingCreatedEventProducer>();
        return builder;
    }

    private static bool IsEnabled()
    {
        var env = Environment.GetEnvironmentVariable("ENABLE_FEATURE_KAFKA_PUBLISH");
        if (env == null)
        {
            return false;
        }

        if (!bool.TryParse(env, out var isEnabled))
        {
            return false;
        }

        return isEnabled;
    }
}