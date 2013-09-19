using System.Configuration;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Autofac;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core.Services;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace EggFarmSystem.Client.Core
{
    public class Bootstrapper : IBootstrapper
    {
        IList<IModule> modules;
        IContainer container;

        public Bootstrapper()
        {
            modules = new List<IModule>();
        }

        public SplashScreen SplashScreen { get; set; }

        public ICollection<Modules.IModule> Modules
        {
            get { return new ReadOnlyCollection<Modules.IModule>(modules); }
        }

        public void Register(Modules.IModule module)
        {
            modules.Add(module);
        }

        public void Start(Application app)
        {
          ContainerBuilder builder = new ContainerBuilder();

          builder.RegisterInstance(this).As<IBootstrapper>();
          builder.RegisterType<ClientContext>().As<IClientContext>().SingleInstance();
          builder.RegisterType<Views.MainWindow>().As<IMainView>().SingleInstance();
          builder.RegisterType<MessageBroker>().As<IMessageBroker>().SingleInstance();
          builder.RegisterModule<CoreCommandsRegistry>();
          builder.RegisterModule(new ServiceClientRegistry()
          {
              IsDirectAccess = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDirectAccess"])
          });

          foreach (var module in modules)
          {
              builder.RegisterModule(module.Registry);
              module.Initialize();
          }

          container = builder.Build();
          RegisterMessageListeners();

          var mainView = container.Resolve<IMainView>();
          mainView.Initialize();
          var mainWindow = (Window)mainView;
          app.MainWindow = mainWindow;
          mainWindow.Show(); 
        }

        void RegisterMessageListeners()
        {
            var broker = container.Resolve<IMessageBroker>();
            
            broker.Subscribe(CommonMessages.ChangeMainView, (param) =>
                {
                    var type = param as Type;
                    if (type == null)
                        return;
                    var context = container.Resolve<IClientContext>();
                    context.MainViewType = type;
                    var view = container.Resolve(type) as UserControlBase;
                    var mainView = container.Resolve<IMainView>();
                    mainView.ChangeView(view);
                });

            broker.Subscribe(CommonMessages.ChangeMainActions, param =>
                {
                    var commands = param as IList<CommandBase>;
                    
                    var mainView = container.Resolve<IMainView>();
                    mainView.ChangeActionCommands(commands);
                });

            broker.Subscribe(CommonMessages.CloseSplashScreen, param =>
                {
                    if(SplashScreen != null)
                        SplashScreen.Close(TimeSpan.FromSeconds(2));
                });
        }

        public IList<System.Windows.Controls.MenuItem> GetMainMenuItems()
        {
            if (container == null)
                return null;

            var items = new List<MenuItem>();

            foreach (var module in modules)
            {
                if (module.AvailableMenus == null) continue;
                foreach (var menu in module.AvailableMenus)
                {
                    var menuItem = new MenuItem();
                    menuItem.Header = menu.Title().ToUpper();
                    if(menu.CommandType != null)
                        menuItem.Command = container.Resolve(menu.CommandType) as ICommand;
                    items.Add(menuItem);
                }
            }

            return items;
        }
    }
}
