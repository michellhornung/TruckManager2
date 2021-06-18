using Microsoft.EntityFrameworkCore;
using System;

namespace TruckManager.InfraStructure.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbContext Context { get; }
    }
}
