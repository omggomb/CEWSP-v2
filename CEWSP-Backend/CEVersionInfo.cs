using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CEWSP_Backend
{
    /// <summary>
    /// Provide information about the given CE version
    /// </summary>
    public class CEVersionInfo
    {
        private static CEVersionInfo s_eaasMinVersion;

        /// <summary>
        /// Constructs a new version info object for the given path
        /// </summary>
        public CEVersionInfo()
        {
        }

        /// <summary>
        /// Path to the 64bit executable of CE
        /// </summary>
        public string ExecPath { get; private set; }

        /// <summary>
        /// Is this version an EaaS version?
        /// </summary>
        public bool IsEaaS
        {
            get
            {
                if (s_eaasMinVersion == null)
                {
                    s_eaasMinVersion = new CEVersionInfo();
                    s_eaasMinVersion.FromString("3.6.10");
                }

                return (this >= s_eaasMinVersion);
            }
        }

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

        public void FromFile(string sExecPath)
        {
            if (sExecPath == null)
            {
                throw new ArgumentNullException("sExecPath");
            }

            CheckPath(sExecPath);

            var info = new FileInfo(sExecPath);

            if (!info.Exists)
            {
                throw new ArgumentException("Path is valid but file doesn't exist", "sExecPath");
            }

            ExecPath = sExecPath;

            var versionInfo = FileVersionInfo.GetVersionInfo(info.FullName);

            MajorVersion = versionInfo.FileMajorPart;
            MinorVersion = versionInfo.FileMinorPart;
            SubVersion = versionInfo.FileBuildPart;
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
                throw new ArgumentException("Got more or less than three digits while parsing CE" +
                                            "version string (expected e.g. 3.4.5");
            }

            MajorVersion = int.Parse(split[0]);
            MinorVersion = int.Parse(split[1]);
            SubVersion = int.Parse(split[2]);
        }

        #region Operators

        public static bool operator !=(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            return !(inf1 == inf2);
        }

        public static bool operator <(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            if (inf1 == inf2)
            {
                return false;
            }

            if (inf1.MajorVersion == inf2.MajorVersion)
            {
                if (inf1.MinorVersion == inf2.MinorVersion)
                {
                    return inf1.SubVersion < inf2.SubVersion;
                }
                else
                {
                    return inf1.MinorVersion < inf2.MinorVersion;
                }
            }
            else
            {
                return inf1.MajorVersion < inf2.MajorVersion;
            }
        }

        public static bool operator <=(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            if (inf1 == inf2)
                return true;

            return inf1 < inf2;
        }

        public static bool operator ==(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            if (System.Object.ReferenceEquals(inf1, inf2))
            {
                return true;
            }

            if (Object.Equals(inf1, null) && !Object.Equals(inf2, null))
                return false;
            if (Object.Equals(inf2, null) && !Object.Equals(inf1, null))
                return false;

            if ((inf1.MajorVersion == inf2.MajorVersion) &&
                (inf1.MinorVersion == inf2.MinorVersion) &&
                (inf1.SubVersion == inf2.SubVersion))
            {
                return true;
            }

            return false;
        }

        public static bool operator >(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            if (inf1 == inf2)
            {
                return false;
            }

            if (inf1.MajorVersion == inf2.MajorVersion)
            {
                if (inf1.MinorVersion == inf2.MinorVersion)
                {
                    return inf1.SubVersion > inf2.SubVersion;
                }
                else
                {
                    return inf1.MinorVersion > inf2.MinorVersion;
                }
            }
            else
            {
                return inf1.MajorVersion > inf2.MajorVersion;
            }
        }

        public static bool operator >=(CEVersionInfo inf1, CEVersionInfo inf2)
        {
            if (inf1 == inf2)
                return true;

            return inf1 > inf2;
        }

        #endregion Operators

        public override string ToString()
        {
            return MajorVersion + "." + MinorVersion + "." + SubVersion;
        }

        public override bool Equals(object obj)
        {
            if (obj is CEVersionInfo)
            {
                return this == (obj as CEVersionInfo);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int code = 38 + MajorVersion.GetHashCode();
            code = 38 * code + MinorVersion.GetHashCode();
            code = 38 * code + SubVersion.GetHashCode();
            return code;
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