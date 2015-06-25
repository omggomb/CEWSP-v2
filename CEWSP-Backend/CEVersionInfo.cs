using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_Backend
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
        public CEVersionInfo()
        {
        }

        public void FromString(string versionString)
        {
            if (versionString == null)
                throw new ArgumentNullException(versionString);

            for (int i = 0; i < versionString.Length; i++)
            {
                if (!Char.IsDigit(versionString[i]) && !(versionString[i] == '.'))
                {
                    throw new ArgumentException("Unexpected character found in CE version string '" 
                                                + versionString[i] + "'");
                }
            }

            var split = versionString.Split('.');

            if (split.Count() != 3)
            {
                throw new ArgumentException("Got more or less than three digits while parsing CE"  + 
                                            "version string (expected e.g. 3.4.5");
            }

            this.MajorVersion = int.Parse(split[0]);
            this.MinorVersion = int.Parse(split[1]);
            this.SubVersion = int.Parse(split[2]);
        }

        public void FromFile(string sExecPath)
        {
            if (sExecPath == null)
            {
                throw new ArgumentNullException("sExecPath");
            }

            this.CheckPath(sExecPath);

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

        public static bool operator ==(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            if ((inf1.MajorVersion == inf2.MajorVersion) &&
                (inf1.MinorVersion == inf2.MinorVersion) &&
                (inf1.SubVersion == inf2.SubVersion))
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            if ((inf1.MajorVersion != inf2.MajorVersion) ||
                 (inf1.MinorVersion != inf2.MinorVersion) ||
                 (inf1.SubVersion != inf2.SubVersion))
            {
                return true;
            }

            return false;
        }

        private void CheckPath(string sExecPath)
        {
            var invalidCharacters = Path.GetInvalidPathChars();
            foreach (char c in sExecPath)
            {
                if (invalidCharacters.Contains(c))
                {
                    throw new ArgumentException("Path contains invalid characters", "sExecPath");
                }
            }
        }
    }
}
