using System.Collections.Generic;
using System.Linq;

namespace TruckManager.Domain.ViewModels
{
    public class TruckViewModel
    {
        public int Id { get; set; }
        public ModelType ModelType { get; set; }
        public int ManufactureYear { get; set; }
        public int ModelYear { get; set; }
        public string Name { get; set; }
    }

    public static class TruckViewModelExtensions
    {
        public static Truck ToAdd(this TruckViewModel viewModel)
        {
            return new Truck(viewModel.ModelType, viewModel.ManufactureYear, viewModel.ModelYear, viewModel.Name);
        }
        public static Truck ToEdit(this TruckViewModel viewModel)
        {
            return new Truck(viewModel.Id, viewModel.ModelType, viewModel.ManufactureYear, viewModel.ModelYear, viewModel.Name);
        }

        public static List<TruckViewModel> ToViews(this List<Truck> items)
        {
            return items.Select(ToView).ToList();
        }
        public static TruckViewModel ToView(this Truck item)
        {
            return new TruckViewModel()
            {
                Id = item.Id,
                ModelType = item.ModelType,
                ManufactureYear = item.ManufactureYear,
                ModelYear = item.ModelYear,
                Name = item.Name,
            };
        }
    }
}
