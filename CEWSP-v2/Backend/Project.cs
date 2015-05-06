using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OmgUtils.Logging;
using OmgUtils.ApplicationSettingsManagement;
using System.IO;

namespace CEWSP_v2.Backend
{
    /// <summary>
    /// A single project loaded in CEWSP.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// The folder containing this project's settings.
        /// </summary>
        string ProjectRootFolder { get; set; }

        /// <summary>
        /// The folder that stores all the game assets
        /// </summary>
        string GameFolderName 
        { 
            get; 
            set
            {
               // ProjectSettings.SetValue(Definitions.ProjectSettingsIdentificationNames.CEGame, value);
            }
        }

        /// <summary>
        /// The name of the project
        /// </summary>
        string ProjectName { get; set; }

        /// <summary>
        /// The CE installation location used for this project
        /// </summary>
        string ProjectCERoot { get; set; }

        /// <summary>
        /// Path to the image associated with this project
        /// </summary>
        string ImageDir { get; set; }


        public SettingsManager ProjectSettings { get; private set; }

        public Project()
        {
            ProjectSettings = new SettingsManager(Log.ApplicationLog);
            ResetFactoryDefaults();
        }

        /// <summary>
        /// Creates a new instance of a project. Only does basic error checking on the parameters
        /// </summary>
        /// <param name="name">The name of the project</param>
        /// <param name="ceRoot">CE installtion root used for this project</param>
        /// <param name="ceGame">Relative path to the game folder</param>
        public Project(string name, string ceRoot, string ceGame, string imagePath)
            : this()
        {
            if (!(name == null) && !(ceRoot == null) && !(ceGame == null) && imagePath != null)
            {
                ProjectName = name;
                GameFolderName = ceGame;
                ProjectCERoot = ceRoot;
                ImageDir = imagePath;
            }
        }

        /// <summary>
        /// Loads all the data from the given folder.
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public bool LoadFromFolder(string sPath)
        {
            // TODO: implement Project::LoadFromFolder
            Log.LogInfo("Loading profile from path: \"" + sPath + "\" ...");
            return true;
        }

        /// <summary>
        /// Saves the project's settings to the folder it has been loaded from.
        /// </summary>
        /// <returns></returns>
        public bool SaveToFolder()
        {
            // TODO: implement Project::SaveToFolder
            return SaveToFolder(String.IsNullOrWhiteSpace(ProjectRootFolder) 
                ? Definitions.ConstantDefinitions.RelativeProjectsPath + ProjectName + "\\" 
                : ProjectRootFolder);
        }

        /// <summary>
        /// Saves the project's settings to the given folder.
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public bool SaveToFolder(string sPath)
        {
            Directory.CreateDirectory(sPath);
            return ProjectSettings.SaveSettings(sPath + "\\hello.xml");
        }

        /// <summary>
        /// Reset the project's settings to reasonable defaults.
        /// </summary>
        public void ResetFactoryDefaults()
        {
            // TODO: implement Project::ResetFactoryDefaults
            ProjectSettings.AddSetting(new StringSetting()
                {
                    IdentificationName = Definitions.ProjectSettingsIdentificationNames.ProjectName,
                    HumanReadableName = Properties.SettingsDesc.HumProjName,
                    Description = Properties.SettingsDesc.DescProjName,
                    Value = "New Project"
                });

            ProjectSettings.AddSetting(new StringSetting()
            {
                IdentificationName = Definitions.ProjectSettingsIdentificationNames.CERoot,
                HumanReadableName = Properties.SettingsDesc.HumProjCERoot,
                Description = Properties.SettingsDesc.DescProjCERoot,
                Value = ""
            });

            ProjectSettings.AddSetting(new StringSetting()
            {
                IdentificationName = Definitions.ProjectSettingsIdentificationNames.CEGame,
                HumanReadableName = Properties.SettingsDesc.HumProjGameFolder,
                Description = Properties.SettingsDesc.DescProjGameFolder,
                Value = "Game"
            });
        }
    }
}
