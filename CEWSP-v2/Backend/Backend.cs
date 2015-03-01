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
    public  static class ApplicationBackend
    {
        /// <summary>
        /// Global settings that apply to the whole application
        /// </summary>
        public static SettingsManager GlobalSettings { get; private set; }

        public static List<string> FoundProjectsNames { get; private set; }

        static ApplicationBackend ()
        {
            FoundProjectsNames = new List<string>();
        }

        /// <summary>
        /// Loads the last known profile, profile history, creates a logging instance.
        /// If this fails, the application will not work and should shut down.
        /// </summary>
        /// <returns>True on succes</returns>
        public static bool Init()
        {
            

            if (!LoadGlobalSettings())
            {
                Log.LogError("Failed to load global settings!");
                return false;
            }

            Log.LogInfo("Successfully loaded global settings.");

            // No way to fail, if there are no projects a new one will have to be created
            LoadProjects();

            Log.LogInfo("Successfully loaded profiles.");

            FoundProjectsNames.Add("fakeProject");


           
            return true;
        }

        

        /// <summary>
        /// Loads the application level settings
        /// </summary>
        /// <returns>True on success</returns>
        static bool LoadGlobalSettings()
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

        static void FactoryResetGlobalSettings()
        {
            // Last active project
            GlobalSettings.AddSetting(new StringSetting()
            {
                IdentificationName = SettingsIdentificationNames.SetLastUsedProject,
                Category = SettingsCategoryNames.GlobalSettingsCategoryProjects,
                Description = Properties.SettingsDesc.DescLastUsedProject,
                HumanReadableName = Properties.SettingsDesc.HumLastUsedProject,
                Value = ConstantDefintitions.CommonValueNone
            });

            // Show welcome window
            GlobalSettings.AddSetting(new BoolSetting()
            {
                IdentificationName = SettingsIdentificationNames.SetShowWelcomeWindow,
                Category = SettingsCategoryNames.GlobalSettingsCategoryStartup,
                Description = Properties.SettingsDesc.DescShowWelcomeWindow,
                HumanReadableName = Properties.SettingsDesc.HumShowWelcomeWindow,
                Value = true
            });
        }

        /// <summary>
        /// Loads all saved projects
        /// </summary>
        /// <returns></returns>
        private static void LoadProjects()
        {
            var dirInf = new DirectoryInfo(ConstantDefintitions.RelativeProjectsPath);

            // TODO : Tell user he has to create a project ... startup page?
            // If dir doesn't exist, a new project has to be created
            if (!dirInf.Exists)
            {
                Log.LogWarning("No projects found.");    
                return;
            }

            var subDirs = dirInf.GetDirectories();

            
            if (subDirs.Length == 0)
            {
                Log.LogWarning("No projects found.");

                return;
            }

            // Now load all the project's names into a list so they can be accessed if needed
            foreach (var dir in subDirs)
            {
                FoundProjectsNames.Add(dir.Name);
                Log.LogInfo("Found project with name: " + dir.Name);
            }
        }
    }
}
