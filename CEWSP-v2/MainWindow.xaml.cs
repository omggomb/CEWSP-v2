using CEWSP_Backend;
using CEWSP_Backend.Backend;
using CEWSP_Backend.Definitions;
using OmgUtils.ApplicationSettingsManagement;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace CEWSP_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public LYProjectOutData ActiveProject { get; private set; }

        public MainWindow()
        {
#if (DEBUG)
            Directory.SetCurrentDirectory(@"..\..\..\WorkingDir\");
#endif
            InitializeComponent();

            InitLog();

            InitBackend();

         

            InitWindow();
        }

        /// <summary>
        /// Initialize the application log
        /// </summary>
        private void InitLog()
        {
            if (!Log.Init(logTextBlock))
            {
                MessageBox.Show(Properties.Resources.MsgFailedToInitLog, Properties.Resources.CommonError,
                                 MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            // Hack because using a rich text box is not reliable as it relies on haivng focus
            // for the ScrollToEnd function to work.
            // Instead we use a ScrollViewer with a TextBlock inside of it, but since
            // the TextBlock has no TextChanged event, we need to raise our own event.
            Log.OnMessageLogged += delegate
            {
                logScrollViewer.ScrollToBottom();
            };

            Log.LogInfo("Log successfully initialized.");
        }

        /// <summary>
        /// Init the application back-end (loads projects, global settings)
        /// </summary>
        private void InitBackend()
        {
            Log.LogInfo("Initializing back-end...");

            if (!ApplicationBackend.Init())
            {
                MessageBox.Show(Properties.Resources.MsgFailedToInitBackend, Properties.Resources.CommonError,
                                 MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            Log.LogInfo("Back-end successfully initialized.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.LogInfo("Application closing down...");
            ApplicationBackend.Shutdown();
        }

        /// <summary>
        /// Determine wich window to show, and load a project or close the application
        /// </summary>
        private void InitWindow()
        {
            LoadProject();
            UpdateUI();
        }

        private void LoadProject()
        {
            ActiveProject = ApplicationBackend.GetActiveProject();
        }

        private void UpdateUI()
        {
            string lyRoot = ActiveProject.LYRoot;
            explorerView1.WatchDir = lyRoot;
            explorerView1.InitializeTree(new CustomControls.ExplorerItemFactory());
        }

        private void OpenCodeSolutionClicked(object sender, RoutedEventArgs e)
        {
            ApplicationBackend.OpenCodeSolution();
        }

        private void OnOpenSetupAssistantClicked(object sender, RoutedEventArgs e)
        {
            ApplicationBackend.OpenSetupAssistant();
        }

        private void OnOpenProjectManagerClicked(object sender, RoutedEventArgs e)
        {
            ApplicationBackend.OpenProjectManager();
        }

        private void OnLmbrWafConfigureClicked(object sender, RoutedEventArgs e)
        {
            ApplicationBackend.RunLmbrWafConfigure();
        }

        private void OnAddNewExplorerTabClicked(object sender, RoutedEventArgs e)
        {
            var treeView = new ExplorerTreeView.ExplorerTreeViewControl();
            var grid = new Grid();

            grid.Children.Add(treeView);

            var item = new TabItem();
            item.Content = grid;

            tabView.Items.Insert(tabView.Items.Count - 1, item);

            treeView.WatchDir = ActiveProject.LYRoot + "\\..";
            treeView.InitializeTree(new CustomControls.ExplorerItemFactory()); 
        }
    }
}