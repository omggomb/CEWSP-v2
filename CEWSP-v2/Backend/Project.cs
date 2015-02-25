using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OmgUtils.Logging;
using OmgUtils.ApplicationSettingsManagement;

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

        public SettingsManager ProjectSettings { get; private set; }


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
            return true;
        }

        /// <summary>
        /// Saves the project's settings to the given folder.
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public bool SaveToFolder(string sPath)
        {
            // TODO: implement Project::SaveToFolder(string)
            return true;
        }

        /// <summary>
        /// Reset the project's settings to reasonable defaults.
        /// </summary>
        public void ResetFactoryDefaults()
        {
            // TODO: implement Project::ResetFactoryDefaults
        }

    }
}
