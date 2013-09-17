﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.HenDepreciation.ViewModels
{
    public class HenDepreciationListViewModel : ViewModelBase, IPagingInfo
    {
        private readonly IMessageBroker broker;
        private readonly IHenDepreciationService service;

        public HenDepreciationListViewModel(IMessageBroker broker, IHenDepreciationService service)
        {
            this.broker = broker;
            this.service =service;
        }

        #region properties

        public int PageSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int PageIndex
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int TotalPage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}