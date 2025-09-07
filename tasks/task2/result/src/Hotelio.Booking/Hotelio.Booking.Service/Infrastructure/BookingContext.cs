using Hotelio.Booking.Service.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Booking.Service.Infrastructure;

public class BookingContext : DbContext
{
    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookingEntityConfiguration());
    }
    
    public DbSet<Entities.Booking> Bookings { get; set; }
}