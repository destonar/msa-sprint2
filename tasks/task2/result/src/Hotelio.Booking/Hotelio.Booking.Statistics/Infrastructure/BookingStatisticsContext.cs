using Hotelio.Booking.Statistics.Entities;
using Hotelio.Booking.Statistics.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Booking.Statistics.Infrastructure;

public class BookingStatisticsContext : DbContext
{
    public BookingStatisticsContext(DbContextOptions<BookingStatisticsContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookingRecordConfiguration());
    }
    
    public DbSet<BookingRecord> BookingHistory { get; set; }
}