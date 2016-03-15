using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEWSP_Backend
{
    public struct ConfigValueNames
    {
        public const string IconPath = "iconPath";
        public const string DescFilePath = "descFilePath";
        public const string CompatibleVersions = "supportedVersions";
    }

    public class GameTemplate
    {
        private DirectoryInfo m_dirInf;

        public string Name { get; set; }
        public string IconPath { get; set; }
        public string DescFilePath { get; set; }
        public List<CEVersionInfo> SupportedVersions { get; set; }
        public List<string> UnknownConfigNames { get; set; }

        public DirectoryInfo Directory
        {
            get
            {
                return m_dirInf;
            }
        }

        public GameTemplate()
        {
            SupportedVersions = new List<CEVersionInfo>();
            this.UnknownConfigNames = new List<string>();
        }

        public bool LoadFromFolder(string sPath)
        {
            m_dirInf = new DirectoryInfo(sPath);

            if (!m_dirInf.Exists)
            {
                throw new ArgumentException(String.Format("Tried to load game template {0}, but the folder does not exist.", m_dirInf.Name));
            }

            Name = m_dirInf.Name;
            return LoadConfig();
        }

        private bool LoadConfig()
        {
            var configFile = new FileInfo(m_dirInf.FullName + "\\templateConfig.cfg");

            if (!configFile.Exists)
            {
                throw new ArgumentException(String.Format("Tried to load game template {0}, but there was no config file.", m_dirInf.Name));
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
                        var version = new CEVersionInfo();
                        version.FromString(versions[i]);
                        SupportedVersions.Add(version);
                    }
                }
                else
                {
                    this.UnknownConfigNames.Add(splitLine[0]);
                }
            }

            return true;
        }
    }
}