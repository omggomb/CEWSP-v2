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

        Analytics.ValidationRules.CERootPathValidationRule m_ceRootValidationRule;

        string m_sCERoot;
        public string CERoot
        {
            get
            {
                return m_sCERoot;
            }
            set
            {
                Analytics.ReasonList reasons;

                m_sCERoot = value;
                m_ceRootValidationRule.IsValid(value, out reasons);
                DisplayCERootIssues(reasons);
            }
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
        public CreateNewProject()
        {

            m_bProjectCreated = false;
            m_ceRootValidationRule = new Analytics.ValidationRules.CERootPathValidationRule();

            InitializeComponent();

            CreateToolTips();
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
        }
    }
}
