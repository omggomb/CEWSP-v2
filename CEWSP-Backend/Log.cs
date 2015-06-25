using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.Windows;
using CEWSP_Backend.Definitions;

namespace CEWSP_Backend
{
    /// <summary>
    /// The logging interface for CEWSP
    /// </summary>
    public class Log
    {
        /// <summary>
        /// An instance of OmgUtils.Logging.Logger
        /// </summary>
        public static OmgUtils.Logging.Logger ApplicationLog { get; private set; }

        
        public delegate void OnMessageLoggedDelegate();

        /// <summary>
        /// Fired whenever a message is logged
        /// </summary>
        public static event OnMessageLoggedDelegate OnMessageLogged;

        /// <summary>
        /// Creates an instance of OmgUtils.Logging.Logger
        /// </summary>
        /// <param name="tb">TextBlock instance for visual logging</param>
        /// <returns>True on success</returns>
        public static bool Init(TextBlock tb)
        {
            ApplicationLog = new OmgUtils.Logging.Logger();

            if (!ApplicationLog.Init(ConstantDefinitions.RelativeLogFilePath, tb))
                return false;

           
            return true;
        }

        /// <summary>
        /// Log a message with the defined severity
        /// See OmgUtils.Logging.Logger for information on severity levels
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sSeverity"></param>
        public static void LogMessage(string sMessage, string sSeverity)
        {
            ApplicationLog.LogMessage(sMessage, sSeverity);
            if (OnMessageLogged != null)
                OnMessageLogged();
        }

        /// <summary>
        /// Log a message with black color
        /// </summary>
        /// <param name="sMessage"></param>
        public static void LogInfo(string sMessage)
        {
            LogMessage(sMessage, OmgUtils.Logging.DefaultSeverityLevels.SL_Info);
        }

        /// <summary>
        /// Log a message with Goldenrod color
        /// </summary>
        /// <param name="sMessage"></param>
        public static void LogWarning(string sMessage)
        {
            LogMessage(sMessage, OmgUtils.Logging.DefaultSeverityLevels.SL_Warning);
        }

        /// <summary>
        /// Log a message with dark red color
        /// </summary>
        /// <param name="sMessage"></param>
        public static void LogError(string sMessage)
        {
            LogMessage(sMessage, OmgUtils.Logging.DefaultSeverityLevels.SL_Error);
        }

        /// <summary>
        /// Displays a message box, telling the user that a feature has not been implemented yet.
        /// Logs the same message to the application log.
        /// Appends sMessage to the end of the text.
        /// </summary>
        /// <param name="sMessage"></param>
        public static void NotifyNotImplemented(string sMessage)
        {
            string sFinalMessage = Properties.Resources.MsgFeatureNotImplemented + "\n" +
                Properties.Resources.MsgAdditionalInfo + ": " + sMessage;

            Log.LogWarning("User requested a missing feature: " + sMessage);

            MessageBox.Show(sFinalMessage, Properties.Resources.CommonWarning,
                             MessageBoxButton.OK, MessageBoxImage.Warning);  
        }
    }
}
