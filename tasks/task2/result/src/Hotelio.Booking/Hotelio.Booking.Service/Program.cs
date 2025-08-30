using Hotelio.Booking.Service.Infrastructure;
using Hotelio.Booking.Service.Infrastructure.EventProducer;
using Hotelio.Booking.Service.Integrations;
using Hotelio.Booking.Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddServiceDefaults()
    ;

builder.Services.AddDbContext<BookingContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("booking_db")));
builder.EnrichNpgsqlDbContext<BookingContext>();

builder.AddIntegrations();
builder.AddBookingEventProducer();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGrpcService<BookingService>();

app.Run();