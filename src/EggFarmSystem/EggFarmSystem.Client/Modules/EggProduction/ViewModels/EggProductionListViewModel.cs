using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EggProduction.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EggProduction.ViewModels
{
    public class EggProductionListViewModel :ViewModelBase, IPagingInfo
    {
        private readonly IMessageBroker broker;
        private readonly IEggProductionService service;

        public EggProductionListViewModel(
            IMessageBroker broker, IEggProductionService service, NewEggProductionCommand newCommand,
            EditEggProductionCommand editCommand, DeleteEggProductionCommand deleteCommand, RefreshCommand refreshCommand
            )
        {
            this.broker = broker;
            this.service = service;

            NewCommand = newCommand;
            EditCommand = editCommand;
            DeleteCommand = deleteCommand;
            RefreshCommand = refreshCommand;

            RefreshCommand.MessageName = CommonMessages.RefreshEggProductionList;
            NavigationCommands = new List<CommandBase>(){NewCommand, DeleteCommand, RefreshCommand};

            SubscribeMessages();
        }

        #region commands

        public NewEggProductionCommand NewCommand { get; private set; }

        public EditEggProductionCommand EditCommand { get; private set; }

        public DeleteEggProductionCommand DeleteCommand { get; private set; }

        public RefreshCommand RefreshCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        #endregion

        #region text

        public string DateText { get { return LanguageData.EmployeeCost_DateField; } }
        
        #endregion

        #region properties

        private ObservableCollection<Models.EggProduction> productionList;
        private int pageSize = 20;


        public ObservableCollection<Models.EggProduction> ProductionList
        {
            get { return productionList; }
            set { 
                productionList = value; 
                OnPropertyChanged("ProductionList");
            }
        }

        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        private int pageIndex = 1;

        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                pageIndex = value;
                OnPropertyChanged("PageIndex");
            }
        }

        private DateTime? startDate = null;

        public DateTime? StartDate
        {
            get { return startDate; }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime? endDate = null;

        public DateTime? EndDate
        {
            get { return endDate; }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        private int totalRecords;

        public int TotalRecords
        {
            get { return totalRecords; }
            set
            {
                totalRecords = value;
                var total = pageSize > 0 ? (int)Math.Ceiling((double)totalRecords / pageSize) : 0;
                OnPropertyChanged("TotalRecord");
                TotalPage = total;
            }
        }

        private int totalPage;

        public int TotalPage
        {
            get { return totalPage; }
            set
            {
                totalPage = value;
                OnPropertyChanged("TotalPage");
            }
        }

        #endregion

        #region command handlers

        void SubscribeMessages()
        {
            broker.Subscribe(CommonMessages.DeleteEggProductionSuccess, OnRefresh);
            broker.Subscribe(CommonMessages.DeleteEggProductionFailed, OnDeleteFailed);
            broker.Subscribe(CommonMessages.RefreshEggProductionList, OnRefresh);
        }

        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.DeleteEggProductionSuccess, OnRefresh);
            broker.Unsubscribe(CommonMessages.DeleteEggProductionFailed, OnDeleteFailed);
            broker.Unsubscribe(CommonMessages.RefreshEggProductionList, OnRefresh);
        }

        void OnRefresh(object param)
        {
            var searchInfo = new DateRangeSearchInfo
            {
                Start = startDate,
                End = endDate,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = service.Search(searchInfo);
            ProductionList = new ObservableCollection<Models.EggProduction>(result.Items);
            TotalRecords = result.Total;
            DeleteCommand.EntityId = Guid.Empty;
        }

        void OnDeleteFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        #endregion

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
