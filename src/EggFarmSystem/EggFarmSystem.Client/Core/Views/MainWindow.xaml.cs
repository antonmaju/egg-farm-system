using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EggFarmSystem.Client.Commands;

namespace EggFarmSystem.Client.Core.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        private IBootstrapper bootstrapper;
        private IMessageBroker messageBroker;
        private IClientContext clientContext;

        public MainWindow(IBootstrapper bootstrapper, IMessageBroker messageBroker, IClientContext clientContext)
        {
            InitializeComponent();
            this.bootstrapper = bootstrapper;
            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        #region window event

        private void MinimizeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void MaximizeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            this.WindowState = this.WindowState ==  System.Windows.WindowState.Maximized ?
                System.Windows.WindowState.Normal : System.Windows.WindowState.Maximized;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();            
        }

        private void MinimizeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MaximizeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        public void Initialize()
        {
            InitializeMenu();
            InitializeContent();
        }

        void InitializeMenu()
        {
            mnuMain.Items.Clear();

            var items = bootstrapper.GetMainMenuItems();
            foreach (var item in items)
                mnuMain.Items.Add(item);
        }

        void InitializeContent()
        {
            if(mnuMain.Items.Count > 0)
                (mnuMain.Items.GetItemAt(0) as MenuItem).Command.Execute(null);
        }

        public void ChangeView(UserControlBase newView)
        {
            ChangeActionCommands(newView.NavigationCommands);
            scrContent.Content = newView;
        }

        public void ChangeActionCommands(IList<CommandBase> commands)
        {
            stButtons.Children.Clear();
            if (commands != null)
            {
                foreach (var command in commands)
                {
                    var button = new Button();
                    button.Command = command;
                    button.Content = command.Text();
                    stButtons.Children.Add(button);
                }
            }
        }
    }
}
