
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TruckManager.Domain;
using TruckManager.InfraStructure.Interfaces;

namespace TruckManager.InfraStructure
{

    public class TruckManagerContext : DbContext, ITruckManagerContext
    {
        public DbContext Context => this;

        public TruckManagerContext()
        {
        }

        public DbSet<Truck> Trucks { get; set; }

        public async Task Save() {
            await this.Context.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=TruckManager;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RunBuilder();
            modelBuilder.RunSeed();
        }
    }
}