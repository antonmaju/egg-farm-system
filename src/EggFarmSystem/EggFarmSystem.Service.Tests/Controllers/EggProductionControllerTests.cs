using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Hosting;
using EggFarmSystem.Models;
using EggFarmSystem.Service.Controllers;
using EggFarmSystem.Services;
using Moq;
using Xunit;

namespace EggFarmSystem.Service.Tests.Controllers
{
    public class EggProductionControllerTests : ControllerTestBase
    {
        private readonly EggProductionController controller;
        private readonly Mock<IEggProductionService> serviceMock;

        public EggProductionControllerTests()
        {
            serviceMock = new Mock<IEggProductionService>();
            controller = new EggProductionController(serviceMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [Fact]
        public void GetById_ThrowsErrorIfNotFound()
        {
            Guid id = Guid.NewGuid();
            EggProduction production = null;
            serviceMock.Setup(s => s.Get(id)).Returns(production);
            Assert.Throws<HttpResponseException>(() => controller.Get(id));
        }

        [Fact]
        public void Can_GetById()
        {
            Guid id = Guid.NewGuid();
            var production= new EggProduction();
            serviceMock.Setup(s => s.Get(id)).Returns(production);
            Assert.Equal(production, controller.Get(id));
        }

        [Fact]
        public void GetByDate_ThrowsErrorIfNotFound()
        {
            DateTime today = DateTime.Today;
            EggProduction production = null;
            serviceMock.Setup(s => s.GetByDate(today)).Returns(production);
            Assert.Throws<HttpResponseException>(() => controller.GetByDate(today));
        }

        [Fact]
        public void Can_GetByDate()
        {
            DateTime today = DateTime.Today;
            var production = new EggProduction();
            serviceMock.Setup(s => s.GetByDate(today)).Returns(production);
            Assert.Equal(production, controller.GetByDate(today));
        }

        [Fact]
        public void Post_ReturnsBadRequest_IfValueIsInvalid()
        {
            var production = new EggProduction();
            var response = controller.Post(production);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void Post_ReturnsInternalServerError_IfSavingFailed()
        {
            var production = new EggProduction
            {
                Date = DateTime.Today,
                Details = new List<EggProductionDetail>()
                        {
                            new EggProductionDetail
                                {
                                    HouseId = Guid.NewGuid(),
                                    CrackedEggCount = 1,
                                    Fcr = 1,
                                    GoodEggCount = 1,
                                    RetailQuantity = 1
                                }
                        }
            };

            serviceMock.Setup(c => c.Save(production)).Throws(new Exception());
            var response = controller.Post(production);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }


        //TODO:
        //find a way to mock Url.Link
        //[Fact]
        public void Post_SaveEggProduction()
        {
            var production = new EggProduction
            {
                Date = DateTime.Today,
                Details = new List<EggProductionDetail>()
                        {
                            new EggProductionDetail
                                {
                                    HouseId = Guid.NewGuid(),
                                    CrackedEggCount = 1,
                                    Fcr = 1,
                                    GoodEggCount = 1,
                                    RetailQuantity = 1
                                }
                        }
            };
            var response = controller.Post(production );
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void Put_ReturnsBadRequest_IfValueIsInvalid()
        {
            var production = new EggProduction();
            var response = controller.Put(Guid.NewGuid(), production);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void Put_ReturnsInternalServerError_IfSavingFailed()
        {
            var cost = new EggProduction
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today,
                Details = new List<EggProductionDetail>()
                        {
                            new EggProductionDetail
                                {
                                    HouseId = Guid.NewGuid(),
                                    CrackedEggCount = 1,
                                    Fcr = 1,
                                    GoodEggCount = 1,
                                    RetailQuantity = 1
                                }
                        }
            };

            serviceMock.Setup(c => c.Save(cost)).Throws(new Exception());
            var response = controller.Put(cost.Id, cost);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void Put_SaveEmployeeCost()
        {
            var cost = new EggProduction
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today,
                Details = new List<EggProductionDetail>()
                        {
                            new EggProductionDetail
                                {
                                    HouseId = Guid.NewGuid(),
                                    CrackedEggCount = 1,
                                    Fcr = 1,
                                    GoodEggCount = 1,
                                    RetailQuantity = 1
                                }
                        }
            };
            var response = controller.Put(cost.Id, cost);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Delete_ReturnsInternalServerError_IfDeleteFailed()
        {
            Guid id = Guid.NewGuid();
            serviceMock.Setup(s => s.Delete(id)).Throws(new Exception());
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void Can_Delete()
        {
            Guid id = Guid.NewGuid();
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
