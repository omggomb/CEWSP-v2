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
using CEWSP_Backend.Definitions;
using CEWSP_Backend;

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

            if (sProjectToBeLoaded != CEWSP_Backend.Definitions.ConstantDefinitions.CommonValueNone)
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
