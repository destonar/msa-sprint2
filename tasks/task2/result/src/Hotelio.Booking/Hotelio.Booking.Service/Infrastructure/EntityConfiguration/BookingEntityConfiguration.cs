using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotelio.Booking.Service.Infrastructure.EntityConfiguration;

public class BookingEntityConfiguration : IEntityTypeConfiguration<Entities.Booking>
{
    public void Configure(EntityTypeBuilder<Entities.Booking> builder)
    {
        builder.ToTable("bookings");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}