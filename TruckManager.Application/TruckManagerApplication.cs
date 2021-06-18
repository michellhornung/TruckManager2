using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TruckManager.Application.Interfaces;
using TruckManager.Domain.ViewModels;
using TruckManager.InfraStructure.Interfaces;

namespace TruckManager.Application
{
    public class TruckManagerApplication : ITruckManagerApplication
    {
        private ITruckManagerContext _dbContext;

        public TruckManagerApplication(ITruckManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TruckViewModel> Get(int id)
        {
            var result = await _dbContext.Trucks.FindAsync(id);
            return result.ToView();
        }

        public async Task<List<TruckViewModel>> List(string search)
        {
            var result = await _dbContext.Trucks.Where(x => x.Name.ToLower().Contains((search ?? "").ToLower())).ToListAsync();
            return result.ToViews();
        }

        public async Task<TruckViewModel> Add(TruckViewModel viewModel)
        {
            var truck = viewModel.ToAdd();
            truck.Check();

            var duplicatedResult = await _dbContext.Trucks.Where(x => x.Name.ToLower() == truck.Name.ToLower()).ToListAsync();
            if (duplicatedResult.Any())
            {
                throw new Exception($"Caminhão {truck.Name} já cadastrado");
            }
            truck.Id = 0;
            truck.CreatedAt = DateTime.Now;
            truck.UpdatedAt = DateTime.Now;

            var result = _dbContext.Trucks.Add(truck);
            await _dbContext.Save();

            return await this.Get(result?.Entity.Id??0);
        }

        public async Task<TruckViewModel> Edit(TruckViewModel viewModel)
        {
            var truck = viewModel.ToEdit();
            truck.Check();

            var truckCurrent = await _dbContext.Trucks.FindAsync(truck.Id);
            if (truckCurrent == null)
            {
                throw new Exception("Caminhão não encontrado");
            }

            var duplicatedResult = await _dbContext.Trucks.Where(x => x.Name.ToLower() == truck.Name.ToLower()).ToListAsync();
            if (duplicatedResult.Any(x => x.Id != truck.Id))
            {
                throw new Exception($"Caminhão {truck.Name} já cadastrado");
            }

            truckCurrent.Name = truck.Name;
            truckCurrent.ModelType = truck.ModelType;
            truckCurrent.ManufactureYear = truck.ManufactureYear;
            truckCurrent.ModelYear = truck.ModelYear;
            truckCurrent.UpdatedAt = DateTime.Now;

            var result = _dbContext.Trucks.Update(truckCurrent);
            await _dbContext.Save();

            return await this.Get(truck.Id);
        }

        public async Task Delete(int id)
        {
            var truck = await _dbContext.Trucks.FindAsync(id);
            if (truck == null)
            {
                throw new Exception("Caminhão não encontrado");
            }

            var result = _dbContext.Trucks.Remove(truck);
            await _dbContext.Save();
        }

    }
}
