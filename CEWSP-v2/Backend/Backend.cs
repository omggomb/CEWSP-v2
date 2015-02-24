using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using OmgUtils.Logging;

using System.Runtime.InteropServices;

namespace CEWSP_v2.Backend
{
    /// <summary>
    /// Takes care of all the logic
    /// </summary>
    public class Backend
    {
        /// <summary>
        /// Loads the last known profile, profile history, creates a logging instance
        /// </summary>
        /// <returns>True on succes</returns>
        public bool Init()
        {
            Log.LogInfo("Backend successfully initialized...");
            return true;
        }
    }
}
