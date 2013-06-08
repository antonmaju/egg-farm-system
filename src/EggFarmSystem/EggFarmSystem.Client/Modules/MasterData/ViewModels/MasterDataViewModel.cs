using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.MasterData.Views;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class MasterDataViewModel : ViewModelBase
    {
        private bool _isEmployeeInput;
        private bool _isHenInput;
        private bool _isHouseInput;
        private readonly IMessageBroker messageBroker;

        private Lazy<IHenListView> henListProxy;
        private Lazy<IHenHouseListView> houseListProxy;
        private Lazy<IEmployeeListView> employeeListProxy;
        private Lazy<IHenEntryView> henEntryProxy;
        private Lazy<IHenHouseEntryView> henHouseEntryProxy;
        private Lazy<IEmployeeEntryView> employeeEntryProxy;

        public MasterDataViewModel(
            IMessageBroker messageBroker,
            Lazy<IHenListView> henListProxy,
            Lazy<IHenHouseListView> houseListProxy,
            Lazy<IEmployeeListView> employeeListProxy,
            Lazy<IHenEntryView> henEntryProxy,
            Lazy<IHenHouseEntryView>  houseEntryProxy,
            Lazy<IEmployeeEntryView> employeeEntryProxy 
            )
        {
            this.messageBroker = messageBroker;
            this.henListProxy = henListProxy;
            this.houseListProxy = houseListProxy;
            this.employeeListProxy = employeeListProxy;

            this.henEntryProxy = henEntryProxy;
            this.henHouseEntryProxy = houseEntryProxy;
            this.employeeEntryProxy = employeeEntryProxy;

            InitializeCommand();
            SetMessageListeners();
        }

        public void InitializeContent()
        {
            HenListCommand.Execute(null);
        }

        public bool IsEmployeeInput
        {
            get { return _isEmployeeInput; }
            set {
                _isEmployeeInput = value;
                if(value) ResetSelection("Employee");
                OnPropertyChanged("IsEmployeeInput");
            }
        }

        public bool IsHenInput
        {
            get { return _isHenInput; }
            set {
                _isHenInput = value;
                if (value) ResetSelection("Hen");
                OnPropertyChanged("IsHenInput");
            }
        }

        public bool IsHouseInput
        {
            get { return _isHouseInput; }
            set 
            {
                _isHouseInput = value;
                if (value) ResetSelection("House");
                OnPropertyChanged("IsHouseInput");
            }
        }

        void ResetSelection(string mode)
        {
            switch (mode)
            {
                case "Hen":
                    IsEmployeeInput = false;
                    IsHouseInput = false;
                    break;

                case "House":
                    IsHenInput = false;
                    IsEmployeeInput = false;
                    break;

                case "Employee":
                    IsHenInput = false;
                    IsHouseInput = false;
                    break;
            }
        }

        public ICommand EmployeeListCommand { get; private set; }

        public ICommand HenListCommand { get; private set; }

        public ICommand HouseListCommand { get; private set; }

        void InitializeCommand()
        {
            EmployeeListCommand = new DelegateCommand(param =>
                {
                    IsEmployeeInput = true;
                    var view = employeeListProxy.Value as UserControlBase;
                    Content = view;
                    messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
                },param=>true);

            HenListCommand = new DelegateCommand(OnHenListRefresh, param => true);

            HouseListCommand = new DelegateCommand(param =>
                {
                    IsHouseInput = true;
                    var view = (UserControlBase) houseListProxy.Value;
                    Content = view;
                    messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
                }, param => true);
        }

        private UserControl content;
        public UserControl Content
        {
            get { return content; }
            set 
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        void SetMessageListeners()
        {
            messageBroker.Subscribe(CommonMessages.NewHenView, ShowNewHen);
            messageBroker.Subscribe(CommonMessages.EditHenView,OnHenEditRequest);
            messageBroker.Subscribe(CommonMessages.HenSaved, OnHenListRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteHenSuccess, OnHenListRefresh);
        }

        void UnsetMessageListeners()
        {
            messageBroker.Unsubscribe(CommonMessages.NewHenView, ShowNewHen);
            messageBroker.Unsubscribe(CommonMessages.EditHenView, OnHenEditRequest);
            messageBroker.Unsubscribe(CommonMessages.HenSaved, OnHenListRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteHenSuccess, OnHenListRefresh);
        }

        void OnHenListRefresh(object param)
        {
            IsHenInput = true;
            var view = henListProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.RefreshHenList, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnHenEditRequest(object param)
        {
            var view = henEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.LoadHen,param);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void ShowNewHen(object param)
        {
            var view = henEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.NewHenEntry, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        public override void Dispose()
        {
            UnsetMessageListeners();
        }
    }
}
