using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Text;
using TruckManager.Domain.ViewModels;

namespace TruckManager.Test
{
    [TestClass]
    public class ResulViewModelTest
    {
        [TestMethod]
        public void ShouldReturnViewModelResultEmpty()
        {
            var truckViewModel = new TruckViewModel();

            var result = new ResulViewModel<TruckViewModel>();

            result.Should().Not.Be.Null();
        }
        [TestMethod]
        public void ShouldReturnViewModelResul()
        {
            var truckViewModel = new TruckViewModel();

            var result = new ResulViewModel<TruckViewModel>(truckViewModel);

            result.Should().Not.Be.Null();
        }
        [TestMethod]
        public void ShouldReturnViewModelResulError()
        {
            var ex = new Exception("Test");

            var result = new ResulViewModel<TruckViewModel>(ex);

            result.Should().Not.Be.Null();
        }
    }
}
