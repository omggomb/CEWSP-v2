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

        /// <summary>
        /// Loads the last known profile, profile history, creates a logging instance
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
    }
}
