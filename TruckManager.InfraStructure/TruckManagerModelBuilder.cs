using Microsoft.EntityFrameworkCore;
using TruckManager.Domain;

namespace TruckManager.InfraStructure
{
    public static class TruckManagerModelBuilder
    {
        public static void RunBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Truck>(etd =>
               {
                   etd.ToTable("tbTruck");
                   etd.HasKey(c => c.Id);
                   etd.Property(c => c.Id);
                   etd.Property(c => c.Name).HasMaxLength(100);
               });
        }
    }
}
