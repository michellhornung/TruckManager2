using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TruckManager.Application;
using TruckManager.Application.Interfaces;
using TruckManager.Domain;
using TruckManager.Domain.ViewModels;
using TruckManager.InfraStructure.Interfaces;

namespace TruckManager.Test
{
    [TestClass]
    public class TruckManagerApplicationTest
    {
        private Mock<ITruckManagerContext> _dbContextMock;

        private ITruckManagerApplication _app;

        public TruckManagerApplicationTest()
        {
            _dbContextMock = new Mock<ITruckManagerContext>();

            _app = new TruckManagerApplication(_dbContextMock.Object);
        }

        [TestMethod]
        public async Task ShouldGetTruckAsync()
        {
            var truck = new Truck();
            _dbContextMock.Setup(x => x.Trucks.FindAsync(It.IsAny<int>())).ReturnsAsync(truck);

            var result = await _app.Get(It.IsAny<int>());

            result.Should().Not.Be.Null();
        }

        [TestMethod]
        public async Task ShouldListTruckAsync()
        {
            var truck = new Truck(ModelType.FH, DateTime.Today.Year, DateTime.Today.Year, "Teste");
            var trucks = new List<Truck>() {
                truck
            };

            var mock = trucks.AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(c => c.Trucks).Returns(mock.Object);

            var result = await _app.List(It.IsAny<string>());

            result.Should().Not.Be.Null();
        }

        //[TestMethod]
        //public async Task ShouldeErrorOnAddTruckWhenInvalidManufactoryYear()
        //{
        //    var truck = new Truck(0, ModelType.FH, 2000, 2001, "Teste");
        //    var truckViewModel = truck.ToView();

        //    Exception expectedEx = null;
        //    try
        //    {
        //        await _app.Add(truckViewModel);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        expectedEx = ex;
        //    }

        //    expectedEx.Should().Not.Be.Null();
        //    expectedEx.Message.Should().Be.Equals("Ano de fabricação não pode ser inferior ao ano atual");
        //    _dbContextMock.Verify(x => x.Trucks.Add(It.IsAny<Truck>()), Times.Never);
        //    _dbContextMock.Verify(x => x.Save(), Times.Never);
        //}

        [TestMethod]
        public async Task ShouldeErrorOnAddTruckWhenInvalidName()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Add(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Nome inválido");
            _dbContextMock.Verify(x => x.Trucks.Add(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnAddTruckWhenInvalidManufactureYear()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year - 1, DateTime.Today.Year - 1, "Teste");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Add(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Ano de fabricação não pode ser diferente do ano atual");
            _dbContextMock.Verify(x => x.Trucks.Add(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnAddTruckWhenInvalidModelYearLess()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year - 1, "Teste");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Add(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Ano do modelo não pode ser inferior ao ano de fabricação");
            _dbContextMock.Verify(x => x.Trucks.Add(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnAddTruckWhenInvalidModelYearGreater()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 2, "Teste");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Add(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Ano do modelo não pode ser superior ao ano subsequente ao de fabricação");
            _dbContextMock.Verify(x => x.Trucks.Add(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnAddTruckWhenTruckDuplicated()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste");
            var truckViewModel = truck.ToView();

            var trucks = new List<Truck>() {
                truck
            };

            var mock = trucks.AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(c => c.Trucks).Returns(mock.Object);

            Exception expectedEx = null;
            try
            {
                await _app.Add(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            _dbContextMock.Verify(x => x.Trucks.Add(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldAddTruck()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste");
            var truckViewModel = truck.ToView();

            var trucks = new List<Truck>();

            var mock = trucks.AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(c => c.Trucks).Returns(mock.Object);

            _dbContextMock.Setup(x => x.Trucks.Add(It.IsAny<Truck>()));

            _dbContextMock.Setup(x => x.Trucks.FindAsync(It.IsAny<int>())).ReturnsAsync(truck);

            var result = await _app.Add(truckViewModel);

            result.Should().Not.Be.Null();
            _dbContextMock.Verify(x => x.Trucks.Add(It.IsAny<Truck>()), Times.Once);
            _dbContextMock.Verify(x => x.Save(), Times.Once);
        }


        [TestMethod]
        public async Task ShouldeErrorOnEditTruckWhenInvalidManufactoryYear()
        {
            var truck = new Truck(0, ModelType.FH, 2000, 2001, "Teste");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Edit(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Ano de fabricação não pode ser inferior ao ano atual");
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnEditTruckWhenInvalidType()
        {
            var truck = new Truck(0, ModelType.None, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Edit(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Modelo inválido");
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnEditTruckWhenInvalidName()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Edit(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Nome inválido");
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnEditTruckWhenInvalidModelYearLess()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year - 1, "Teste");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Edit(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Ano do modelo não pode ser inferior ao ano de fabricação");
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnEditTruckWhenInvalidModelYearGreater()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 2, "Teste");
            var truckViewModel = truck.ToView();

            Exception expectedEx = null;
            try
            {
                await _app.Edit(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Ano do modelo não pode ser superior ao ano subsequente ao de fabricação");
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnEditTruckWhenTrucNotFound()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste");
            var truck2 = new Truck(1, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste2");
            var truckViewModel = truck.ToView();

            var trucks = new List<Truck>() {
                truck,
                truck2
            };

            var mock = trucks.AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(c => c.Trucks).Returns(mock.Object);

            Exception expectedEx = null;
            try
            {
                await _app.Edit(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            expectedEx.Message.Should().Be.Equals("Caminhão não encontrado");
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeErrorOnEditTruckWhenTruckDuplicated()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste");
            var truck2 = new Truck(1, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste");
            var truckViewModel = truck.ToView();

            var trucks = new List<Truck>() {
                truck,
                truck2
            };

            var mock = trucks.AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(c => c.Trucks).Returns(mock.Object);

            _dbContextMock.Setup(x => x.Trucks.FindAsync(It.IsAny<int>())).ReturnsAsync(truck);

            Exception expectedEx = null;
            try
            {
                await _app.Edit(truckViewModel);
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldEditTruck()
        {
            var truck = new Truck(0, ModelType.FH, DateTime.Today.Year, DateTime.Today.Year + 1, "Teste");
            var truckViewModel = truck.ToView();

            var trucks = new List<Truck>();

            var mock = trucks.AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(c => c.Trucks).Returns(mock.Object);

            _dbContextMock.Setup(x => x.Trucks.Update(It.IsAny<Truck>()));

            _dbContextMock.Setup(x => x.Trucks.FindAsync(It.IsAny<int>())).ReturnsAsync(truck);

            var result = await _app.Edit(truckViewModel);

            result.Should().Not.Be.Null();
            _dbContextMock.Verify(x => x.Trucks.Update(It.IsAny<Truck>()), Times.Once);
            _dbContextMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public async Task ShouldeErrorOnDeleteTruckWhenTruckNoExists()
        {
            Truck truck = null;

            _dbContextMock.Setup(x => x.Trucks.FindAsync(It.IsAny<int>())).ReturnsAsync(truck);

            Exception expectedEx = null;
            try
            {
                await _app.Delete(It.IsAny<int>());
            }
            catch (System.Exception ex)
            {
                expectedEx = ex;
            }

            expectedEx.Should().Not.Be.Null();
            _dbContextMock.Verify(x => x.Trucks.FindAsync(It.IsAny<int>()), Times.Once);
            _dbContextMock.Verify(x => x.Trucks.Remove(It.IsAny<Truck>()), Times.Never);
            _dbContextMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod]
        public async Task ShouldeDeleteTruck()
        {
            var truck = new Truck();

            _dbContextMock.Setup(x => x.Trucks.FindAsync(It.IsAny<int>())).ReturnsAsync(truck);

            await _app.Delete(It.IsAny<int>());

            _dbContextMock.Verify(x => x.Trucks.FindAsync(It.IsAny<int>()), Times.Once);
            _dbContextMock.Verify(x => x.Trucks.Remove(It.IsAny<Truck>()), Times.Once);
            _dbContextMock.Verify(x => x.Save(), Times.Once);
        }
    }
}
