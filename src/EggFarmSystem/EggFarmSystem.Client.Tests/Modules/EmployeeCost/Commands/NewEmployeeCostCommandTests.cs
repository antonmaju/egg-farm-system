using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using Moq;
using Xunit;

namespace EggFarmSystem.Client.Tests.Modules.EmployeeCost.Commands
{
    public class NewEmployeeCostCommandTests 
    {
        private readonly Mock<IMessageBroker> brokerMock;
        private NewEmployeeCostCommand command;

        public NewEmployeeCostCommandTests()
        {
            brokerMock = new Mock<IMessageBroker>();
            command = new NewEmployeeCostCommand(brokerMock.Object);
        }

        [Fact]
        public void Command_ShouldPublishNewEmployeeMessage()
        {
            command.Execute(null);
            brokerMock.Verify(b => b.Publish(CommonMessages.NewEmployeeCostEntry, null));
        }
    }
}
