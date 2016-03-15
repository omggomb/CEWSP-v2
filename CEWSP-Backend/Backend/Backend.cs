using CEWSP_Backend.Definitions;
using OmgUtils.ApplicationSettingsManagement;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEWSP_Backend.Backend
{
    /// <summary>
    /// Takes care of all the logic
    /// </summary>
    public static class ApplicationBackend
    {
        /// <summary>
        /// Global settings that apply to the whole application
        /// </summary>
        public static SettingsManager GlobalSettings { get; private set; }

        public static List<string> FoundProjectsNames { get; private set; }

        public static List<GameTemplate> FoundGameTemplates { get; private set; }
        internal static Project ActiveProject { get; private set; }

        static ApplicationBackend()
        {
            FoundProjectsNames = new List<string>();
            FoundGameTemplates = new List<GameTemplate>();
        }

        /// <summary>
        /// Loads the last known profile, profile history, creates a logging instance.
        /// If this fails, the application will not work and should shut down.
        /// </summary>
        /// <returns>True on success</returns>
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

            LoadGameTemplates();

            return true;
        }

        /// <summary>
        /// Loads the application level settings
        /// </summary>
        /// <returns>True on success</returns>
        private static bool LoadGlobalSettings()
        {
            Log.LogInfo("Loading global settings...");
            GlobalSettings = new SettingsManager(Log.ApplicationLog);

            FactoryResetGlobalSettings();

            // First, check if file exists
            // if not
            if (!File.Exists(ConstantDefinitions.RelativeGlobalSettingsPath))
            {
                // Create and save
                string sDirPath = OmgUtils.Path.PathUtils.GetFilePath(ConstantDefinitions.RelativeGlobalSettingsPath);
                Directory.CreateDirectory(sDirPath);
                return GlobalSettings.SaveSettings(ConstantDefinitions.RelativeGlobalSettingsPath);
            }

            // File exists...load
            return GlobalSettings.LoadSettingsFromFile(ConstantDefinitions.RelativeGlobalSettingsPath);
        }

        private static void FactoryResetGlobalSettings()
        {
            // Last active project
            GlobalSettings.AddSetting(new StringSetting()
            {
                IdentificationName = SettingsIdentificationNames.SetLastUsedProject,
                Category = SettingsCategoryNames.GlobalSettingsCategoryProjects,
                Description = Properties.SettingsDesc.DescLastUsedProject,
                HumanReadableName = Properties.SettingsDesc.HumLastUsedProject,
                Value = ConstantDefinitions.CommonValueNone
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
        /// Load the specified project as the active project
        /// </summary>
        /// <param name="sProjectToBeLoaded">Name of the project to be loaded</param>
        public static ProjectOutData LoadProject(string sProjectToBeLoaded)
        {
            ActiveProject = new Project();
            ActiveProject.LoadFromFolder(ConstantDefinitions.RelativeProjectsPath + sProjectToBeLoaded);

            return ActiveProject.GetProjectInformation();
        }

        /// <summary>
        /// Loads all saved projects
        /// </summary>
        /// <returns></returns>
        public static void LoadProjects()
        {
            FoundProjectsNames.Clear();
            var dirInf = new DirectoryInfo(ConstantDefinitions.RelativeProjectsPath);

            Log.LogInfo("Loading projects...");

            // TODO : Tell user he has to create a project ... startup page?
            // If dir doesn't exist, a new project has to be created
            if (!dirInf.Exists || dirInf.GetDirectories().Length == 0)
            {
                Log.LogWarning("No projects found.");
                if (!dirInf.Exists)
                {
                    dirInf.Create();
                }
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

        private static void LoadGameTemplates()
        {
            Log.LogInfo("Loading game templates...");

            var dirInf = new DirectoryInfo(ConstantDefinitions.RelativeGameTemplatesPath);

            if (!dirInf.Exists)
            {
                Log.LogInfo("No game templates found, nothing loaded.");
                return;
            }

            foreach (var dir in dirInf.GetDirectories())
            {
                // none is a template that does not alter any files, but still
                // can display a configuration dialog. Its name is localized thus it
                // must be loaded separately
                if (dir.Name == "none")
                {
                    var defaultTemplate = new GameTemplate();
                    defaultTemplate.LoadFromFolder(dir.FullName);

                    defaultTemplate.Name = Properties.CreateNewProject.DefaultTemplateName;
                    FoundGameTemplates.Add(defaultTemplate);
                    continue;
                }

                var t = new GameTemplate();

                if (t.LoadFromFolder(dir.FullName))
                {
                    FoundGameTemplates.Add(t);
                    Log.LogInfo("Successfully loaded game template '" + t.Name + "'.");
                }
            }
        }

        public static ProjectOutData CreateNewProject(ProjectInData projectData)
        {
            var project = new Project(projectData);

            project.SaveToFolder();

            if (!Directory.Exists(projectData.ProjectGameRoot))
            {
                Directory.CreateDirectory(projectData.ProjectGameRoot);
            }

            return project.GetProjectInformation();
        }
    }
}