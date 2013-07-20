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
    public class DeleteEmployeeCostCommandTests
    {
        private Mock<IMessageBroker> brokerMock;
        private Mock<IEmployeeCostService> costServiceMock;
        private DeleteEmployeeCostCommand command;

        public DeleteEmployeeCostCommandTests()
        {
            brokerMock = new Mock<IMessageBroker>();
            costServiceMock = new Mock<IEmployeeCostService>();
            command = new DeleteEmployeeCostCommand(brokerMock.Object, costServiceMock.Object);
            command.SupressPrompt = true;
        }

        [Fact]
        public void DeleteCommand_CantExecuteIfNoIdSupplied()
        {
            Assert.False(command.CanExecute(null));
        }
        
        [Fact]
        public void DeleteCommand_CanExecute_IfEntityIdSupplied()
        {
            command.EntityId = Guid.NewGuid();
            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void DeleteCommand_CanExecute_IfParamIsSupplied()
        {
            Assert.True(command.CanExecute(Guid.NewGuid()));
        }

        [Fact]
        public void DeleteCommand_PublishFailedMessage_IfErrorOccurs()
        {
            Guid id = Guid.NewGuid();
            costServiceMock.Setup(s => s.Delete(id)).Throws<ServiceException>();
            command.EntityId = id;
            command.Execute(null);
            brokerMock.Verify(b => b.Publish(CommonMessages.DeleteEmployeeCostFailed, It.IsAny<Error>()));
        }

        [Fact]
        public void DeleteCommand_PublishSuccessMessage_IfDeleteSuccess()
        {
            Guid id = Guid.NewGuid();
            command.EntityId = id;
            command.Execute(null);
            brokerMock.Verify(b => b.Publish(CommonMessages.DeleteEmployeeCostSuccess, id));
        }
    }
}
