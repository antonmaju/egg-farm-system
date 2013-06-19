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
    public class HensControllerTests : ControllerTestBase
    {
        private readonly HensController controller;
        private readonly Mock<IHenService> henServiceMock;

        public HensControllerTests()
        {
            henServiceMock = new Mock<IHenService>();
            controller = new HensController(henServiceMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [Fact]
        public void Can_GetAll()
        {
            var result = new List<Hen>();
            henServiceMock.Setup(s => s.GetAll()).Returns(result);
            Assert.Equal(result, controller.GetAll());
        }

        [Fact]
        public void GetId_ThrowsErrorIfNotFound()
        {
            Guid id = Guid.NewGuid();
            Hen hen = null;
            henServiceMock.Setup(s => s.Get(id)).Returns(hen);
            Assert.Throws<HttpResponseException>(() => controller.GetById(id));
        }

        [Fact]
        public void Can_GetId()
        {
            Guid id = Guid.NewGuid();
            var hen = new Hen();
            henServiceMock.Setup(s => s.Get(id)).Returns(hen);
            Assert.Equal(hen, controller.GetById(id));
        }

        [Fact]
        public void Delete_ReturnsInternalServerError_IfDeleteFailed()
        {
            Guid id = Guid.NewGuid();
            henServiceMock.Setup(s => s.Delete(id)).Throws(new Exception());
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

      

        [Fact]
        public void Can_DeleteHen()
        {
            Guid id = Guid.NewGuid();
            var response = controller.Delete(id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
