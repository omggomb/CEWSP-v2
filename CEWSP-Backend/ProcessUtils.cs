using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_Backend
{
    static class ProcessUtils
    {
        /// <summary>
        /// Creates a new process object and invokes Start() on it. Writes the path and arguments to the log.
        /// </summary>
        /// <param name="path">Path to the process</param>
        /// <param name="arguments">Optional parameters</param>
        /// <returns>The newly created process object, with Start() already having been invoked</returns>
        public static Process Start(string path, string arguments = "")
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = path,
                Arguments = arguments
            };

            Start(process);

            return process;
        }

        /// <summary>
        /// Invokes Start() on the given process p, writes the path and arguments to the console
        /// </summary>
        /// <param name="p">The process to be started</param>
        /// <returns>Result of p.Start()</returns>
        public static bool Start(Process p)
        {
            Log.LogInfo(string.Format(@"Starting process: ""{0}"" with arguments: ""{1}""", p.StartInfo.FileName, p.StartInfo.Arguments));
            return p.Start();
        }
    }
}
