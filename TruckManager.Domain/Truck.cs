using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckManager.Domain
{
    public class Truck
    {
        public Truck() { }
        public Truck(ModelType modelType, int manufactureYear, int modelYear, string name)
        {
            this.ModelType = modelType;
            this.Name = name;
            this.ManufactureYear = manufactureYear;
            this.ModelYear = modelYear;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
        public Truck(int id, ModelType modelType, int manufactureYear, int modelYear, string name)
        {
            this.Id = id;
            this.ModelType = modelType;
            this.Name = name;
            this.ManufactureYear = manufactureYear;
            this.ModelYear = modelYear;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ModelType ModelType { get; set; }
        public int ManufactureYear { get; set; }
        public int ModelYear { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Check()
        {
            var currentYear = DateTime.Today.Year;

            if (this.Id == 0 && this.ManufactureYear != currentYear)
            {
                throw new Exception("Ano de fabricação não pode ser diferente do ano atual");
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                throw new Exception("Nome inválido");
            }
            if (this.ModelType == ModelType.None)
            {
                throw new Exception("Modelo inválido");
            }
            if (this.ModelYear < this.ManufactureYear)
            {
                throw new Exception("Ano do modelo não pode ser inferior ao ano de fabricação");
            }
            if (this.ModelYear > this.ManufactureYear + 1)
            {
                throw new Exception("Ano do modelo não pode ser superior ao ano subsequente ao de fabricação");
            }
        }
    }
}
