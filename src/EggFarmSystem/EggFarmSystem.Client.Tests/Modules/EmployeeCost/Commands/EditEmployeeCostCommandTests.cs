using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using EggFarmSystem.Client.Modules.Usage.Views;
using Moq;
using Xunit;

namespace EggFarmSystem.Client.Tests.Modules.EmployeeCost.Commands
{
    public class EditEmployeeCostCommandTests
    {
        private readonly Mock<IMessageBroker> brokerMock;
        private readonly EditEmployeeCostCommand command;

        public EditEmployeeCostCommandTests()
        {
            brokerMock = new Mock<IMessageBroker>();
            command = new EditEmployeeCostCommand(brokerMock.Object);
        }

        [Fact]
        public void EditEmployeeCost_CantExecuteIfNoId()
        {
            Assert.False(command.CanExecute(null));
        }

        [Fact]
        public void EditEmployeeCost_CanExecuteIfIdIsSupplied()
        {
            command.EntityId = Guid.NewGuid();
            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void EditEmployeeCost_CanExecuteIfIdParamIsSupplied()
        {
            Assert.True(command.CanExecute(Guid.NewGuid()));
        }

        [Fact]
        public void EditEmployeeCost_ShouldPublishLoadCostMessage()
        {
            command.EntityId = Guid.NewGuid();
            command.Execute(null);
            brokerMock.Verify(b => b.Publish(CommonMessages.LoadEmployeeCost, command.EntityId));
        }
    }
}
