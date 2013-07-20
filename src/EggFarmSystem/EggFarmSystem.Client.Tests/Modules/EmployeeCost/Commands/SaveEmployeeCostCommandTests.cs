using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using Moq;
using Xunit;

namespace EggFarmSystem.Client.Tests.Modules.EmployeeCost.Commands
{
    public class SaveEmployeeCostCommandTests
    {
        private Mock<IMessageBroker> brokerMock;
        private Mock<IEmployeeCostService> serviceMock;
        private SaveEmployeeCostCommand command;

        public SaveEmployeeCostCommandTests()
        {
            brokerMock = new Mock<IMessageBroker>();
            serviceMock = new Mock<IEmployeeCostService>();
            command = new SaveEmployeeCostCommand(brokerMock.Object, serviceMock.Object);
        }

        [Fact]
        public void Command_CantExecute_IfNullCost()
        {
            Assert.False(command.CanExecute(null));
        }

        [Fact]
        public void Command_CanExecuteIf_CosIsSet()
        {
            command.Cost = new Models.EmployeeCost();
            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void Command_PublishFailedMessage_IfSaveFailed()
        {
            var cost = new Models.EmployeeCost();
            serviceMock.Setup(s => s.Save(cost)).Throws<ServiceException>();
            command.Cost = cost;
            command.Execute(null);
            brokerMock.Verify(b => b.Publish(CommonMessages.SaveEmployeeCostFailed, It.IsAny<Error>()));
        }

        [Fact]
        public void Command_PublishSuccessMessage_IfSaveSuccess()
        {
            var cost = new Models.EmployeeCost();
            command.Cost = cost;
            command.Execute(null);
            brokerMock.Verify(b => b.Publish(CommonMessages.SaveEmployeeCostSuccess, null));
        }

    }
}
