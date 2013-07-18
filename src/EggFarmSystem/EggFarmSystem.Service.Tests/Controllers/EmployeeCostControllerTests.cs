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
    public class EmployeeCostControllerTests : ControllerTestBase
    {
        private readonly EmployeeCostController controller;
        private readonly Mock<IEmployeeCostService> costServiceMock;

        public EmployeeCostControllerTests()
        {
            costServiceMock = new Mock<IEmployeeCostService>();
            controller = new EmployeeCostController(costServiceMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [Fact]
        public void Can_Search()
        {
            var result = new SearchResult<EmployeeCost>();
            costServiceMock.Setup(s => s.Search(It.IsAny<DateRangeSearchInfo>())).Returns(result);
            Assert.Equal(result, controller.GetByCriteria(1,10));
        }

        [Fact]
        public void GetById_ThrowsErrorIfNotFound()
        {
            Guid id = Guid.NewGuid();
            EmployeeCost cost = null;
            costServiceMock.Setup(s => s.Get(id)).Returns(cost);
            Assert.Throws<HttpResponseException>(() => controller.Get(id));
        }

        [Fact]
        public void Can_GetById()
        {
            Guid id = Guid.NewGuid();
            var cost = new EmployeeCost();
            costServiceMock.Setup(s => s.Get(id)).Returns(cost);
            Assert.Equal(cost, controller.Get(id));
        }

        [Fact]
        public void GetByDate_ThrowsErrorIfNotFound()
        {
            DateTime today = DateTime.Today;
            EmployeeCost cost = null;
            costServiceMock.Setup(s => s.GetByDate(today)).Returns(cost);
            Assert.Throws<HttpResponseException>(() => controller.GetByDate(today));
        }

        [Fact]
        public void Can_GetByDate()
        {
            DateTime today = DateTime.Today;
            var cost = new EmployeeCost();
            costServiceMock.Setup(s => s.GetByDate(today)).Returns(cost);
            Assert.Equal(cost, controller.GetByDate(today));
        }
        
        [Fact]
        public void Post_ReturnsBadRequest_IfValueIsInvalid()
        {
            var cost = new EmployeeCost();
            var response = controller.Post(cost);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void Post_ReturnsInternalServerError_IfSavingFailed()
        {
            var cost = new EmployeeCost
                {
                    Date = DateTime.Today,
                    Total = 100,
                    Details = new List<EmployeeCostDetail>()
                        {
                            new EmployeeCostDetail {EmployeeId = Guid.NewGuid(), Present = true, Salary = 100}
                        }
                };

            costServiceMock.Setup(c => c.Save(cost)).Throws(new Exception());
            var response = controller.Post(cost);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }


        //TODO:
        //find a way to mock Url.Link
        //[Fact]
        public void Post_SaveEmployeeCost()
        {
            var cost = new EmployeeCost
            {
                Date = DateTime.Today,
                Total = 100,
                Details = new List<EmployeeCostDetail>()
                        {
                            new EmployeeCostDetail {EmployeeId = Guid.NewGuid(), Present = true, Salary = 100}
                        }
            };
            var response = controller.Post(cost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void Put_ReturnsBadRequest_IfValueIsInvalid()
        {
            var cost = new EmployeeCost();
            var response = controller.Put(Guid.NewGuid(), cost);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void Put_ReturnsInternalServerError_IfSavingFailed()
        {
            var cost = new EmployeeCost
            {
                Id=Guid.NewGuid(),
                Date = DateTime.Today,
                Total = 100,
                Details = new List<EmployeeCostDetail>()
                        {
                            new EmployeeCostDetail {EmployeeId = Guid.NewGuid(), Present = true, Salary = 100}
                        }
            };

            costServiceMock.Setup(c => c.Save(cost)).Throws(new Exception());
            var response = controller.Put(cost.Id,cost);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void Put_SaveEmployeeCost()
        {
            var cost = new EmployeeCost
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today,
                Total = 100,
                Details = new List<EmployeeCostDetail>()
                        {
                            new EmployeeCostDetail {EmployeeId = Guid.NewGuid(), Present = true, Salary = 100}
                        }
            };
            var response = controller.Put(cost.Id, cost);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Delete_ReturnsInternalServerError_IfDeleteFailed()
        {
            Guid id = Guid.NewGuid();
            costServiceMock.Setup(s => s.Delete(id)).Throws(new Exception());
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
