﻿using CEWSP_Backend.Analytics;
using CEWSP_Backend.Backend;
using CEWSP_Backend.Definitions;
using CEWSP_Backend.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace CEWSP_Backend
{
    public class Utillities
    {
        private static BitmapImage m_errorBitmap;
        private static BitmapImage m_checkmarkBitmap;
        private static BitmapImage m_warningBitmap;
        private static BitmapImage m_infoBitmap;
        private static BitmapImage m_defaultProjectBitmap;

        private static string m_workingDir;

        public static int ToolTipIconWidth { get; set; }

        static Utillities()
        {
            ToolTipIconWidth = 20;
        }

        public static DirectoryInfo GetCERootFromEditor(string sEditorExePath)
        {
            if (String.IsNullOrWhiteSpace(sEditorExePath))
                throw new ArgumentException("Didn't get valid string for editor executable");

            var info = new FileInfo(sEditorExePath);

            if (info.Exists == false)
            {
                throw new ArgumentException("Got non existing editor path");
            }

            var versionInfo = new CEVersionInfo();
            versionInfo.FromFile(sEditorExePath);

            try
            {
                if (versionInfo.IsEaaS)
                {
                    return info.Directory.Parent.Parent;
                }
                else
                {
                    return info.Directory.Parent;
                }
            }
            catch (NullReferenceException)
            {
                var ex = new InvalidCEEditorPathException("One of the path's directories was null");
                ex.GivenEditoPath = sEditorExePath;
                throw ex;
            }
        }

        public static BitmapImage InfoBitmap
        {
            get
            {
                if (m_infoBitmap == null)
                {
                    m_infoBitmap = new BitmapImage(new Uri("/Images/info-icon-64.png", UriKind.Relative));
                }

                return m_infoBitmap;
            }
        }

        public static BitmapImage ErrorBitmap
        {
            get
            {
                if (m_errorBitmap == null)
                {
                    m_errorBitmap = new BitmapImage(new Uri("/Images/error-icon-64.png", UriKind.Relative));
                }

                return m_errorBitmap;
            }
        }

        public static BitmapImage WarningBitmap
        {
            get
            {
                if (m_warningBitmap == null)
                {
                    m_warningBitmap = new BitmapImage(new Uri("/Images/warning-icon-64.png", UriKind.Relative));
                }

                return m_warningBitmap;
            }
        }

        public static BitmapImage CheckmarkBitmap
        {
            get
            {
                if (m_checkmarkBitmap == null)
                {
                    m_checkmarkBitmap = new BitmapImage(new Uri("/Images/checkmark-icon-64.png", UriKind.Relative));
                }

                return m_checkmarkBitmap;
            }
        }

        public static BitmapImage DefaultProjectBitmap
        {
            get
            {
                if (m_defaultProjectBitmap == null)
                    m_defaultProjectBitmap = new BitmapImage(new Uri("/Images/default-project-image.png", UriKind.Relative));

                return m_defaultProjectBitmap;
            }
        }

        /// <summary>
        /// Constructs a tooltip with the main message at the top.
        /// Each submessage gets its own icon
        /// </summary>
        /// <param name="sMainMessage"></param>
        /// <param name="asSubMessages"></param>
        /// <returns></returns>
        public static ToolTip ConstructToolTip(string sMainMessage, params string[] asSubMessages)
        {
            var tip = new ToolTip();

            var topStack = new StackPanel() { Orientation = Orientation.Vertical };

            var mainMessage = new TextBlock() { Text = sMainMessage };

            topStack.Children.Add(mainMessage);

            if (asSubMessages.Count() > 0)
            {
                mainMessage.FontWeight = System.Windows.FontWeights.Bold;

                foreach (var subMessage in asSubMessages)
                {
                    var subStack = new StackPanel() { Orientation = Orientation.Horizontal };

                    var img = new Image()
                    {
                        Source = InfoBitmap,
                        Width = ToolTipIconWidth,
                        Height = ToolTipIconWidth
                    };

                    subStack.Children.Add(img);
                    subStack.Children.Add(new TextBlock() { Text = subMessage, VerticalAlignment = System.Windows.VerticalAlignment.Center });

                    topStack.Children.Add(subStack);
                }
            }

            tip.Content = topStack;

            return tip;
        }

        internal static Process StartProcessFromLYRootNonBlocking(string path, string arguments = "")
        {
            string currentDir = Directory.GetCurrentDirectory();
            string lyRoot = ApplicationBackend.GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();

            Directory.SetCurrentDirectory(lyRoot);
            var proc = Process.Start(path, arguments);
            Directory.SetCurrentDirectory(currentDir);
            return proc;
        }

        public static Popup CreateIssueToolTip(ReasonList reasons)
        {
            var tip = new Popup();

            tip.Opened += delegate
            {
                tip.MouseLeave += delegate
                {
                    tip.IsOpen = false;
                };
            };

            var scrollViewer = new ScrollViewer() { MaxWidth = 400, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
            tip.Child = scrollViewer;

            var headerIconImage = new Image() { Width = ToolTipIconWidth, Height = ToolTipIconWidth };

            var listView = new ListView();
            scrollViewer.Content = listView;

            var headerWrapPanel = new WrapPanel();
            var headerTextBlock = new TextBlock()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                FontWeight = System.Windows.FontWeights.Bold
            };
            headerTextBlock.FontSize *= 1.2;

            headerWrapPanel.Children.Add(headerIconImage);
            headerWrapPanel.Children.Add(headerTextBlock);

            var headerItem = new ListViewItem();
            headerItem.Content = headerWrapPanel;

            listView.Items.Add(headerItem);

            if (reasons.Count == 0)
            {
                headerIconImage.Source = CheckmarkBitmap;
                headerTextBlock.Text = Properties.ValidationReasons.CommonEverythingGood;
            }
            else
            {
                if (reasons.ContainsError)
                {
                    headerIconImage.Source = ErrorBitmap;
                }
                else
                    headerIconImage.Source = WarningBitmap;

                headerTextBlock.Text = Properties.ValidationReasons.CommonThereAreIssues;

                foreach (var reason in reasons)
                {
                    var reasonExpander = new Expander() { IsExpanded = false };

                    var reasonStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                    var reasonImage = new Image() { Width = ToolTipIconWidth, Height = ToolTipIconWidth };

                    if (reason.Severity == EReasonSeverity.eRS_error)
                        reasonImage.Source = ErrorBitmap;
                    else
                        reasonImage.Source = WarningBitmap;

                    var reasonTextBlock = new TextBlock
                    {
                        Text = reason.HumanReadableExplanation,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center
                    };

                    reasonStackPanel.Children.Add(reasonImage);
                    reasonStackPanel.Children.Add(reasonTextBlock);

                    reasonExpander.Header = reasonStackPanel;

                    if (reason.HumanReadableSolutions.Count > 0)
                    {
                        var reasonSolutions = new ListView();
                        reasonExpander.Content = reasonSolutions;

                        foreach (var item in reason.HumanReadableSolutions)
                        {
                            var solStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                            var solImage = new Image() { Width = ToolTipIconWidth, Height = ToolTipIconWidth, Source = InfoBitmap };
                            var solText = new TextBlock() { Text = item, VerticalAlignment = System.Windows.VerticalAlignment.Center };

                            solStackPanel.Children.Add(solImage);
                            solStackPanel.Children.Add(solText);

                            var solListItem = new ListViewItem() { Content = solStackPanel, IsEnabled = false };
                            reasonSolutions.Items.Add(solListItem);
                        }
                    }

                    var reasonListItem = new ListViewItem() { Content = reasonExpander };
                    listView.Items.Add(reasonListItem);
                }
            }

            return tip;
        }

        /// <summary>
        /// Splits the output of lmbr 
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static List<string> TrimLmbrExeOutput(string sInput)
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(sInput) || string.IsNullOrWhiteSpace(sInput))
            {
                return list;
            }

            var strings = sInput.Split('\n');

            foreach(var element in strings)
            {
                var index = element.IndexOf(ConstantDefinitions.LmbrExeOutputTrimStart);

                if (index != -1)
                {
                    index += ConstantDefinitions.LmbrExeOutputTrimStart.Length;

                    var line = element.Substring(index, element.Length - index).Trim();

                    list.Add(line);
                }
            }

            return list;

        }

        /// <summary>
        /// Starts a process with the LY root directory set as working directory and no window created
        /// </summary>
        /// <param name="path">Path to the process or file</param>
        /// <param name="args">Arguments to be passed to the process</param>
        /// <param name="errorOut">Optional errorout hook</param>
        /// <returns>The standard output as a string</returns>
        public static string StartProcessNoWindowFromLYRoot(string path, string args, out StreamReader errorOut)
        {
            var process = new Process();

            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = path,
                Arguments = args,
                CreateNoWindow = true,
            };

           
            process.StartInfo.RedirectStandardError = true;
            // TODO: Fix me
            errorOut = null;
           

            string currentWorkingDir = Directory.GetCurrentDirectory();

            string lyRoot = ApplicationBackend.GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString();

            Directory.SetCurrentDirectory(lyRoot);

            process.Start();
            process.WaitForExit();

            Directory.SetCurrentDirectory(currentWorkingDir);

            return process.StandardOutput.ReadToEnd();

        }

        /// <summary>
        /// Starts a process with the LY root directory set as working directory and no window created
        /// </summary>
        /// <param name="path">Path to the process or file</param>
        /// <param name="args">Arguments to be passed to the process</param>
        /// <param name="errorOut">Optional errorout hook</param>
        /// <returns>The standard output as a string</returns>
        public static string StartProcessNoWindowFromLYRoot(string path, string args = "")
        {
            StreamReader stdOutDummy;
            return StartProcessNoWindowFromLYRoot(path, args, out stdOutDummy);
        }

        public static void HoldWorkingDirAndSetToLYRoot()
        {
            m_workingDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(ApplicationBackend.GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString());
        }

        public static void RestoreWorkingDir()
        {
            Directory.SetCurrentDirectory(m_workingDir);
        }
    }
}