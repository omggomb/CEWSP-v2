using CEWSP_Backend.Definitions;
using Microsoft.Win32;
using OmgUtils.ApplicationSettingsManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CEWSP_Backend.Backend
{
    /// <summary>
    /// Takes care of all the logic
    /// </summary>
    public static class ApplicationBackend
    {
        static ApplicationBackend()
        {
            FoundProjectsNames = new List<string>();
            FoundGameTemplates = new List<GameTemplate>();
            m_snyCtx = SynchronizationContext.Current;
        }

        public static List<GameTemplate> FoundGameTemplates { get; private set; }

        public static List<string> FoundProjectsNames { get; private set; }

        private static SynchronizationContext m_snyCtx;

        /// <summary>
        /// Global settings that apply to the whole application
        /// </summary>
        public static SettingsManager GlobalSettings { get; private set; }

        internal static LYProject ActiveProject { get; private set; }

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

                var engRoot = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Amazon\\Lumberyard\\Settings", "ENG_RootPath", null);

                // Check if that directory actually exists since LY doesn't seem to adjust registry when installing a new minor version
                if (engRoot != null && Directory.Exists(engRoot))
                {
                    GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).SetFromString(engRoot);
                    Log.LogInfo("Found LY installation at: " + engRoot);
                }
                else
                {
                    throw new NotImplementedException("Ask user to manually specify path");
                }
        
            LoadVisualSuffix();

            LoadProjects();

            //LoadGameTemplates();

            return true;
        }

        /// <summary>
        /// Queries lmbr.exe for the visual studio version used. If each of it is true, the latest one will be used.
        /// </summary>
        private static void LoadVisualSuffix()
        {
            // TODO: Check what happens if the user didn't check to compile anything
            Log.LogInfo("Querying used Visual Studio version...");
            var result = StartLmbrExe(ConstantDefinitions.LmbrExeCheckVS120Cmd);

            if (result.Contains("YES"))
            {
                GlobalSettings.GetSetting(SettingsIdentificationNames.SetVisualSuffix).SetFromString(ConstantDefinitions.VisualSuffixVS120);
                Log.LogInfo("Engine uses VS2013");
            }

            result = StartLmbrExe(ConstantDefinitions.LmbrExeCheckVS140Cmd);

            if (result.Contains("YES"))
            {
                GlobalSettings.GetSetting(SettingsIdentificationNames.SetVisualSuffix).SetFromString(ConstantDefinitions.VisualSuffixVS140);
                Log.LogInfo("Engine uses VS2015");
            }
        }

        public static void RunLmbrWafConfigure()
        {
            string lyRoot = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();
            lyRoot += "\\" + ConstantDefinitions.LmbWafRelativePath;

            string command = ConstantDefinitions.LmbrWafConfigureCommand;

            var p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                FileName = lyRoot,
                Arguments = command,
            };

            p.EnableRaisingEvents = true;

            p.Exited += delegate
            {
                Log.ApplicationLog.TextBlock.Dispatcher.Invoke(new ThreadStart(() =>
                {

                    Log.LogInfo("Lmbr_waf configure exited! (Exitcode: " + p.ExitCode + ")");
                }));              

            };

            //Utillities.HoldWorkingDirAndSetToLYRoot();
            ProcessUtils.Start(p);
            //Utillities.RestoreWorkingDir();
        }

        public static void OpenProjectManager()
        {
            string managerPath = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();
            managerPath += "\\" + ConstantDefinitions.ProjectManagerRelativePath;

            Log.LogInfo("Launching Project Manager at: " + managerPath);
            Utillities.StartProcessFromLYRootNonBlocking(managerPath);
        }

        public static void OpenSetupAssistant()
        {
            string assistantPath = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();
            assistantPath += "\\" + ConstantDefinitions.LYSetupAssistantRelativePath;

            Log.LogInfo("Starting Setup assistant at: " + assistantPath);
            var proc = Utillities.StartProcessFromLYRootNonBlocking(assistantPath);

            proc.EnableRaisingEvents = true;

            proc.Exited += delegate
            {
                Log.ApplicationLog.TextBlock.Dispatcher.BeginInvoke(new ThreadStart(LoadVisualSuffix));
            };
        }


        /// <summary>
        /// Gets the currently active project from lmbr.exe and sets it as ActiveProject in the backend
        /// </summary>
        /// <returns></returns>
        public static void LoadProjects()
        {
            FoundProjectsNames.Clear();

            Log.LogInfo("Loading project...");

            var engRoot = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();

            var toolPath = engRoot + '\\' + ConstantDefinitions.LmbrExeRelativePath;

            var cmdLine = ConstantDefinitions.LmbrExeGetActiveProjectCmd;

            var result = Utillities.StartProcessNoWindowFromLYRoot(toolPath, cmdLine);

            if (result.Length > 0)
            {
                var projectName = result;

                projectName = Utillities.TrimLmbrExeOutput(projectName)[0];

                Log.LogInfo("Successfully got active project: " + projectName);

                ActiveProject = new LYProject(projectName);
            }
            else
            {
                Log.LogError("Couldn't get currently active project!");
                throw new NotImplementedException("Ask user run LY at least once?");
            }

        }

        /// <summary>
        /// Shuts down any backend specific services and save the global settings to file.
        /// </summary>
        public static void Shutdown()
        {
            GlobalSettings.SaveSettings();
        }

        /// <summary>
        /// Starts the 64bit editor using the provided command line.
        /// </summary>
        /// <param name="sCommandLine">Arguments to pass to the process. If empty, will not be saved as last used.
        /// If null, empty will be used.</param>
        public static void Start64bitEditor(string sCommandLine)
        {
            if (sCommandLine == null)
            {
                Log.LogWarning("Tried to start 64bit editor with null commandline");
                sCommandLine = "";
            }


            GlobalSettings.GetSetting(SettingsIdentificationNames.SetLastUsedEd64CommandLine).SetFromString(sCommandLine);

            string visualSuffix = GetVisualSuffix();
            string profileSuffix = GetProfileSuffix();

            string editorPath = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();

            editorPath += "\\" + ".\\Bin64" + visualSuffix + profileSuffix + "\\Editor.exe";


            ProcessUtils.Start(editorPath, sCommandLine);
        }

        private static string GetProfileSuffix()
        {
            // TODO: Implement
            return "";
        }

        private static string GetVisualSuffix()
        {
           return GlobalSettings.GetSetting(SettingsIdentificationNames.SetVisualSuffix).GetValueAsString();
        }

        public static void Start64bitGame(string sCommandLine)
        {
            string exeName = ActiveProject.ProjectName + "Launcher.exe";

            string exePath = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();

            string visualSuffix = GetVisualSuffix();
            string profileSuffix = GetProfileSuffix();
            exePath += ".\\Bin64" + visualSuffix + profileSuffix + "\\" + exeName;

            ProcessUtils.Start(exePath, sCommandLine);
        }

        private static void FactoryResetGlobalSettings()
        {
            // Show welcome window
            GlobalSettings.AddSetting(new BoolSetting()
            {
                IdentificationName = SettingsIdentificationNames.SetShowWelcomeWindow,
                Category = SettingsCategoryNames.GlobalSettingsCategoryStartup,
                Description = Properties.SettingsDesc.DescShowWelcomeWindow,
                HumanReadableName = Properties.SettingsDesc.HumShowWelcomeWindow,
                Value = true
            });

            // LY root
            GlobalSettings.AddSetting(new StringSetting()
                {
                    IdentificationName = SettingsIdentificationNames.SetLYRoot,
                    Category = SettingsCategoryNames.GlobalSettingsCategoryStartup,
                    Description = Properties.SettingsDesc.DescLYRoot,
                    HumanReadableName = Properties.SettingsDesc.HumLYRoot,
                    Value = ConstantDefinitions.CommonValueNone
                });

            #region CommandLineStuff

            // Last used 64bit editor commandline
            GlobalSettings.AddSetting(new StringSetting()
            {
                IdentificationName = SettingsIdentificationNames.SetLastUsedEd64CommandLine,
                Category = SettingsCategoryNames.GlobalSettingsPrograms,
                Description = Properties.SettingsDesc.DescLastUsedEd64CommandLine,
                HumanReadableName = Properties.SettingsDesc.HumLastUsedEd64CommandLine,
                Value = ConstantDefinitions.CommonValueNone,
            });

            #endregion CommandLineStuff

            // Visual suffix
            GlobalSettings.AddSetting(new StringSetting()
            {
                IdentificationName = SettingsIdentificationNames.SetVisualSuffix,
                Category = SettingsCategoryNames.GlobalSettingsPrograms,
                Description = Properties.SettingsDesc.DescVisualSuffix,
                HumanReadableName = Properties.SettingsDesc.HumVisualSuffix,
                Value = ConstantDefinitions.VisualSuffixVS120
            });

            //GlobalSettings.AddSetting(new StringSetting()
            //{
            //    IdentificationName = SettingsIdentificationNames.SetBuildProfile,
            //    Cage
            //})
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

        public static LYProjectOutData GetActiveProject()
        {
            return ActiveProject.GetOutData();
        }

        /// <summary>
        /// Starts lmbr.exe with the given commands and returns stdOut as string
        /// </summary>
        /// <param name="args">Arguments to start lmbr.exe with</param>
        /// <returns></returns>
        private static string StartLmbrExe(string args)
        {
            string processPath = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();
            processPath += "\\" + ConstantDefinitions.LmbrExeRelativePath;

            return Utillities.StartProcessNoWindowFromLYRoot(processPath, args);
        }

        public static void OpenCodeSolution()
        {
            string path = GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();

            path += "\\Solutions\\LumberyardSDK_";
            path += GlobalSettings.GetSetting(SettingsIdentificationNames.SetVisualSuffix).GetValueAsString();
            path += ".sln";

            ProcessUtils.Start(path);
        }
    }
}