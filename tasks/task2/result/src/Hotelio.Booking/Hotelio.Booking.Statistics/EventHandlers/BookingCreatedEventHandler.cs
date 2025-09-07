using Hotelio.Booking.IntegrationEvents;
using Hotelio.Booking.Statistics.Entities;
using Hotelio.Booking.Statistics.Infrastructure;

namespace Hotelio.Booking.Statistics.EventHandlers;

public class BookingCreatedEventHandler : IBookingCreatedEventHandler
{
    private readonly BookingStatisticsContext _dbContext;
    private readonly ILogger<BookingCreatedEventHandler> _logger;

    public BookingCreatedEventHandler(BookingStatisticsContext dbContext, ILogger<BookingCreatedEventHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task HandleAsync(BookingCreated @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling event {EventType} with id: {EventId} body: {EventBody}", nameof(BookingCreated), @event.EventId, @event);
        var record = new BookingRecord(@event.BookingId,
            @event.UserId,
            @event.HotelId,
            @event.PromoCode,
            @event.DiscountPercent,
            @event.Price,
            @event.OccuredOn);
        
        _dbContext.BookingHistory.Add(record);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Event {EventType} with id: {EventId} handled successfully",  nameof(BookingCreated), @event.EventId);
    }
}