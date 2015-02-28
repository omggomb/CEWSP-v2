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
using System.Windows.Shapes;

using CEWSP_v2.Backend;
using CEWSP_v2.Definitions;

namespace CEWSP_v2.Dialogs
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        /// <summary>
        /// Used to fill out information in the list view and
        /// to pass it on to the CreateNewProject dialog
        /// </summary>
        Backend.Backend ApplicationBackend { get; set; }

        /// <summary>
        /// Set to true if the exit button was pressed and the application should shut down
        /// </summary>
        bool m_bExitButtonPressed;

        public Welcome(Backend.Backend applicationBackend)
        {
            InitializeComponent();

            m_bExitButtonPressed = false;

            if (applicationBackend == null)
                throw new ArgumentNullException("applicationBackend");

            ApplicationBackend = applicationBackend;

            CreateToolTips();

            FillProjects();
        }

        private void CreateToolTips()
        {
            // exit button
            exitBotton.ToolTip = Utillities.ConstructToolTip(Properties.Welcome.TipExit);

            // Create button
            newProjectButton.ToolTip = Utillities.ConstructToolTip(Properties.Welcome.TipCreate,
                                                                    Properties.Welcome.TipCreateNeedForProject);

            // Checkbox
            showAgainCheckBox.ToolTip = Utillities.ConstructToolTip(Properties.Welcome.TipDontShowCheckBox,
                                                                    Properties.Welcome.TipDontShowCheckBoxNoProjects);
        }

        /// <summary>
        /// Create a list entry for each found project or advice the user to create a new one if none
        /// were found
        /// </summary>
        private void FillProjects()
        {
            if (ApplicationBackend.FoundProjectsNames.Count <= 0)
            {
                // Add message to list box
                var item = new ListBoxItem();

                var textBlock = new TextBlock() { TextWrapping = TextWrapping.Wrap };
                textBlock.Text = Properties.Welcome.MsgNoProjectCreateNew;

                item.Content = textBlock;
                item.IsEnabled = false;

                projectListBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

                projectListBox.Items.Add(item);

                // Disable checkbox since CEWSP needs a project to load
                showAgainCheckBox.IsEnabled = false;
            }
            else
            {
                foreach (var name in ApplicationBackend.FoundProjectsNames)
                {
                    AddProjectToList(name);
                }
            }
        }

        /// <summary>
        /// Adds the project with this name to the list and loads it image.
        /// </summary>
        /// <param name="sProjectName"></param>
        void AddProjectToList(string sProjectName)
        {
            // TODO: AddProjectToList
        }

        /// <summary>
        /// Fired when the exit button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitBotton_Click(object sender, RoutedEventArgs e)
        {
            if (ShouldClose())
            {
                m_bExitButtonPressed = true;
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Need to listen for this, since the user could just press the x-Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // If the exit button was pressed don't ask the user anymore
            if (m_bExitButtonPressed)
                return;

            e.Cancel = !ShouldClose();
        }

        /// <summary>
        /// If no projects were found, informs the user that at least one project needs to be created
        /// in order to work with CEWSP and asks him if he really wants to quit the application.
        /// If projects were found, returns true
        /// </summary>
        /// <returns></returns>
        bool ShouldClose()
        {
            if (ApplicationBackend.FoundProjectsNames.Count > 0)
                return true;

            var res = MessageBox.Show(Properties.Welcome.MsgCloseWelcomeWithoutProjects, Properties.Resources.CommonWarning,
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (res != MessageBoxResult.No)
                return true;

            return false;
        }

        private void showAgainCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (showAgainCheckBox.IsChecked == true)
            {
                ApplicationBackend.GlobalSettings.GetSetting(SettingsIdentificationNames.SetShowWelcomeWindow).SetFromString("false");
            }
            else
            {
                ApplicationBackend.GlobalSettings.GetSetting(SettingsIdentificationNames.SetShowWelcomeWindow).SetFromString("true");
            }
        }

        private void newProjectButton_Click(object sender, RoutedEventArgs e)
        {
            m_bExitButtonPressed = true;

            Close();
            var dia = new CreateNewProject();

            dia.ShowDialog();
        }
    }
}
