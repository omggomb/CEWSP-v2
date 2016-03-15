using CEWSP_Backend.Definitions;
using OmgUtils.ApplicationSettingsManagement;
using System;
using System.IO;

namespace CEWSP_Backend.Backend
{
    /// <summary>
    /// Encapsulates data about a project that should be created
    /// </summary>
    public struct ProjectInData
    {
        /// <summary>
        /// Path to the sandbox editor
        /// </summary>
        public string ProjectCEEditorPath { get; set; }

        /// <summary>
        /// Path to the installation directory of this project
        /// </summary>
        public string ProjectCERoot { get; set; }

        /// <summary>
        /// Path to the game folder for this project
        /// </summary>
        public string ProjectGameRoot { get; set; }

        /// <summary>
        /// Path to the thumbnail image of this project
        /// </summary>
        public string ProjectImagePath { get; set; }

        /// <summary>
        /// Name of the project to be created
        /// </summary>
        public string ProjectName { get; set; }
    }

    /// <summary>
    /// Encapsulates information about a created project
    /// </summary>
    public struct ProjectOutData
    {
        /// <summary>
        /// The name of the project
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// The installation directory of this project's CE
        /// </summary>
        public string CERootDir { get; set; }

        /// <summary>
        /// Is this the EaaS version?
        /// </summary>
        public bool IsEaaS { get; set; }

        /// <summary>
        /// Version of CE used in this project as string
        /// </summary>
        public string CEVersion { get; set; }

        /// <summary>
        /// Path to the thumbnail image for this project
        /// </summary>
        public string ImageDir { get; set; }
    }

    /// <summary>
    /// A single project loaded in CEWSP.
    /// </summary>
    internal class Project
    {
        #region Properties

        /// <summary>
        /// The folder containing this project's settings.
        /// </summary>
        private string ProjectRootFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Version of CE used in this project
        /// </summary>
        private CEVersionInfo CEVersion { get; set; }

        /// <summary>
        /// The folder that stores all the game assets
        /// </summary>
        private string GameFolderName
        {
            get
            {
                return ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.CEGame).GetValueAsString();
            }
            set
            {
                ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.CEGame).SetFromString(value);
            }
        }

        /// <summary>
        /// The name of the project
        /// </summary>
        private string ProjectName
        {
            get
            {
                return ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.ProjectName).GetValueAsString();
            }
            set
            {
                ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.ProjectName).SetFromString(value);
            }
        }

        /// <summary>
        /// The CE installation location used for this project
        /// </summary>
        private string ProjectCERoot
        {
            get
            {
                return ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.CERoot).GetValueAsString();
            }
            set
            {
                ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.CERoot).SetFromString(value);
            }
        }

        /// <summary>
        /// Path to the Sandbox editor
        /// </summary>
        private string ProjectEditorPath
        {
            get
            {
                return (string)ProjectSettings[ProjectSettingsIdentificationNames.CEEditorRoot];
            }
            set
            {
                CEVersion.FromFile(value);
                ProjectSettings[ProjectSettingsIdentificationNames.CEEditorRoot].SetFromString(value);
            }
        }

        /// <summary>
        /// Path to the image associated with this project
        /// </summary>
        private string ImageDir
        {
            get
            {
                return ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.ProjectImagePath).GetValueAsString();
            }
            set
            {
                ProjectSettings.GetSetting(ProjectSettingsIdentificationNames.ProjectImagePath).SetFromString(value);
            }
        }

        public SettingsManager ProjectSettings { get; private set; }

        #endregion Properties

        public Project()
        {
            ProjectSettings = new SettingsManager(Log.ApplicationLog);
            ResetFactoryDefaults();
            CEVersion = new CEVersionInfo();
        }

        /// <summary>
        /// Creates a new instance of a project. Only does basic error checking on the parameters
        /// </summary>
        /// <param name="name">The name of the project</param>
        /// <param name="ceRoot">CE installation root used for this project</param>
        /// <param name="ceGame">Relative path to the game folder</param>
        [Obsolete("Use the ProjectInData struct constructor instead", true)]
        public Project(string name, string ceRoot, string ceGame, string imagePath, string ceEditorRoot)
            : this()
        {
            if (!(name == null) && !(ceRoot == null) && !(ceGame == null) && imagePath != null && ceEditorRoot != null)
            {
                ProjectName = name;
                GameFolderName = ceGame;
                ProjectCERoot = ceRoot;
                ImageDir = imagePath;
                ProjectEditorPath = ceEditorRoot;
            }
        }

        /// <summary>
        /// Constructs a new project object. Does no error checking
        /// </summary>
        /// <param name="data">The data from which to construct </param>
        public Project(ProjectInData data) : this()
        {
            ProjectName = data.ProjectName;
            GameFolderName = data.ProjectGameRoot;
            ProjectCERoot = data.ProjectCERoot;
            ImageDir = data.ProjectImagePath;
            ProjectEditorPath = data.ProjectCEEditorPath;
        }

        /// <summary>
        /// Gets information about this poject
        /// </summary>
        /// <returns></returns>
        public ProjectOutData GetProjectInformation()
        {
            return new ProjectOutData
            {
                CERootDir = ProjectCERoot,
                CEVersion = CEVersion.ToString(),
                ImageDir = ImageDir,
                IsEaaS = CEVersion.IsEaaS,
                ProjectName = ProjectName,
            };
        }

        /// <summary>
        /// Loads all the data from the given folder.
        /// </summary>
        /// <param name="sPath">Absolute path to the folder containing the settings.xml</param>
        /// <returns>True on success</returns>
        public bool LoadFromFolder(string sPath)
        {
            // TODO: implement Project::LoadFromFolder
            Log.LogInfo("Loading profile from path: \"" + sPath + "\" ...");

            bool bSuccess = ProjectSettings.LoadSettingsFromFile(sPath + "\\settings.xml");

            if (bSuccess)
            {
                var dirInfo = new DirectoryInfo(sPath);
                ProjectRootFolder = dirInfo.FullName;

                Log.LogInfo("Successfully loaded profile: " + (string)ProjectSettings[ProjectSettingsIdentificationNames.ProjectName]);
            }

            return bSuccess;
        }

        /// <summary>
        /// Saves the project's settings to the folder it has been loaded from.
        /// </summary>
        /// <returns>True on success</returns>
        public bool SaveToFolder()
        {
            // TODO: implement Project::SaveToFolder
            if (String.IsNullOrWhiteSpace(ProjectRootFolder))
            {
                ProjectRootFolder = ConstantDefinitions.RelativeProjectsPath + ProjectName + "\\";
            }
            return SaveToFolder(ProjectRootFolder);
        }

        /// <summary>
        /// Saves the project's settings to the given folder.
        /// </summary>
        /// <param name="sPath">Absolute path to the destination folder</param>
        /// <returns>True on success</returns>
        public bool SaveToFolder(string sPath)
        {
            var dirInfo = Directory.CreateDirectory(sPath);

            if (ProjectRootFolder == null)
            {
                ProjectRootFolder = dirInfo.FullName;
            }
            return ProjectSettings.SaveSettings(sPath + "\\settings.xml");
        }

        /// <summary>
        /// Reset the project's settings to reasonable defaults.
        /// </summary>
        public void ResetFactoryDefaults()
        {
            // TODO: implement Project::ResetFactoryDefaults
            ProjectSettings.AddSetting(new StringSetting()
            {
                IdentificationName = ProjectSettingsIdentificationNames.ProjectName,
                HumanReadableName = Properties.SettingsDesc.HumProjName,
                Description = Properties.SettingsDesc.DescProjName,
                Value = "New Project"
            });

            ProjectSettings.AddSetting(new StringSetting()
            {
                IdentificationName = ProjectSettingsIdentificationNames.CERoot,
                HumanReadableName = Properties.SettingsDesc.HumProjCERoot,
                Description = Properties.SettingsDesc.DescProjCERoot,
                Value = ""
            });

            ProjectSettings.AddSetting(new StringSetting()
            {
                IdentificationName = ProjectSettingsIdentificationNames.CEGame,
                HumanReadableName = Properties.SettingsDesc.HumProjGameFolder,
                Description = Properties.SettingsDesc.DescProjGameFolder,
                Value = "Game"
            });

            ProjectSettings.AddSetting(new StringSetting()
            {
                IdentificationName = ProjectSettingsIdentificationNames.ProjectImagePath,
                HumanReadableName = Properties.SettingsDesc.HumProjImage,
                Description = Properties.SettingsDesc.DescProjImage,
                Value = ConstantDefinitions.DefaultProjectImagePath
            });

            ProjectSettings.AddSetting(new StringSetting()
            {
                IdentificationName = ProjectSettingsIdentificationNames.CEEditorRoot,
                HumanReadableName = Properties.SettingsDesc.HumCEEditorRoot,
                Description = Properties.SettingsDesc.DescCEEditorRoot,
                Value = ConstantDefinitions.CommonValueNone
            });
        }
    }
}