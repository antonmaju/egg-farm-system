using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EggFarmSystem.Client.Core.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        private IBootstrapper bootstrapper;
        private IMessageBroker messageBroker;

        public MainWindow(IBootstrapper bootstrapper, IMessageBroker messageBroker)
        {
            InitializeComponent();
            this.bootstrapper = bootstrapper;
            this.messageBroker = messageBroker;    
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

        public void ChangeView(System.Windows.Controls.UserControl newView)
        {
            scrContent.Content = newView;
        }
    }
}
