using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.ValidationRules
{
    public class CERootPathValidationRule : BaseValidationRule
    {
        
        string m_sPathToCheck;

        Dictionary<string, bool> m_requiredFolders;

        public CERootPathValidationRule()
        {
            m_requiredFolders = new Dictionary<string, bool>();

            m_requiredFolders.Add("Bin32", false);
            m_requiredFolders.Add("Bin64", false);
            m_requiredFolders.Add("Code", false);
        }

        public override bool IsValid(object o, out ReasonList reasons)
        {
            if (o == null)
                throw new ArgumentNullException("o");

            if (!(o is string))
                throw new ArgumentException("CERootPathValidationRule was called without using a string.");

            m_sPathToCheck = o as string;

            var pathValidationRule = new CEPathValidationRule(true, true);
            pathValidationRule.IsValid(m_sPathToCheck, out reasons);
            m_reasons = reasons;

            // If the path itself is faulty we cannot check the contents
            if (!m_reasons.ContainsError)
                CheckFolders();

            return m_reasons.Count == 0 ? true : false;
        }

        private void CheckFolders()
        {
            var dirInf = new DirectoryInfo(m_sPathToCheck);

            // At this point the directory must exist as this method wouldn't have been called otherwise
            foreach (var dir in dirInf.GetDirectories())
            {
                if (m_requiredFolders.ContainsKey(dir.Name))
                    m_requiredFolders[dir.Name] = true;
            }

            if (m_requiredFolders.ContainsValue(false))
            {
                string sMissingFolders = "";

                foreach (var pair in m_requiredFolders)
                {
                    if (pair.Value == false)
                        sMissingFolders += pair.Key + ", ";
                }

                var reason = new Reason()
                {
                    ReasonType = EReason.eR_PathIsMissingFolderToBeRoot,
                    Severity = EReasonSeverity.eRS_error,
                    HumanReadableExplanation = Properties.ValidationReasons.CERootIsMissingFolders,
                };

                reason.HumanReadableSolutions.Add(Properties.ValidationReasons.SolCERootIsMissingFolders);

                m_reasons.Add(reason);
            }
        }
    }
}
