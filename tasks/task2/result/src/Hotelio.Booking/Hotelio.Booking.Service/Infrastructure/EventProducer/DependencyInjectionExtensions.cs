using Confluent.Kafka;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public static class DependencyInjectionExtensions
{
    public static WebApplicationBuilder AddBookingEventProducer(this WebApplicationBuilder builder)
    {
        builder.AddKafkaProducer<string, string>("kafka", settings =>
        {
            settings.Config.Acks = Acks.All;
        });
        
        builder.Services.AddHostedService<TopicInitializer>();
        builder.Services.AddScoped<IBookingCreatedEventProducer, BookingCreatedEventProducer>();
        return builder;
    }
}