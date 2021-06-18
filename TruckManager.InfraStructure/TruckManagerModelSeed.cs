using Microsoft.EntityFrameworkCore;
using TruckManager.Domain;

namespace TruckManager.InfraStructure
{
    public static class TruckManagerModelSeed
    {
        public static void RunSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Truck>().HasData(
                new Truck(ModelType.FH, 2021, 2021, "Caminhão 01") { Id = 1 },
                new Truck(ModelType.FM, 2021, 2021, "Caminhão 02") { Id = 2 }
            );
        }
    }
}