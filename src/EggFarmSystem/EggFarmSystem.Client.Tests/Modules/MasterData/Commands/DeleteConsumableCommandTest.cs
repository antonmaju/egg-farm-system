using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EggFarmSystem.Client.Tests.Modules.MasterData.Commands
{
    public class DeleteConsumableCommandTest
    {
        private Mock<IMessageBroker> brokerMock;
        private Mock<IConsumableService> serviceMock;
        private DeleteConsumableCommand command;

        public DeleteConsumableCommandTest()
        {
            brokerMock = new Mock<IMessageBroker>();
            serviceMock = new Mock<IConsumableService>();
            command = new DeleteConsumableCommand(brokerMock.Object, serviceMock.Object);
        }

        [Fact]
        public void Should_Publish_DeleteConsumableFailed_IfDeleteFailed()
        {
            Guid randomId = Guid.NewGuid();
            var exception = new Exception("Any exception");
            serviceMock.Setup(s => s.Delete(randomId)).Throws(exception);
            command.Execute(randomId);
            brokerMock.Verify(b => b.Publish(CommonMessages.DeleteConsumableFailed, It.IsAny<Error>()));
        }

    }
}
