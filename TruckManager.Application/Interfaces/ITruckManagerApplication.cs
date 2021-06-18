using System.Collections.Generic;
using System.Threading.Tasks;
using TruckManager.Domain;
using TruckManager.Domain.ViewModels;

namespace TruckManager.Application.Interfaces
{
    public interface ITruckManagerApplication
    {
        Task<TruckViewModel> Get(int id);
        Task<List<TruckViewModel>> List(string search);
        Task<TruckViewModel> Add(TruckViewModel truck);
        Task<TruckViewModel> Edit(TruckViewModel truck);
        Task Delete(int id);
    }
}
