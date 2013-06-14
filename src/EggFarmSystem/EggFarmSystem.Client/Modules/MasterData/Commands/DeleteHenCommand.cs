using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteHenCommand : DeleteMasterDataCommand
    {
        private readonly IHenService henService;

        public DeleteHenCommand(IMessageBroker messageBroker, IHenService henService)
            : base(messageBroker)
        {
            this.henService = henService;
        }

        protected override void OnDeleteMasterData(Guid entityId)
        {
            henService.Delete(entityId);
        }
    }
}
