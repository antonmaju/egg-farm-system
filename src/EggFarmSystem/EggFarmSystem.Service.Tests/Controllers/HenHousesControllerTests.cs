using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using EggFarmSystem.Service.Controllers;
using Moq;
using Xunit;

namespace EggFarmSystem.Service.Tests.Controllers
{
    public class HenHousesControllerTests
    {
        private readonly HenHousesController controller;
        private readonly Mock<IHenHouseService> houseServiceMock;

        public HenHousesControllerTests()
        {
            houseServiceMock = new Mock<IHenHouseService>();
            controller = new HenHousesController(houseServiceMock.Object);
        }

        [Fact]
        public void Can_GetAll()
        {
            var result = new List<HenHouse>();
            houseServiceMock.Setup(s => s.GetAll()).Returns(result);
            Assert.Equal(result,controller.GetAll());
        }

        [Fact]
        public void GetId_ThrowsErrorIfNotFound()
        {
            Guid id = Guid.NewGuid();
            HenHouse house = null;
            houseServiceMock.Setup(s => s.Get(id)).Returns(house);
            Assert.Throws<HttpResponseException>(() => controller.GetById(id));
        }

        [Fact]
        public void Can_GetId()
        {
            Guid id = Guid.NewGuid();
            HenHouse house = new HenHouse();
            houseServiceMock.Setup(s => s.Get(id)).Returns(house);
            Assert.Equal(house, controller.GetById(id));
        }

    }
}
