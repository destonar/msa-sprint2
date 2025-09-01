using Hotelio.Booking.IntegrationEvents;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public class NoopEventProducer : IBookingCreatedEventProducer
{
    private readonly ILogger<NoopEventProducer> _logger;

    public NoopEventProducer(ILogger<NoopEventProducer> logger)
    {
        _logger = logger;
    }
    
    public Task Publish(BookingCreated @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Can't send an event: event publishing is disabled via feature flag");
        return Task.CompletedTask;
    }
}