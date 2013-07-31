using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Hosting;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using EggFarmSystem.Service.Controllers;
using Moq;
using Xunit;

namespace EggFarmSystem.Service.Tests.Controllers
{
    public class HenHousesControllerTests : ControllerTestBase
    {
        private readonly HenHousesController controller;
        private readonly Mock<IHenHouseService> houseServiceMock;

        public HenHousesControllerTests()
        {
            houseServiceMock = new Mock<IHenHouseService>();
            controller = new HenHousesController(houseServiceMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
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

        #region post

        [Fact]
        public void Post_GetBadRequest_IfNameIsNull()
        {
            var house = new HenHouse
                {
                    Active = false,
                    Depreciation = 12,
                    PurchaseCost = 1,
                    YearUsage = 1
                };

            var msg = controller.Post(house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Name");
        }

        [Fact]
        public void Post_GetBadRequest_IfPurchaseCostIsZero()
        {
            var house = new HenHouse
            {
                Active = false,
                Depreciation = 12,
                Name="a",
                YearUsage = 1
            };

            var msg = controller.Post(house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "PurchaseCost");
        }

        [Fact]
        public void Post_GetBadRequest_IfDepreciationIsZero()
        {
            var house = new HenHouse
            {
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                YearUsage = 1
            };

            var msg = controller.Post(house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Depreciation");
        }

        [Fact]
        public void Post_GetBadRequest_IfYearUsageIsZero()
        {
            var house = new HenHouse
            {
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                Depreciation = 1
            };

            var msg = controller.Post(house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "YearUsage");
        }

        [Fact]
        public void Post_ReturnsInternalServerError_IfSavingFailed()
        {
            var house = new HenHouse
            {
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                Depreciation = 1,
                YearUsage = 1
            };

            houseServiceMock.Setup(c => c.Save(house)).Throws(new Exception("Whatever"));
            var msg = controller.Post(house);
            Assert.Equal(HttpStatusCode.InternalServerError, msg.StatusCode);
        }

        [Fact]
        public void Can_PostHouse()
        {
            var house = new HenHouse
            {
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                Depreciation = 1,
                YearUsage = 1
            };

            controller.Post(house);
        }

        #endregion

        #region put

        [Fact]
        public void Put_GetBadRequest_IfNameIsNull()
        {
            Guid id = Guid.NewGuid();

            var house = new HenHouse
            {
                Id = id,
                Active = false,
                Depreciation = 12,
                PurchaseCost = 1,
                YearUsage = 1
            };

            var msg = controller.Put(id,house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Name");
        }

        [Fact]
        public void Put_GetBadRequest_IfPurchaseCostIsZero()
        {
            Guid id = Guid.NewGuid();

            var house = new HenHouse
            {
                Id = id,
                Active = false,
                Depreciation = 12,
                Name = "a",
                YearUsage = 1
            };

            var msg = controller.Put(id,house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "PurchaseCost");
        }

        [Fact]
        public void Put_GetBadRequest_IfDepreciationIsZero()
        {
            Guid id = Guid.NewGuid();

            var house = new HenHouse
            {
                Id = id,
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                YearUsage = 1
            };

            var msg = controller.Put(id, house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Depreciation");
        }

        [Fact]
        public void Put_GetBadRequest_IfYearUsageIsZero()
        {
            Guid id = Guid.NewGuid();

            var house = new HenHouse
            {
                Id = id,
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                Depreciation = 1
            };

            var msg = controller.Put(id, house);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "YearUsage");
        }

        [Fact]
        public void Put_ReturnsInternalServer_IfSavingFailed()
        {
            Guid id = Guid.NewGuid();
            var house = new HenHouse
            {
                Id=id,
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                Depreciation = 1,
                YearUsage = 1
            };

            houseServiceMock.Setup(c => c.Save(house)).Throws(new Exception("Whatever"));
            var msg = controller.Put(id, house);
            Assert.Equal(HttpStatusCode.InternalServerError, msg.StatusCode);
        }

        [Fact]
        public void Can_PutHouse()
        {
            Guid id = Guid.NewGuid();
            var house = new HenHouse
            {
                Id = id,
                Active = false,
                Name = "a",
                PurchaseCost = 1,
                Depreciation = 1,
                YearUsage = 1
            };

            controller.Put(id, house);
        }

        #endregion 

        [Fact]
        public void Delete_ReturnsInternalServerError_IfDeleteFailed()
        {
            Guid id = Guid.NewGuid();
            houseServiceMock.Setup(s => s.Delete(id)).Throws(new Exception());
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void Can_DeleteHouse()
        {
            Guid id = Guid.NewGuid();
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
