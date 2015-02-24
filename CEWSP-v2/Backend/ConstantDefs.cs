using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Definitions
{
    /// <summary>
    /// This class only defines constants of all sorts
    /// </summary>
    public class ConstantDefintitions
    {
        /// <summary>
        /// Relative path to the application log file
        /// </summary>
        public const string RelativeLogFilePath = ".\\CEWSP-Log.txt";

        /// <summary>
        /// Relative path to the global settings file.
        /// Global settings apply on application level.
        /// </summary>
        public const string RelativeGlobalSettingsPath = ".\\Settings\\global.xml";

       

        /// <summary>
        /// String value if no value is defined
        /// </summary>
        public const string CommonValueNone = "!none";
    }

    /// <summary>
    /// Defines all categories in which settings can be
    /// </summary>
    public class SettingsCategoryNames
    {
        /// <summary>
        /// Name of category "Projects" inside the global settings
        /// </summary>
        public const string GlobalSettingsCategoryProjects = "Projects";
    }

    /// <summary>
    /// Defines all the identification names for settings
    /// </summary>
    public class SettingsIdentificationNames
    {
        
        /// <summary>
        /// Project that was last active before application was closed
        /// </summary>
        public const string SetLastUsedProject = "lastUsedProject";
    }
}
