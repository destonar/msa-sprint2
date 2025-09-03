using Hotelio.Booking.Service.Infrastructure;
using Hotelio.Booking.Service.Infrastructure.EventProducer;
using Hotelio.Booking.Service.Integrations;
using Hotelio.Booking.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddServiceDefaults()
    ;

builder.Services.AddDbContext<BookingContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("booking_db")));
builder.EnrichNpgsqlDbContext<BookingContext>();
builder.WebHost.ConfigureKestrel(options =>
{
    var grpcPort = builder.Environment.IsDevelopment() ? 5019 : 8080;
    options.ListenAnyIP(grpcPort, o => o.Protocols = HttpProtocols.Http2);

    options.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);
});
builder.AddIntegrations();
builder.AddBookingEventProducer();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGet("/ping", ([FromServices]EventProducerOptions options) => options.IsEnabled ? "pong-v2" : "pong");
app.MapGrpcService<BookingService>();

app.Run();