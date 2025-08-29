using Hotelio.Booking.Service.Infrastructure;
using Hotelio.Booking.Service.Integrations;
using Hotelio.Booking.Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddServiceDefaults()
    .AddDefaultOpenApi()
    ;

builder.Services.AddGrpc();

builder.Services.AddDbContext<BookingContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("booking_db")));
builder.EnrichNpgsqlDbContext<BookingContext>();

builder.AddIntegrations();

var app = builder.Build();

app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseDefaultOpenApi();
}

app.MapGrpcService<BookingService>();

app.Run();