using Booking;
using Grpc.Core;
using Hotelio.Booking.Service.Entities;
using Hotelio.Booking.Service.Infrastructure;
using Hotelio.Booking.Service.Integrations.Hotels;
using Hotelio.Booking.Service.Integrations.PromoCodes;
using Hotelio.Booking.Service.Integrations.Reviews;
using Hotelio.Booking.Service.Integrations.Users;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Booking.Service.Services;

public class BookingService : global::Booking.BookingService.BookingServiceBase
{
    private readonly ILogger<BookingService> _logger;
    private readonly BookingContext _dbContext;
    private readonly IUsersClient _usersClient;
    private readonly IHotelsClient _hotelsClient;
    private readonly IReviewsClient _reviewsClient;
    private readonly IPromoCodesClient _promoCodesClient;

    public BookingService(
        ILogger<BookingService> logger,
        BookingContext dbContext,
        IUsersClient usersClient,
        IHotelsClient hotelsClient,
        IReviewsClient reviewsClient,
        IPromoCodesClient promoCodesClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _usersClient = usersClient;
        _hotelsClient = hotelsClient;
        _reviewsClient = reviewsClient;
        _promoCodesClient = promoCodesClient;
    }
    
    public override async Task<BookingListResponse> ListBookings(BookingListRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Bookings requested for UserId: {UserId}",  request.UserId);
        List<Entities.Booking> bookings;
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            bookings = await _dbContext.Bookings
                .ToListAsync(context.CancellationToken);
        }
        else
        {
            bookings = await _dbContext.Bookings
                .Where(b => b.UserId == request.UserId)
                .ToListAsync(context.CancellationToken);
        }
        
        return new BookingListResponse
        {
            Bookings = { bookings.Select(b => b.ToResponse()) }
        };
    }
    
    public override async Task<BookingResponse> CreateBooking(BookingRequest request, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        await ValidateUserAsync(request.UserId, cancellationToken);
        await ValidateHotelAsync(request.HotelId, cancellationToken);
        
        var basePrice = await ResolveBasePriceAsync(request.UserId, cancellationToken);
        var discount = await ResolvePromoDiscountAsync(request.PromoCode, request.UserId,  cancellationToken);
        
        var finalPrice = basePrice - discount;
        _logger.LogInformation("Final price calculated: base={BasePrice}, discount={Discount}, final={FinalPrice}", basePrice, discount, finalPrice);
        
        var booking = new Entities.Booking(request.UserId, request.HotelId, request.PromoCode, discount, finalPrice, DateTime.UtcNow);
        _dbContext.Bookings.Add(booking);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        
        _logger.LogInformation("Booking for hotel id={HotelId} is created for user id={UserId}", request.HotelId, request.UserId);
        return booking.ToResponse();
    }

    private async Task ValidateUserAsync(string userId, CancellationToken cancellationToken)
    {
        var isActive = await _usersClient.IsUserActiveAsync(userId, cancellationToken);
        if (!isActive)
        {
            throw new Exception($"User {userId} is not active");
        }
        
        var isBlacklisted = await _usersClient.IsUserBlacklistedAsync(userId, cancellationToken);
        if (isBlacklisted)
        {
            throw new Exception($"User {userId} is blacklisted");
        }
    }

    private async Task ValidateHotelAsync(string hotelId, CancellationToken cancellationToken)
    {
        var isOperational = await _hotelsClient.IsOperationalAsync(hotelId, cancellationToken);
        if (!isOperational)
        {
            throw new Exception($"Hotel {hotelId} is not operational");
        }
        
        var isTrusted = await _reviewsClient.IsHotelTrustedAsync(hotelId, cancellationToken);
        if (!isTrusted)
        {
            throw new Exception($"Hotel {hotelId} is not trusted");
        }
        
        var isFullyBooked = await _hotelsClient.IsFullyBookedAsync(hotelId, cancellationToken);
        if (isFullyBooked)
        {
            throw new Exception($"Hotel {hotelId} is fully booked");
        }
    }

    private async Task<double> ResolveBasePriceAsync(string userId, CancellationToken cancellationToken)
    {
        var status = await _usersClient.GetUserStatusAsync(userId, cancellationToken);
        return status == "VIP" ? 80.0 : 100.0;
    }

    private async Task<double> ResolvePromoDiscountAsync(string code, string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogInformation("No promo code was supplied");
            return 0.0;
        }
        
        var promo = await _promoCodesClient.ValidateCode(code, userId, cancellationToken);
        if (promo == null)
        {
            _logger.LogInformation("Promo code '{PromoCode}' is invalid or not applicable for user {UserId}", code, userId);
            return 0.0;
        }
        
        return promo.Discount;
    }
}