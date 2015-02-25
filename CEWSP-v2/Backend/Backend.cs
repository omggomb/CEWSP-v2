using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.IO;

using OmgUtils.Logging;
using OmgUtils.ApplicationSettingsManagement;

using CEWSP_v2.Definitions;


namespace CEWSP_v2.Backend
{
    /// <summary>
    /// Takes care of all the logic
    /// </summary>
    public class Backend
    {
        /// <summary>
        /// Global settings that apply to the whole application
        /// </summary>
        public SettingsManager GlobalSettings { get; private set; }

        public List<string> FoundProjectsNames { get; private set; }

        /// <summary>
        /// Loads the last known profile, profile history, creates a logging instance.
        /// If this fails, the application will not work and should shut down.
        /// </summary>
        /// <returns>True on succes</returns>
        public bool Init()
        {
            

            if (!LoadGlobalSettings())
            {
                Log.LogError("Failed to load global settings!");
                return false;
            }

            Log.LogInfo("Successfully loaded global settings.");

            if (!LoadProjects())
            {
                Log.LogError("Failed to load projects!");
                return false;
            }

            Log.LogInfo("Successfully loaded profiles.");


           
            return true;
        }

        

        /// <summary>
        /// Loads the application level settings
        /// </summary>
        /// <returns>True on success</returns>
        bool LoadGlobalSettings()
        {
            Log.LogInfo("Loading global settings...");
            GlobalSettings = new SettingsManager(Log.ApplicationLog);

            FactoryResetGlobalSettings();

            // First, check if file exists
            // if not
            if (!File.Exists(ConstantDefintitions.RelativeGlobalSettingsPath))
            {
                // Create and save
                string sDirPath = OmgUtils.Path.PathUtils.GetFilePath(ConstantDefintitions.RelativeGlobalSettingsPath);
                Directory.CreateDirectory(sDirPath);
                return GlobalSettings.SaveSettings(ConstantDefintitions.RelativeGlobalSettingsPath);
            }

            // File exists...load
            return GlobalSettings.LoadSettingsFromFile(ConstantDefintitions.RelativeGlobalSettingsPath);
        }

        void FactoryResetGlobalSettings()
        {
            GlobalSettings.AddSetting(new StringSetting()
            {
                IdentificationName = SettingsIdentificationNames.SetLastUsedProject,
                Category = SettingsCategoryNames.GlobalSettingsCategoryProjects,
                Description = Properties.Resources.SetDescLastUsedProject,
                HumanReadableName = Properties.Resources.SetHumLastUsedProject,
                Value = ConstantDefintitions.CommonValueNone
            });
        }

        /// <summary>
        /// Loads all saved projects
        /// </summary>
        /// <returns></returns>
        private bool LoadProjects()
        {
            var dirInf = new DirectoryInfo(ConstantDefintitions.RelativeProjectsPath);

            // TODO : Tell user he has to create a project ... startup page?
            // If dir doesn't exist, a new project has to be created
            if (!dirInf.Exists)
            {
                Log.LogWarning("No projects found, asking user to create one...");
                dirInf = Directory.CreateDirectory(ConstantDefintitions.RelativeProjectsPath);

                return ShowNewProjectDialog();
            }

            var subDirs = dirInf.GetDirectories();

            // If there are no projects, create a new one
            if (subDirs.Length == 0)
            {
                Log.LogWarning("No projects found, asking user to create one...");

                return ShowNewProjectDialog();
            }

            FoundProjectsNames = new List<string>();

            // Now load all the project's names into a list so they can be accessed if needed
            foreach (var dir in subDirs)
            {
                FoundProjectsNames.Add(dir.Name);
                Log.LogInfo("Found project with name: " + dir.Name);
            }

            return true;
        }

        /// <summary>
        /// Displays a modal dialog for creating a new project
        /// </summary>
        /// <returns></returns>
        private bool ShowNewProjectDialog()
        {
            var dia = new Dialogs.CreateNewProject();
            dia.ShowDialog();
            return true;
        }
    }
}
