using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CEWSP_v2.Backend;
using CEWSP_v2.Definitions;

using OmgUtils.ApplicationSettingsManagement;


namespace CEWSP_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
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
            if (!Backend.Log.Init(logTextBlock))
            {
                MessageBox.Show(Properties.Resources.MsgFailedToInitLog, Properties.Resources.CommonError,
                                 MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }


            // Hack because using a rich text box is not reliable as it relies on haivng focus
            // for the ScrollToEnd function to work.
            // Instead we use a ScrollViewer with a TextBlock inside of it, but since
            // the TextBlock has no TextChanged event, we need to raise our own event.
            Backend.Log.OnMessageLogged += delegate
            {
                logScrollViewer.ScrollToBottom();
            };

            Backend.Log.LogInfo("Log successfully initialized.");
        }

        /// <summary>
        /// Init the application backend (loads projects, global settings)
        /// </summary>
        private void InitBackend()
        {
           

            Backend.Log.LogInfo("Initializing backend...");

            if (!ApplicationBackend.Init())
            {
                MessageBox.Show(Properties.Resources.MsgFailedToInitBackend, Properties.Resources.CommonError,
                                 MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            Backend.Log.LogInfo("Backend successfully initialized.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Backend.Log.LogInfo("Application closing down...");
        }

        /// <summary>
        /// Determine wich window to show, and load a project or close the application
        /// </summary>
        void InitWindow()
        {
            var shouldShowWelcomeWindow = ApplicationBackend.GlobalSettings.GetSetting(SettingsIdentificationNames.SetShowWelcomeWindow) as BoolSetting;

            string sProjectToBeLoaded = ConstantDefinitions.CommonValueNone;

            if (ApplicationBackend.FoundProjectsNames.Count == 0 ||
                shouldShowWelcomeWindow.Value == true)
            {
                sProjectToBeLoaded = Dialogs.Welcome.ShowAndReturn();   
            }

            if (sProjectToBeLoaded != Definitions.ConstantDefinitions.CommonValueNone)
            {
                LoadProject(sProjectToBeLoaded);
            }
        }

        private void LoadProject(string sProjectToBeLoaded)
        {
            throw new NotImplementedException();
        }
    }
}
