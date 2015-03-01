using System.Windows;

using CEWSP_v2.Backend;

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

        public CreateNewProject()
        {
           
            m_bProjectCreated = false;

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
    }
}
