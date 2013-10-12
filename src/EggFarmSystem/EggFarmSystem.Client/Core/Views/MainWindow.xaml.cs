using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shapes;
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
        private HwndSource hwndSource;

        public MainWindow(IBootstrapper bootstrapper, IMessageBroker messageBroker, IClientContext clientContext)
        {
            InitializeComponent();
           
            
            this.bootstrapper = bootstrapper;
            this.messageBroker = messageBroker;
            this.clientContext = clientContext;

            this.Loaded += MainWindow_Loaded;
            this.PreviewMouseMove += MainWindow_PreviewMouseMove;
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

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            messageBroker.Publish(CommonMessages.CloseSplashScreen, null);
        }


        //thanks for http://blog.magnusmontin.net/2013/03/16/how-to-create-a-custom-window-in-wpf/ for drag and resize handling
        public override void OnApplyTemplate()
        {
            var moveHandler = GetTemplateChild("moveHandler") as Rectangle;
            moveHandler.PreviewMouseDown += moveHandler_PreviewMouseDown;

            Grid resizeGrid = GetTemplateChild("resizeGrid") as Grid;
            if (resizeGrid != null)
            {
                foreach (UIElement element in resizeGrid.Children)
                {
                    Rectangle resizeRectangle = element as Rectangle;
                    if (resizeRectangle != null)
                    {
                        resizeRectangle.PreviewMouseDown += ResizeRectangle_PreviewMouseDown;
                        resizeRectangle.MouseMove += ResizeRectangle_MouseMove;
                    }
                }
            }

            base.OnApplyTemplate();
        }

        void moveHandler_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        protected void ResizeRectangle_MouseMove(Object sender, MouseEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            switch (rectangle.Name)
            {
                case "top":
                    Cursor = Cursors.SizeNS;
                    break;
                case "bottom":
                    Cursor = Cursors.SizeNS;
                    break;
                case "left":
                    Cursor = Cursors.SizeWE;
                    break;
                case "right":
                    Cursor = Cursors.SizeWE;
                    break;
                case "topLeft":
                    Cursor = Cursors.SizeNWSE;
                    break;
                case "topRight":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "bottomLeft":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "bottomRight":
                    Cursor = Cursors.SizeNWSE;
                    break;
                default:
                    break;
            }
        }

        void MainWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                Cursor = Cursors.Arrow;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        protected void ResizeRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            switch (rectangle.Name)
            {
                case "topResizeRectangle":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "bottomResizeRectangle":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "leftResizeRectangle":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "rightResizeRectangle":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "topLeftResizeRectangle":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "topRightResizeRectangle":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "bottomLeftResizeRectangle":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "bottomRightResizeRectangle":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
                default:
                    break;
            }
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        private enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += OnSourceInitialized;
            base.OnInitialized(e);
        }

        void OnSourceInitialized(object sender, EventArgs e)
        {
            hwndSource = (HwndSource)PresentationSource.FromVisual(this);
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

            //dummy
            mnuMain.Items.Add(new MenuItem { Header = "DASHBOARD" });
            
            HandleMenuItemsEvents();
            (mnuMain.Items[0] as MenuItem).IsChecked = true;
        }

        void HandleMenuItemsEvents()
        {
            foreach (MenuItem item in mnuMain.Items)
            {
                item.Checked += menuItem_Checked;
            }
        }

        void menuItem_Checked(object sender, RoutedEventArgs e)
        {
            var currentItem = sender as MenuItem;

            if (currentItem.IsChecked)
            {
                foreach (MenuItem item in mnuMain.Items)
                {
                    if (item != currentItem)
                        item.IsChecked = false;
                }
            }
        }

        void InitializeContent()
        {
            if (mnuMain.Items.Count > 0)
                (mnuMain.Items.GetItemAt(0) as MenuItem).Command.Execute(null);
        }

        public void ChangeView(UserControlBase newView)
        {
            IDisposable oldView = null;

            if (grdContent.Children.Count > 0)
                oldView = grdContent.Children[0] as IDisposable;

            ChangeActionCommands(newView.NavigationCommands);
            grdContent.Children.Clear();
            grdContent.Children.Add(newView);

            if(oldView != null)
                oldView.Dispose();
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
                    button.Style = FindResource("NavButton") as Style;
                    stButtons.Children.Add(button);
                }
            }
        }
        

    }

    enum ResizeDirection
    {
        Left = 1,
        Right = 2,
        Top = 3,
        TopLeft = 4,
        TopRight = 5,
        Bottom = 6,
        BottomLeft = 7,
        BottomRight = 8,
    }
}
