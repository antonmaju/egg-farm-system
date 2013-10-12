using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EmployeeCost.ViewModels
{
    public class EmployeeCostListViewModel :ViewModelBase, IPagingInfo
    {
        private readonly IMessageBroker broker;
        private readonly IEmployeeCostService costService;

        public EmployeeCostListViewModel(IMessageBroker broker, IClientContext clientContext, IEmployeeCostService costService,
            NewEmployeeCostCommand newCommand, EditEmployeeCostCommand editCommand, DeleteEmployeeCostCommand deleteCommand,
            RefreshCommand refreshCommand)
        {
            this.broker = broker;
            this.costService = costService;
            pageSize = clientContext.PageSize;

            NewCommand = newCommand;
            EditCommand = editCommand;
            DeleteCommand = deleteCommand;
            RefreshCommand = refreshCommand;
            RefreshCommand.MessageName = CommonMessages.RefreshEmployeeCostList;
            NavigationCommands = new List<CommandBase>{NewCommand,  DeleteCommand, RefreshCommand};

            SubscribeMessages();
        }

        #region commands

        public NewEmployeeCostCommand NewCommand { get; private set; }

        public EditEmployeeCostCommand EditCommand { get; private set; }

        public DeleteEmployeeCostCommand DeleteCommand { get; private set; }

        public RefreshCommand RefreshCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; } 

        #endregion

        #region text

        public string DateText { get { return LanguageData.EmployeeCost_DateField; } }

        public string TotalText { get { return LanguageData.EmployeeCost_TotalField; } }
        
        #endregion

        #region properties

        private ObservableCollection<Models.EmployeeCost> costList;
 
        public ObservableCollection<Models.EmployeeCost> CostList
        {
            get { return costList; }
            set
            {
                costList = value;
                OnPropertyChanged("CostList");
            }
        }

        private int pageSize = 20;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize= value;
                OnPropertyChanged("PageSize");
                OnRefresh(null);
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
                pageIndex= value;
                OnPropertyChanged("PageIndex");
                OnRefresh(null);
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
                OnRefresh(null);
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
                OnRefresh(null);
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
    
        void SubscribeMessages()
        {
            broker.Subscribe(CommonMessages.DeleteEmployeeCostSuccess, OnRefresh);
            broker.Subscribe(CommonMessages.DeleteEmployeeCostFailed, OnDeleteFailed);
            broker.Subscribe(CommonMessages.RefreshEmployeeCostList, OnRefresh);
        }

        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.DeleteEmployeeCostSuccess, OnRefresh);
            broker.Unsubscribe(CommonMessages.DeleteEmployeeCostFailed, OnDeleteFailed);
            broker.Unsubscribe(CommonMessages.RefreshEmployeeCostList, OnRefresh);
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
            var result = costService.Search(searchInfo);
            CostList = new ObservableCollection<Models.EmployeeCost>(result.Items);
            TotalRecords = result.Total;
            DeleteCommand.EntityId = Guid.Empty;
        }

        void OnDeleteFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
