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
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    /// <summary>
    /// TODO: refactor duplication
    /// </summary>
    public class MasterDataViewModel : ViewModelBase
    {
        private bool _isEmployeeInput;
        private bool _isHenInput;
        private bool _isHouseInput;
        private bool _isConsumableInput;
        private bool _isAdditionalCostInput;

        private readonly IMessageBroker messageBroker;

        private Lazy<IHenListView> henListProxy;
        private Lazy<IHenHouseListView> houseListProxy;
        private Lazy<IEmployeeListView> employeeListProxy;
        private Lazy<IConsumableListView> consumableListProxy;
        private Lazy<IAdditionalCostListView> additionalCostListProxy; 

        private Lazy<IHenEntryView> henEntryProxy;
        private Lazy<IHenHouseEntryView> houseEntryProxy;
        private Lazy<IEmployeeEntryView> employeeEntryProxy;
        private Lazy<IConsumableEntryView> consumableEntryProxy;
        private Lazy<IAdditionalCostEntryView> additionalCostEntryProxy; 
        
        
        public MasterDataViewModel(
            IMessageBroker messageBroker,
            Lazy<IHenListView> henListProxy,
            Lazy<IHenHouseListView> houseListProxy,
            Lazy<IEmployeeListView> employeeListProxy,
            Lazy<IConsumableListView> consumableListProxy,
            Lazy<IAdditionalCostListView> additionalCostListProxy,

            Lazy<IHenEntryView> henEntryProxy,
            Lazy<IHenHouseEntryView>  houseEntryProxy,
            Lazy<IEmployeeEntryView> employeeEntryProxy,
            Lazy<IConsumableEntryView> consumableEntryProxy,
            Lazy<IAdditionalCostEntryView> additionalCostEntryProxy 
            )
        {
            this.messageBroker = messageBroker;
            this.henListProxy = henListProxy;
            this.houseListProxy = houseListProxy;
            this.employeeListProxy = employeeListProxy;
            this.consumableListProxy = consumableListProxy;
            this.additionalCostListProxy = additionalCostListProxy;

            this.henEntryProxy = henEntryProxy;
            this.houseEntryProxy = houseEntryProxy;
            this.employeeEntryProxy = employeeEntryProxy;
            this.consumableEntryProxy = consumableEntryProxy;
            this.additionalCostEntryProxy = additionalCostEntryProxy;

            InitializeCommand();
            SetMessageListeners();
        }

        public void InitializeContent()
        {
            HenListCommand.Execute(null);
        }

        #region Text

        public string HenText { get { return LanguageData.Hen_Title; } }

        public string HouseText { get { return LanguageData.House_Title; } }

        public string EmployeeText { get { return LanguageData.Employee_Title; } }

        public string ConsumableText { get { return LanguageData.Consumable_Title; } }

        public string AdditionalCostText { get { return LanguageData.AdditionalCost_Title; } }
        
        #endregion

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

        public bool IsConsumableInput
        {
            get { return _isConsumableInput; }
            set
            {
                _isConsumableInput = value;
                if (value) ResetSelection("Consumable");
                OnPropertyChanged("IsConsumableInput");
            }
        }

        public bool IsAdditionalCostInput
        {
            get { return _isAdditionalCostInput; }
            set {
                _isAdditionalCostInput = value;
                if (value)
                    ResetSelection("AdditionalCost");
                OnPropertyChanged("IsAdditionalCostInput");
            }
        }

        void ResetSelection(string mode)
        {
            switch (mode)
            {
                case "Hen":
                    IsEmployeeInput = false;
                    IsHouseInput = false;
                    IsConsumableInput = false;
                    IsAdditionalCostInput = false;
                    break;

                case "House":
                    IsHenInput = false;
                    IsEmployeeInput = false;
                    IsConsumableInput = false;
                    IsAdditionalCostInput = false;
                    break;

                case "Employee":
                    IsHenInput = false;
                    IsHouseInput = false;
                    IsConsumableInput = false;
                    IsAdditionalCostInput = false;
                    break;

                case "Consumable":
                    IsHenInput = false;
                    IsHouseInput = false;
                    IsEmployeeInput = false;
                    IsAdditionalCostInput = false;
                    break;

                case "AdditionalCost":
                    IsHenInput = false;
                    IsConsumableInput = false;
                    IsEmployeeInput = false;
                    IsAdditionalCostInput = false;

                    break;
            }
        }

        public ICommand EmployeeListCommand { get; private set; }

        public ICommand HenListCommand { get; private set; }

        public ICommand HouseListCommand { get; private set; }

        public ICommand ConsumableListCommand { get; private set; }

        public ICommand AdditionalCostListCommand { get; private set; }

        void InitializeCommand()
        {
            EmployeeListCommand = new DelegateCommand(OnEmployeeListRefresh,param=>true);

            HenListCommand = new DelegateCommand(OnHenListRefresh, param => true);

            HouseListCommand = new DelegateCommand(OnHouseListRefresh, param => true);

            ConsumableListCommand = new DelegateCommand(OnConsumableListRefresh, param => true);

            AdditionalCostListCommand = new DelegateCommand(OnAdditionalCostListRefresh, param => true);
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
            messageBroker.Subscribe(CommonMessages.ChangeMasterDataView, OnMasterDataViewChange);

            messageBroker.Subscribe(CommonMessages.NewHenView, ShowNewHen);
            messageBroker.Subscribe(CommonMessages.EditHenView,OnHenEditRequest);
            messageBroker.Subscribe(CommonMessages.HenSaved, OnHenListRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteHenSuccess, OnHenListRefresh);

            messageBroker.Subscribe(CommonMessages.NewHouseView, ShowNewHouse);
            messageBroker.Subscribe(CommonMessages.EditHouseView, OnHouseEditRequest);
            messageBroker.Subscribe(CommonMessages.SaveHouseSuccess, OnHouseListRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteHouseSuccess, OnHouseListRefresh);

            messageBroker.Subscribe(CommonMessages.NewEmployeeView, ShowNewEmployee);
            messageBroker.Subscribe(CommonMessages.EditEmployeeView, OnEmployeeEditRequest);
            messageBroker.Subscribe(CommonMessages.SaveEmployeeSuccess, OnEmployeeListRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteEmployeeSuccess, OnEmployeeListRefresh);

            messageBroker.Subscribe(CommonMessages.NewConsumableView, ShowNewConsumable);
            messageBroker.Subscribe(CommonMessages.EditConsumableView, OnConsumableEditRequest);
            messageBroker.Subscribe(CommonMessages.SaveConsumableSuccess, OnConsumableListRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteConsumableSuccess, OnConsumableListRefresh);
            
            messageBroker.Subscribe(CommonMessages.NewAdditionalCostView, ShowNewAdditionalCost);
            messageBroker.Subscribe(CommonMessages.EditAdditionalCostView, OnAdditionalCostEditRequest);
            messageBroker.Subscribe(CommonMessages.SaveAdditionalCostSuccess, OnAdditionalCostListRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteAdditionalCostSuccess, OnAdditionalCostListRefresh);
        }

        void UnsetMessageListeners()
        {
            messageBroker.Unsubscribe(CommonMessages.ChangeMasterDataView, OnMasterDataViewChange);
            
            messageBroker.Unsubscribe(CommonMessages.NewHenView, ShowNewHen);
            messageBroker.Unsubscribe(CommonMessages.EditHenView, OnHenEditRequest);
            messageBroker.Unsubscribe(CommonMessages.HenSaved, OnHenListRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteHenSuccess, OnHenListRefresh);

            messageBroker.Unsubscribe(CommonMessages.NewHouseView, ShowNewHouse);
            messageBroker.Unsubscribe(CommonMessages.EditHouseView, OnHouseEditRequest);
            messageBroker.Unsubscribe(CommonMessages.SaveHouseSuccess, OnHouseListRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteHouseSuccess, OnHouseListRefresh);

            messageBroker.Unsubscribe(CommonMessages.NewEmployeeView, ShowNewEmployee);
            messageBroker.Unsubscribe(CommonMessages.EditEmployeeView, OnEmployeeEditRequest);
            messageBroker.Unsubscribe(CommonMessages.SaveEmployeeSuccess, OnEmployeeListRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteEmployeeSuccess, OnEmployeeListRefresh);

            messageBroker.Unsubscribe(CommonMessages.NewConsumableView, ShowNewConsumable);
            messageBroker.Unsubscribe(CommonMessages.EditConsumableView, OnConsumableEditRequest);
            messageBroker.Unsubscribe(CommonMessages.SaveConsumableSuccess, OnConsumableListRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteConsumableSuccess, OnConsumableListRefresh);

            messageBroker.Unsubscribe(CommonMessages.NewAdditionalCostView, ShowNewAdditionalCost);
            messageBroker.Unsubscribe(CommonMessages.EditAdditionalCostView, OnAdditionalCostEditRequest);
            messageBroker.Unsubscribe(CommonMessages.SaveAdditionalCostSuccess, OnAdditionalCostListRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteAdditionalCostSuccess, OnAdditionalCostListRefresh);
        }

        private void OnMasterDataViewChange(object param)
        {
            switch ((MasterDataTypes) param)
            {
                case MasterDataTypes.Hen:
                    OnHenListRefresh(null);
                    break;
                case MasterDataTypes.HenHouse:
                    OnHouseListRefresh(null);
                    break;
                case MasterDataTypes.Employee:
                    OnEmployeeListRefresh(null);
                    break;
                case MasterDataTypes.Consumable:
                    OnConsumableListRefresh(null);
                    break;

                case MasterDataTypes.AdditionalCost:
                    OnAdditionalCostListRefresh(null);
                    break;
            }
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

        void ShowNewHouse(object param)
        {
            var view = houseEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.NewHouseEntry, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnHouseEditRequest(object param)
        {
            var view = houseEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.LoadHouse, param);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnHouseListRefresh(object param)
        {
            IsHouseInput = true;
            var view = houseListProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.RefreshHouseList, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void ShowNewEmployee(object param)
        {
            var view = employeeEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.NewEmployeeEntry, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnEmployeeEditRequest(object param)
        {
            var view = employeeEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.LoadEmployee, param);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnEmployeeListRefresh(object param)
        {
            IsEmployeeInput = true;
            var view = employeeListProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.RefreshEmployeeList, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnConsumableListRefresh(object param)
        {
            IsConsumableInput = true;
            var view = consumableListProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.RefreshConsumableList, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void ShowNewConsumable(object param)
        {
            var view = consumableEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.NewConsumableEntry, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnConsumableEditRequest(object param)
        {
            var view = consumableEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.LoadConsumable, param);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void ShowNewAdditionalCost(object param)
        {
            var view = additionalCostEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.NewAdditionalCostEntry, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnAdditionalCostEditRequest(object param)
        {
            var view = additionalCostEntryProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.LoadAdditionalCost, param);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        void OnAdditionalCostListRefresh(object param)
        {
            IsAdditionalCostInput = true;
            var view = additionalCostListProxy.Value as UserControlBase;
            Content = view;
            messageBroker.Publish(CommonMessages.RefreshAdditionalCostList, null);
            messageBroker.Publish(CommonMessages.ChangeMainActions, view.NavigationCommands);
        }

        public override void Dispose()
        {
            UnsetMessageListeners();
        }
    }
}
