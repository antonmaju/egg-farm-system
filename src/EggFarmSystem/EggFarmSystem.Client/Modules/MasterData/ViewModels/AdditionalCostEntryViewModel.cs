using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class AdditionalCostEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker broker;
        private readonly IAdditionalCostService costService;

        public AdditionalCostEntryViewModel(IMessageBroker broker, IAdditionalCostService costService,
                                            SaveAdditionalCostCommand saveCommand, CancelCommand command)
        {
            
        }
    }
}
