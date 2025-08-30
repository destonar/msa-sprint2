using Hotelio.Booking.Statistics.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookingStatisticsContext>(o => 
    o.UseNpgsql(builder.Configuration.GetConnectionString("booking_statistics_db"),
        x => x.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

builder.Build();