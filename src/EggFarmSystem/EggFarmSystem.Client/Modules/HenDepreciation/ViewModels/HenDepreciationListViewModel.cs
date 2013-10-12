using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.HenDepreciation.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.HenDepreciation.ViewModels
{
    public class HenDepreciationListViewModel : ViewModelBase, IPagingInfo
    {
        private readonly IMessageBroker broker;
        private readonly IHenDepreciationService service;

        public HenDepreciationListViewModel(IMessageBroker broker, IClientContext clientContext,  IHenDepreciationService service,
            NewHenDepreciationCommand newCommand, EditHenDepreciationCommand editCommand, DeleteHenDepreciationCommand deleteCommand, RefreshCommand refreshCommand)
        {
            this.broker = broker;
            this.service =service;
            pageSize = clientContext.PageSize;

            NewCommand = newCommand;
            EditCommand = editCommand;
            DeleteCommand = deleteCommand;
            RefreshCommand = refreshCommand;

            RefreshCommand.MessageName = CommonMessages.RefreshHenDepreciationList;
            NavigationCommands = new List<CommandBase>() { NewCommand, DeleteCommand, RefreshCommand };

            SubscribeMessages();
        }

        #region commands

        public NewHenDepreciationCommand NewCommand { get; private set; }

        public EditHenDepreciationCommand EditCommand { get; private set; }

        public DeleteHenDepreciationCommand DeleteCommand { get; private set; }

        public RefreshCommand RefreshCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; } 

        #endregion


        #region text

        public string DateText { get { return LanguageData.HenDepreciation_DateField; } }

        #endregion

        #region properties

        private ObservableCollection<Models.HenDepreciation> depreciationList;

        public ObservableCollection<Models.HenDepreciation> DepreciationList
        {
            get { return depreciationList; }
            set 
            {
                depreciationList = value;
                OnPropertyChanged("DepreciationList");
            }
        }

        /// <summary>
        /// This settings can be refactored somewhere
        /// </summary>
        private int pageSize = 20;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize=value;
                OnPropertyChanged("PageSize");
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
            get
            {
                return totalPage;
            }
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
            broker.Subscribe(CommonMessages.DeleteHenDepreciationFailed, OnDeleteFailed);
            broker.Subscribe(CommonMessages.DeleteHenDepreciationSuccess, OnRefresh);
            broker.Subscribe(CommonMessages.RefreshHenDepreciationList, OnRefresh);
        }


        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.DeleteHenDepreciationFailed, OnDeleteFailed);
            broker.Unsubscribe(CommonMessages.DeleteHenDepreciationSuccess, OnRefresh);
            broker.Unsubscribe(CommonMessages.RefreshHenDepreciationList, OnRefresh);
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
            DepreciationList = new ObservableCollection<Models.HenDepreciation>(result.Items);
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
