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
    public class AdditionalCostControllerTests : ControllerTestBase
    {
        private readonly AdditionalCostController controller;
        private readonly Mock<IAdditionalCostService> costServiceMock;

        public AdditionalCostControllerTests()
        {
            costServiceMock = new Mock<IAdditionalCostService>();
            controller = new AdditionalCostController(costServiceMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [Fact]
        public void Can_GetAll()
        {
            var result = new List<AdditionalCost>();
            costServiceMock.Setup(s => s.GetAll()).Returns(result);
            Assert.Equal(result, controller.GetAll());
        }

        [Fact]
        public void GetId_ThrowsErrorsIfNotFound()
        {
            Guid id = Guid.NewGuid();
            AdditionalCost cost = null;
            costServiceMock.Setup(s => s.Get(id)).Returns(cost);
            Assert.Throws<HttpResponseException>(() => controller.GetById(id));
        }

        [Fact]
        public void Can_GetId()
        {
            Guid id = Guid.NewGuid();
            var cost = new AdditionalCost();
            costServiceMock.Setup(s => s.Get(id)).Returns(cost);
            Assert.Equal(cost, controller.GetById(id));
        }

        #region post

        [Fact]
        public void Post_GetBadRequest_IfNameIsNull()
        {
            var cost = new AdditionalCost {Value = 1};
            var msg = controller.Post(cost);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Name");
        }

        [Fact]
        public void Post_GetBadRequest_IfValueIsZero()
        {
            var cost = new AdditionalCost {Name = "a"};
            var msg = controller.Post(cost);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Value");
        }

        [Fact]
        public void Post_ReturnsInternalServerError_IfSavingFailed()
        {
           var cost = new AdditionalCost {Name = "a", Value = 1};
           costServiceMock.Setup(c => c.Save(cost)).Throws(new Exception("Whatever"));
           var msg = controller.Post(cost);
           Assert.Equal(HttpStatusCode.InternalServerError, msg.StatusCode);
        }

        [Fact]
        public void Post_ReturnsCreated()
        {
            var cost = new AdditionalCost { Name = "a", Value = 1 };
            var msg = controller.Post(cost);
            
            //TODO: try find an easier way to mock Url.Link 
            //Assert.Equal(HttpStatusCode.Created, msg.StatusCode);
        }

        #endregion 

        #region put

        [Fact]
        public void Put_GetBadRequest_IfNameIsNull()
        {
            Guid id = Guid.NewGuid();
            var cost = new AdditionalCost { Id= id, Value = 1 };
            var msg = controller.Put(id, cost);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Name");
        }

        [Fact]
        public void Put_GetBadRequest_IfValueIsZero()
        {
            Guid id = Guid.NewGuid();
            var cost = new AdditionalCost {Id= id,  Name = "a" };
            var msg = controller.Put(id, cost);
            Assert.Equal(HttpStatusCode.BadRequest, msg.StatusCode);
            AssertPropertyError(msg, "Value");
        }

        [Fact]
        public void Put_ReturnsInternalServerError_IfSavingFailed()
        {
            Guid id = Guid.NewGuid();
            var cost = new AdditionalCost {Id = id, Name = "a", Value = 1 };
            costServiceMock.Setup(c => c.Save(cost)).Throws(new Exception("Whatever"));
            var msg = controller.Put(id, cost);
            Assert.Equal(HttpStatusCode.InternalServerError, msg.StatusCode);
        }

        [Fact]
        public void Put_ReturnsOK()
        {
            Guid id = Guid.NewGuid();
            var cost = new AdditionalCost { Id=id, Name = "a", Value = 1 };
            var msg = controller.Put(id, cost);
            Assert.Equal(HttpStatusCode.OK, msg.StatusCode);
        }

        #endregion

        #region delete

        public void Delete_ReturnsInternalServerError_IfDeleteFailed()
        {
            Guid id = Guid.NewGuid();
            costServiceMock.Setup(s => s.Delete(id)).Throws(new Exception());
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void Can_DeleteCost()
        {
            Guid id = Guid.NewGuid();
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion
    }
}
