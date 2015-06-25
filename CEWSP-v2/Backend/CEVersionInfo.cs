using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Backend
{
    /// <summary>
    /// Provide information about the given CE version
    /// </summary>
    public class CEVersionInfo
    {
        /// <summary>
        /// Path to the 64bit executable of CE
        /// </summary>
        public string ExecPath { get; private set; }

        /// <summary>
        /// The major version number of this CE (3.8.1 yields 3)
        /// </summary>
        public int MajorVersion { get; private set; }

        /// <summary>
        /// The minor version number of this CE (3.8.1 yields 8)
        /// </summary>
        public int MinorVersion { get; private set; }

        /// <summary>
        /// The subversion number of this CE (3.8.1 yields 1)
        /// </summary>
        public int SubVersion { get; private set; }

        /// <summary>
        /// Constructs a new version info object for the given path
        /// </summary>
        /// <param name="sExecPath">Path to the 64bit editor executable</param>
        public CEVersionInfo(string sExecPath)
        {
            var info = new FileInfo(sExecPath);

            if (!info.Exists)
            {
                throw new ArgumentException("Path is valid but file doesn't exist", "sExecPath");
            }

            this.ExecPath = sExecPath;

            var versionInfo = FileVersionInfo.GetVersionInfo(info.FullName);

            this.MajorVersion = versionInfo.FileMajorPart;
            this.MinorVersion = versionInfo.FileMinorPart;
            this.SubVersion = versionInfo.FileBuildPart;
        }
    }
}
