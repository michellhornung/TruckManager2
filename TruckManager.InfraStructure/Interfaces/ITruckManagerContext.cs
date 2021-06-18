using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TruckManager.Domain;

namespace TruckManager.InfraStructure.Interfaces
{
    public interface ITruckManagerContext: IDbContext
    {
        DbSet<Truck> Trucks { get; set; }
        Task Save();
    }
}
