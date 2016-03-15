namespace CEWSP_Backend.Definitions
{
    /// <summary>
    /// This class only defines constants of all sorts
    /// </summary>
    public class ConstantDefinitions
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
        /// Path to the folder containing all the saved project folders
        /// </summary>
        public const string RelativeProjectsPath = ".\\Settings\\Projects\\";

        public const string RelativeGameTemplatesPath = "./GameTemplates/";

        /// <summary>
        /// String value if no value is defined
        /// </summary>
        public const string CommonValueNone = "!none";

        /// <summary>
        /// Path to the default image used for projects
        /// </summary>
        public const string DefaultProjectImagePath = "./Images/default-project-image.png";
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

        /// <summary>
        /// Name of category "Startup" inside the global settings
        /// </summary>
        public const string GlobalSettingsCategoryStartup = "Startup";
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

        /// <summary>
        /// Should the welcome window be shown on startup? If false,
        /// last used project will be loaded at startup.
        /// </summary>
        public const string SetShowWelcomeWindow = "showWelcomeWindow";
    }

    /// <summary>
    /// All names used for settings inside a project
    /// </summary>
    public class ProjectSettingsIdentificationNames
    {
        /// <summary>
        /// Root installation directory of CE for this project
        /// </summary>
        public const string CERoot = "ceRoot";

        /// <summary>
        /// Relative path to the game folder used for this project
        /// </summary>
        public const string CEGame = "ceGameFolder";

        /// <summary>
        /// The name of the project
        /// </summary>
        public const string ProjectName = "projectName";

        public const string ProjectImagePath = "projectImagePath";

        public const string CEEditorRoot = "ceEditorRoot";
    }
}