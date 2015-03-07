using System.Windows;

using System.Windows.Controls;

using CEWSP_v2.Backend;
using CEWSP_v2.Analytics;
using System.Windows.Controls.Primitives;

namespace CEWSP_v2.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewProject.xaml
    /// </summary>
    public partial class CreateNewProject : Window
    {

        /// <summary>
        /// Set to true if this dialog has created a valid project
        /// </summary>
        bool m_bProjectCreated;

        Popup m_ceRootIssuesPopup;
        Popup m_ceGameIssuesPopup;
        Popup m_projectNameIssuesPopup;

        Analytics.ValidationRules.CERootPathValidationRule m_ceRootValidationRule;
        Analytics.ValidationRules.CEGameValidationRule m_ceGameValidationRule;
        Analytics.ValidationRules.PorjectNameValidationRule m_projectNameValidationRule;

        string m_sCERoot;
        string m_sCEGame;
        string m_sProjectName;

        public string CERoot
        {
            get
            {
                return m_sCERoot;
            }
            set
            {
                Analytics.ReasonList reasons = null;

                m_sCERoot = value;
                m_ceRootValidationRule.IsValid(value, ref reasons);
                DisplayCERootIssues(reasons);
                ceRootTextBox.Text = m_sCERoot;

                 // If the root folder is changed the game folder may need to change, too
                if (m_sCEGame != null)
                {
                    m_ceGameValidationRule.IsValid(m_sCEGame, ref reasons, m_sCERoot);
                    DisplayCEGameIssues(reasons);
                }
                
            }
        }

        public string CEGame
        {
            get
            {
                return m_sCEGame;
            }
            set
            {
                Analytics.ReasonList reasons = null;
                m_sCEGame = value;
                m_ceGameValidationRule.IsValid(m_sCEGame, ref reasons, m_sCERoot);
                DisplayCEGameIssues(reasons);
                ceGameTextBox.Text = m_sCEGame;
               
            }
        }

        public string ProjectName
        {
            get
            {
                return m_sProjectName;
            }
            set
            {
                Analytics.ReasonList list = null;
                m_sProjectName = value;
                m_projectNameValidationRule.IsValid(m_sProjectName, ref list);
                DisplayProjectNameIssues(list);
                projectNameTextBox.Text = m_sProjectName;
            }
        }

        


        public CreateNewProject()
        {

            m_bProjectCreated = false;
            m_ceRootValidationRule = new Analytics.ValidationRules.CERootPathValidationRule();
            m_ceGameValidationRule = new Analytics.ValidationRules.CEGameValidationRule();
            m_projectNameValidationRule = new Analytics.ValidationRules.PorjectNameValidationRule();

            InitializeComponent();

            CreateToolTips();

            CERoot = @"E:\Dev\CRYENGINE\358";
            CEGame = "";
            ProjectName = "";
        }

        private void CreateToolTips()
        {
            browseImageButton.ToolTip = Utillities.ConstructToolTip(Properties.CreateNewProject.TipImage,
                                                               Properties.CreateNewProject.TipImageClickToBrowse);
        }

        private void createProjectWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {


        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {

        }

        string GetProjectName()
        {
            if (m_bProjectCreated)
                return "Hello";
            else
                return Definitions.ConstantDefinitions.CommonValueNone;
        }

        /// <summary>
        /// Shows the dialog and returns the name of the created project
        /// or ConstantDefinitions.CommonValueNone if cancel was pressed
        /// </summary>
        /// <returns></returns>
        public static string ShowAndReturn()
        {
            var dia = new CreateNewProject();
            dia.ShowDialog();

            return dia.GetProjectName();

        }

        private void DisplayCERootIssues(ReasonList reasons)
        {
            m_ceRootIssuesPopup = Utillities.CreateIssueToolTip(reasons);
            m_ceRootIssuesPopup.PlacementTarget = ceRootIssuesImage;

            if (reasons.Count == 0)
                ceRootIssuesImage.Source = Utillities.CheckmarkBitmap;
            else
            {
                if (reasons.ContainsError)
                    ceRootIssuesImage.Source = Utillities.ErrorBitmap;
                else
                    ceRootIssuesImage.Source = Utillities.WarningBitmap;
            }
        }

        private void DisplayCEGameIssues(ReasonList reasons)
        {
            m_ceGameIssuesPopup = Utillities.CreateIssueToolTip(reasons);
            m_ceGameIssuesPopup.PlacementTarget = ceGameIssuesImage;

            if (reasons.Count == 0)
                ceGameIssuesImage.Source = Utillities.CheckmarkBitmap;
            else
            {
                if (reasons.ContainsError)
                    ceGameIssuesImage.Source = Utillities.ErrorBitmap;
                else
                    ceGameIssuesImage.Source = Utillities.WarningBitmap;
            }

        }

        private void DisplayProjectNameIssues(ReasonList list)
        {
            m_projectNameIssuesPopup = Utillities.CreateIssueToolTip(list);
            m_projectNameIssuesPopup.PlacementTarget = projectNameIssuesImage;

            if (list.Count == 0)
                projectNameIssuesImage.Source = Utillities.CheckmarkBitmap;
            else
            {
                if (list.ContainsError)
                    projectNameIssuesImage.Source = Utillities.ErrorBitmap;
                else
                    projectNameIssuesImage.Source = Utillities.WarningBitmap;
            }
        }

        private void ceRootIssuesImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_ceRootIssuesPopup != null)
            {
                m_ceRootIssuesPopup.IsOpen = true;
                m_ceRootIssuesPopup.StaysOpen = true;
                m_ceRootIssuesPopup.PlacementTarget = ceRootIssuesImage;
            }
        }

        private void createProjectWindow_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (m_ceRootIssuesPopup != null && !m_ceRootIssuesPopup.IsMouseOver)
                m_ceRootIssuesPopup.IsOpen = false;

            if (m_ceGameIssuesPopup != null && !m_ceGameIssuesPopup.IsMouseOver)
                m_ceGameIssuesPopup.IsOpen = false;

            if (m_projectNameIssuesPopup != null && !m_projectNameIssuesPopup.IsMouseOver)
                m_projectNameIssuesPopup.IsOpen = false;
        }

        private void ceGameIssuesImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_ceGameIssuesPopup != null)
            {
                m_ceGameIssuesPopup.IsOpen = true;
                m_ceGameIssuesPopup.StaysOpen = true;
                m_ceGameIssuesPopup.PlacementTarget = ceGameIssuesImage;
            }
        }

        private void projectNameIssuesImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_projectNameIssuesPopup != null)
            {
                m_projectNameIssuesPopup.IsOpen = true;
                m_projectNameIssuesPopup.StaysOpen = true;
                m_projectNameIssuesPopup.PlacementTarget = projectNameIssuesImage;
            }
        }

        private void clearProjectNameButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectName = "";
        }

        private void clearCEGameButton_Click(object sender, RoutedEventArgs e)
        {
            CEGame = "";
        }

        private void clearCERootButton_Click(object sender, RoutedEventArgs e)
        {
            CERoot = "";
        }
    }
}
