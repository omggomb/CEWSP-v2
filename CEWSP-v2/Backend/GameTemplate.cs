using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Backend
{
    public struct CEVersion
    {
        /// <summary>
        /// First digit _3_.4.5
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Second digit 3._4_.5
        /// </summary>
        public int Major { get; set; }
        /// <summary>
        /// Third digit 3.4._5_
        /// </summary>
        public int Minor { get; set; }

        public override bool Equals(object obj)
        {
 	         if (obj is CEVersion)
             {
                 var compareTo = (CEVersion)obj;

                 return Version == compareTo.Version &&
                        Major == compareTo.Major &&
                        Minor == compareTo.Minor;
             }

             return false;
        }

        public void SetFromString(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!Char.IsDigit(s[i]) && !(s[i] == '.'))
                {
                    Log.LogError("Unexpected character found in CE version string '" + s[i] + "'");
                    return;
                }
            }

            var split = s.Split('.');

            if (split.Count() != 3)
            {
                Log.LogError("Got more or less than three digits while parsing CE version string (expected e.g. 3.4.5");
                return;
            }

            Version = int.Parse(split[0]);
            Major = int.Parse(split[1]);
            Minor = int.Parse(split[2]);

        }
    }
    public struct ConfigValueNames
    {
        public const string IconPath = "iconPath";
        public const string DescFilePath = "descFilePath";
        public const string CompatibleVersions = "compatibleVersions";
    }

    public class GameTemplate
    {
        DirectoryInfo m_dirInf;

        public string Name { get; set; }
        public string IconPath { get; set; }
        public string DescFilePath { get; set; }
        public List<CEVersion> SupportedVersions { get; set; }
        public DirectoryInfo Directory 
        {
            get
            {
                return m_dirInf;
            }
        }
       
        public GameTemplate()
        {
            SupportedVersions = new List<CEVersion>();
        }

        public bool LoadFromFolder(string sPath)
        {
            m_dirInf = new DirectoryInfo(sPath);

            Log.LogInfo("Loading game template '" + m_dirInf.Name + "'...");

            if (!m_dirInf.Exists)
            {
                Log.LogError(String.Format("Tried to load game template {0}, but the folder does not exist.", m_dirInf.Name));
                return false;
            }

            Name = m_dirInf.Name;
            return LoadConfig();
        }

        private bool LoadConfig()
        {
            var configFile = new FileInfo(m_dirInf.FullName + "\\templateConfig.cfg");

            if (!configFile.Exists)
            {
                Log.LogError(String.Format("Tried to load game template {0}, but there was no config file.", m_dirInf.Name));
                return false;
            }

            var reader = new StreamReader(configFile.OpenRead());

            string sLine = null;

            while (reader.EndOfStream == false) 
            {
                sLine = reader.ReadLine();

                var splitLine = sLine.Split('=');

                if (splitLine[0] == ConfigValueNames.DescFilePath)
                    DescFilePath = splitLine[1];
                else if (splitLine[0] == ConfigValueNames.IconPath)
                    IconPath = splitLine[1];
                else if (splitLine[0] == ConfigValueNames.CompatibleVersions)
                {
                    
                    string sVersionsString = splitLine[1];

                    if (sVersionsString == "all")
                        continue;

                    var versions = sVersionsString.Split(';');

                    for (int i = 0; i < versions.Count(); i++)
                    {
                        var version = new CEVersion();
                        version.SetFromString(versions[i]);
                        SupportedVersions.Add(version);
                    }
                }
                else
                {
                    Log.LogWarning("Found unknown value while reading game template '" + Name + "' config: '" + splitLine[0] + "'");
                }
            }

            return true;

        }
    }
}
