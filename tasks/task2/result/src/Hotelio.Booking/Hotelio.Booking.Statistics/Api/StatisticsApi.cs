using Hotelio.Booking.Statistics.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Booking.Statistics.Api;

public static class StatisticsApi
{
    public static IEndpointRouteBuilder MapStatisticsApi(this IEndpointRouteBuilder app)
    {
        var api = app.NewVersionedApi("Booking Statistics");
        var v1 = api.MapGroup("api/bookings").HasApiVersion(1.0);
        
        v1.MapGet("/", GetAllAsync);
        v1.MapGet("/{userId}", GetForUserAsync);
        
        return app;
    }

    public static async Task<List<BookingRecordDto>> GetAllAsync([FromServices]BookingStatisticsContext dbContext, CancellationToken cancellationToken = default)
    {
        return await dbContext.BookingHistory
            .Select(b => BookingRecordDto.FromEntity(b))
            .ToListAsync(cancellationToken);
    }
    
    public static async Task<List<BookingRecordDto>> GetForUserAsync([FromServices] BookingStatisticsContext dbContext, [FromRoute] string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.BookingHistory
            .Where(b => b.UserId == userId)
            .Select(b => BookingRecordDto.FromEntity(b))
            .ToListAsync(cancellationToken);
    }
}