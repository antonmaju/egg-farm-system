using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Usage.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.Usage.ViewModels
{
    public class UsageListViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IConsumableUsageService usageService;

        

        public UsageListViewModel(IMessageBroker messageBroker, IConsumableUsageService usageService,
            NewUsageCommand newCommand, EditUsageCommand editCommand, DeleteUsageCommand deleteCommand)
        {
            this.messageBroker = messageBroker;
            this.usageService = usageService;

            NewCommand = newCommand;
            EditCommand = editCommand;
            DeleteCommand = deleteCommand;

            NavigationCommands = new List<CommandBase>(){NewCommand, DeleteCommand};

            SubscribeMessages();
        }

        #region commands

        public NewUsageCommand NewCommand { get; private set; }

        public EditUsageCommand EditCommand { get; private set; }

        public DeleteUsageCommand DeleteCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        #endregion

        #region text

        public string DateText { get { return LanguageData.Usage_DateField; } }

        public string TotalText { get { return LanguageData.Usage_TotalField; } }

        #endregion

        #region properties

        private ObservableCollection<ConsumableUsage> usageList;

        public ObservableCollection<ConsumableUsage> UsageList
        {
            get { return usageList; }
            set { 
                usageList = value;
                OnPropertyChanged("UsageList");
            }
        }


        private int pageIndex;

        public int PageIndex
        {
            get { return pageIndex; }
            set 
            {
                pageIndex = value;
                OnPropertyChanged("PageIndex");
            }
        }

        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set 
            { 
                pageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        private DateTime? startDate;

        public DateTime? StartDate
        {
            get { return startDate; }

            set 
            { 
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime? endDate;

        public DateTime? EndDate
        {
            get { return endDate; }

            set 
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }


        #endregion

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.DeleteUsageSuccess, OnRefreshList);
            messageBroker.Subscribe(CommonMessages.DeleteUsageFailed, OnDeleteFailed);
            messageBroker.Subscribe(CommonMessages.RefreshUsageList, OnRefreshList);
        }

        void OnRefreshList(object param)
        {
            var searchInfo = new ConsumableUsageSearchInfo
                {
                    Start = startDate,
                    End = endDate,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            usageService.Search(searchInfo);
        }

        void OnDeleteFailed(object param)
        {
            
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.DeleteUsageSuccess, OnRefreshList);
            messageBroker.Unsubscribe(CommonMessages.DeleteUsageFailed, OnDeleteFailed);
            messageBroker.Unsubscribe(CommonMessages.RefreshUsageList, OnRefreshList);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
