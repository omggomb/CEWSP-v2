using CEWSP_Backend.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_Backend.Backend
{
    public struct LYProjectOutData
    {
        public string LYRoot { get; internal set; }

        public string ProjectName { get; internal set; }
    }
    class LYProject
    {
        /// <summary>
        /// The name of this project
        /// </summary>
        public string ProjectName { get; private set; }

        public LYProject(string sProjectName)
        {
            this.ProjectName = sProjectName;
        }

        internal LYProjectOutData GetOutData()
        {
            return new LYProjectOutData()
            {
                ProjectName = this.ProjectName,
                LYRoot = ApplicationBackend.GlobalSettings.GetSetting(SettingsIdentificationNames.SetLYRoot).GetValueAsString(),
            };
        }
    }
}
