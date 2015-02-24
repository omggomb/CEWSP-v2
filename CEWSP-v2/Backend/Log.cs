using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CEWSP_v2.Backend
{
    /// <summary>
    /// The logging interface for CEWSP
    /// </summary>
    public class Log
    {
        /// <summary>
        /// An instance of OmgUtils.Logging.Logger
        /// </summary>
        private static OmgUtils.Logging.Logger ApplicationLog { get; set; }

        
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

            if (!ApplicationLog.Init(ConstantDefintitions.FullLogFilePath, tb))
                return false;

            ApplicationLog.LogInfo("Log successfully initialized...");
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
        /// Log a message with Darkgolderod color
        /// </summary>
        /// <param name="sMessage"></param>
        public static void LogWarning(string sMessage)
        {
            LogMessage(sMessage, OmgUtils.Logging.DefaultSeverityLevels.SL_Warning);
        }

        /// <summary>
        /// Log a message with darkred color
        /// </summary>
        /// <param name="sMessage"></param>
        public static void LogError(string sMessage)
        {
            LogMessage(sMessage, OmgUtils.Logging.DefaultSeverityLevels.SL_Error);
        }
    }
}
