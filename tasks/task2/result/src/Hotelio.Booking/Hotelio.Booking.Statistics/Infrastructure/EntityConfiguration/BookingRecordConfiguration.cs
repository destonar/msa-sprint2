using Hotelio.Booking.Statistics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotelio.Booking.Statistics.Infrastructure.EntityConfiguration;

public class BookingRecordConfiguration : IEntityTypeConfiguration<BookingRecord>
{
    public void Configure(EntityTypeBuilder<BookingRecord> builder)
    {
        builder.ToTable("booking_history");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}