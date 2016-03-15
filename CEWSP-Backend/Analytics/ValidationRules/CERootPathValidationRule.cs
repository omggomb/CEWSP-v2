using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEWSP_Backend.Analytics.ValidationRules
{
    public class CERootPathValidationRule : BaseValidationRule
    {
        private string m_sPathToCheck;

        private Dictionary<string, bool> m_requiredFolders;

        private CEVersionInfo m_versionToCheck;

        public CERootPathValidationRule()
        {
            m_requiredFolders = new Dictionary<string, bool>();

            ResetRequiredFolders();
        }

        public override bool IsValid(object o, ref ReasonList reasons, params object[] additionalArgs)
        {
            base.IsValid(o, ref reasons, additionalArgs);

            if (!(o is string))
                throw new ArgumentException("CERootPathValidationRule was called without using a string.");

            if (additionalArgs.Count() != 1)
                throw new ArgumentException("Didn't get version info in additionalArgs");

            if (additionalArgs[0] == null)
                throw new ArgumentNullException("Version info is null");

            if (additionalArgs[0] is CEVersionInfo == false)
                throw new ArgumentException("Did not get object of CEVersion as additionalArgs");

            m_versionToCheck = additionalArgs[0] as CEVersionInfo;

            ResetRequiredFolders();

            m_sPathToCheck = o as string;

            var pathValidationRule = new CEPathValidationRule(true, true);

            pathValidationRule.IsValid(m_sPathToCheck, ref m_reasons);

            // If the path itself is faulty we cannot check the contents
            if (!m_reasons.ContainsError)
                CheckFolders();

            reasons = m_reasons;
            return !m_reasons.ContainsError;
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

        private void ResetRequiredFolders()
        {
            m_requiredFolders.Clear();

            m_requiredFolders.Add("Tools", false);

            if (m_versionToCheck != null && m_versionToCheck.IsEaaS)
            {
                m_requiredFolders.Add("bin", false);
                // Code folder may not be extracted
            }
            else
            {
                m_requiredFolders.Add("Bin64", false);
                m_requiredFolders.Add("Code", false);
            }
        }
    }
}