using CEWSP_Backend;
using CEWSP_Backend.Analytics;
using CEWSP_Backend.Backend;
using CEWSP_v2.Dialogs.AlteredControls;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

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
        private bool m_bProjectCreated;

        private CEWSP_Backend.Analytics.ValidationRules.CEEditorPathValidationRule m_ceEditorPathValidationRule;
        private Popup m_ceGameIssuesPopup;
        private CEWSP_Backend.Analytics.ValidationRules.CEGameValidationRule m_ceGameValidationRule;
        private Popup m_ceRootIssuesPopup;
        private CEWSP_Backend.Analytics.ValidationRules.CERootPathValidationRule m_ceRootValidationRule;
        private Popup m_gameTemplateIssuesPopup;
        private Popup m_projectNameIssuesPopup;
        private CEWSP_Backend.Analytics.ValidationRules.ProjectNameValidationRule m_projectNameValidationRule;
        private string m_sCEEditorPath;
        private string m_sCEGame;
        private string m_sCERoot;
        private string m_sProjectImagePath;
        private string m_sProjectName;

        public CreateNewProject()
        {
            m_bProjectCreated = false;
            m_ceRootValidationRule = new CEWSP_Backend.Analytics.ValidationRules.CERootPathValidationRule();
            m_ceGameValidationRule = new CEWSP_Backend.Analytics.ValidationRules.CEGameValidationRule();
            m_projectNameValidationRule = new CEWSP_Backend.Analytics.ValidationRules.ProjectNameValidationRule();
            m_ceEditorPathValidationRule = new CEWSP_Backend.Analytics.ValidationRules.CEEditorPathValidationRule();
            CEVersion = new CEVersionInfo();
            InitializeComponent();

            CreateToolTips();

            CEEditorPath = @"E:\Dev\CRYENGINE\358\Bin64\Editor.exe";
            CEGame = "";
            ProjectName = "";

            ResetProjectImage();
            ResetTemplateList();
        }

        public string CEEditorPath
        {
            get
            {
                return m_sCEEditorPath;
            }
            set
            {
                m_sCEEditorPath = value;

                ReasonList reasons = null;

                bool bValid = m_ceEditorPathValidationRule.IsValid(m_sCEEditorPath, ref reasons);

                DisplayCERootIssues(reasons);

                ceRootTextBox.Text = value;

                if (bValid)
                {
                    CEVersion.FromFile(m_sCEEditorPath);

                    CERoot = Utillities.GetCERootFromEditor(m_sCEEditorPath).FullName;
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
                ReasonList reasons = null;
                m_sCEGame = value;
                m_ceGameValidationRule.IsValid(m_sCEGame, ref reasons, m_sCERoot, CEVersion);
                DisplayCEGameIssues(reasons);
                ceGameTextBox.Text = m_sCEGame;

                if (gameTemplateComboBox.SelectedIndex >= 0)
                {
                    var item = gameTemplateComboBox.SelectedItem as GameTemplateComboItem;
                    CheckSaveTemplateInstallation(item.AssociatedGameTemplate.Name == CEWSP_Backend.Properties.CreateNewProject.DefaultTemplateName);
                }
            }
        }

        public string CERoot
        {
            get
            {
                return m_sCERoot;
            }
            set
            {
                ReasonList reasons = null;

                m_sCERoot = value;
                m_ceRootValidationRule.IsValid(m_sCERoot, ref reasons, CEVersion);

                // If the root folder is changed the game folder may need to change, too
                if (m_sCEGame != null)
                {
                    m_ceGameValidationRule.IsValid(m_sCEGame, ref reasons, m_sCERoot);
                    DisplayCEGameIssues(reasons);
                }
            }
        }

        public CEVersionInfo CEVersion { get; set; }
        public ProjectOutData CreatedProject { get; private set; }

        public string ProjectImagePath
        {
            get
            {
                return m_sProjectImagePath;
            }
            set
            {
                if (File.Exists(value))
                    m_sProjectImagePath = value;

                UpdateProjectImageDisplay();
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
                ReasonList list = null;
                m_sProjectName = value;
                m_projectNameValidationRule.IsValid(m_sProjectName, ref list);
                DisplayProjectNameIssues(list);
                projectNameTextBox.Text = m_sProjectName;
            }
        }

        /// <summary>
        /// Shows the dialog and returns the name of the created project
        /// or ConstantDefinitions.CommonValueNone if cancel was pressed
        /// </summary>
        /// <returns></returns>
        public static ProjectOutData ShowAndReturn()
        {
            var dia = new CreateNewProject();
            dia.ShowDialog();

            return dia.GetProjectInfomation();
        }

        private void browseCERootButton_Click(object sender, RoutedEventArgs e)
        {
            var dia = new OpenFileDialog()
            {
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "SandBox|Editor.exe"
            };
            dia.FileOk += dia_FileOk;

            dia.ShowDialog();
        }

        private void browseImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dia = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                DereferenceLinks = true,
                ValidateNames = true,
                Filter = Properties.Resources.CommonImageFiles + "|*bmp;*jpg;*png;*tiff;*ico;*gif;*wdp"
            };

            dia.FileOk += delegate
            {
                ProjectImagePath = dia.FileName;
            };

            dia.ShowDialog();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void ceRootIssuesImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_ceRootIssuesPopup != null)
            {
                m_ceRootIssuesPopup.IsOpen = true;
                m_ceRootIssuesPopup.StaysOpen = true;
                m_ceRootIssuesPopup.PlacementTarget = ceRootIssuesImage;
            }
        }

        private void CheckSaveTemplateInstallation(bool bIsDefaultTemplate)
        {
            if (CEGame == null || CEGame == "")
            {
                var reasons = new ReasonList();
                reasons.Add(new Reason()
                {
                    HumanReadableExplanation = CEWSP_Backend.Properties.ValidationReasons.CEGameNameIsEmpty,
                    ReasonType = EReason.eR_CheckGameNameEmpty,
                    Severity = EReasonSeverity.eRS_error
                });

                DisplayTemplateIssues(reasons);
            }
            else if (!bIsDefaultTemplate)
            {
                var dir = new DirectoryInfo(CERoot + "\\" + CEGame);

                if (dir.Exists)
                {
                    var reasonList = new ReasonList();
                    reasonList.Add(new Reason()
                    {
                        HumanReadableExplanation = CEWSP_Backend.Properties.ValidationReasons.TemplateMightOverride,
                        Severity = EReasonSeverity.eRS_warning,
                        ReasonType = EReason.eR_GameNotEmptyTemplateOverride
                    });

                    DisplayTemplateIssues(reasonList);
                }
            }
            else
                DisplayTemplateIssues(null);
        }

        private void clearCEGameButton_Click(object sender, RoutedEventArgs e)
        {
            CEGame = "";
        }

        private void clearCERootButton_Click(object sender, RoutedEventArgs e)
        {
            CERoot = "";
        }

        private void clearProjectNameButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectName = "";
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_projectNameValidationRule.IsValid(m_sProjectName) &&
                m_ceRootValidationRule.IsValid(m_sCERoot, CEVersion) &&
                m_ceGameValidationRule.IsValid(m_sCEGame, m_sCERoot, CEVersion))
            {
                CreateProjectFromInputs();
                Close();
            }
            else
            {
                MessageBox.Show(CEWSP_Backend.Properties.CreateNewProject.NoCreateSolveIssues,
                                Properties.Resources.CommonError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateProjectFromInputs()
        {
            var projectData = new ProjectInData()
            {
                ProjectName = ProjectName,
                ProjectCERoot = CERoot,
                ProjectGameRoot = CERoot + "\\" + CEGame,
                ProjectImagePath = ProjectImagePath,
                ProjectCEEditorPath = CEEditorPath
            };
            CreatedProject = ApplicationBackend.CreateNewProject(projectData);

            m_bProjectCreated = true;
        }

        private void createProjectWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void createProjectWindow_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (m_ceRootIssuesPopup != null && !m_ceRootIssuesPopup.IsMouseOver)
                m_ceRootIssuesPopup.IsOpen = false;

            if (m_ceGameIssuesPopup != null && !m_ceGameIssuesPopup.IsMouseOver)
                m_ceGameIssuesPopup.IsOpen = false;

            if (m_projectNameIssuesPopup != null && !m_projectNameIssuesPopup.IsMouseOver)
                m_projectNameIssuesPopup.IsOpen = false;

            if (m_gameTemplateIssuesPopup != null && !m_gameTemplateIssuesPopup.IsMouseOver)
                m_gameTemplateIssuesPopup.IsOpen = false;
        }

        private void CreateToolTips()
        {
            browseImageButton.ToolTip = Utillities.ConstructToolTip(CEWSP_Backend.Properties.CreateNewProject.TipImage,
                                                              CEWSP_Backend.Properties.CreateNewProject.TipImageClickToBrowse,
                                                              CEWSP_Backend.Properties.CreateNewProject.TipImageRightReset);
        }

        private void dia_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CERoot = (sender as OpenFileDialog).FileName;
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

        private void DisplayTemplateIssues(ReasonList reasonList)
        {
            if (reasonList != null && reasonList.Count > 0)
            {
                if (reasonList.ContainsError)
                    gameTemplateIssuesImage.Source = Utillities.ErrorBitmap;
                else
                    gameTemplateIssuesImage.Source = Utillities.WarningBitmap;

                m_gameTemplateIssuesPopup = Utillities.CreateIssueToolTip(reasonList);
                m_gameTemplateIssuesPopup.PlacementTarget = gameTemplateIssuesImage;
            }
            else
            {
                gameTemplateIssuesImage.Source = Utillities.CheckmarkBitmap;
                m_gameTemplateIssuesPopup = null;
            }
        }

        private void gameTemplateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var item = e.AddedItems[0] as GameTemplateComboItem;
                string sAbsPath = item.AssociatedGameTemplate.Directory.FullName + "\\" + item.AssociatedGameTemplate.DescFilePath;
                gameTemplateDescWebBrowser.Source = new Uri(sAbsPath);

                CheckSaveTemplateInstallation(item.AssociatedGameTemplate.Name == CEWSP_Backend.Properties.CreateNewProject.DefaultTemplateName);
            }
        }

        private void gameTemplateIssuesImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_gameTemplateIssuesPopup != null)
            {
                m_gameTemplateIssuesPopup.IsOpen = true;
                m_gameTemplateIssuesPopup.StaysOpen = true;
                m_gameTemplateIssuesPopup.PlacementTarget = gameTemplateIssuesImage;
            }
        }

        private string GetCERootFromEditorExe(string sPathToEditorExe)
        {
            var inf = new FileInfo(sPathToEditorExe);

            if (inf.Exists)
            {
                return inf.Directory.Parent.FullName;
            }
            else
            {
                return sPathToEditorExe;
            }
        }

        private ProjectOutData GetProjectInfomation()
        {
            return CreatedProject;
        }

        [Obsolete("Use GetProjectInformation instead")]
        private string GetProjectName()
        {
            if (m_bProjectCreated)
                return ProjectName;
            else
                return CEWSP_Backend.Definitions.ConstantDefinitions.CommonValueNone;
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

        private void resetImageContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResetProjectImage();
        }

        private void ResetProjectImage()
        {
            m_sProjectImagePath = "/Images/default-project-image.png";
            projectImage.Source = Utillities.DefaultProjectBitmap;
        }

        private void ResetTemplateList()
        {
            foreach (var template in ApplicationBackend.FoundGameTemplates)
            {
                var item = new GameTemplateComboItem() { Content = template.Name, AssociatedGameTemplate = template };
                item.ConstructHeader();
                gameTemplateComboBox.Items.Add(item);
            }

            if (ApplicationBackend.FoundGameTemplates.Count > 0)
                gameTemplateComboBox.SelectedIndex = 0;
        }

        private void UpdateProjectImageDisplay()
        {
            var bit = new BitmapImage(new Uri(m_sProjectImagePath));
            projectImage.Source = bit;
        }
    }
}