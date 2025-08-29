using Hotelio.Booking.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookingContext>(o => 
    o.UseNpgsql(builder.Configuration.GetConnectionString("booking_db"),
        x => x.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

builder.Build();