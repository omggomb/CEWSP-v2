using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.ValidationRules
{
    class CEGameValidationRule : BaseValidationRule
    {
        CERootPathValidationRule m_rootPathValidationRule;
        CEPathValidationRule m_pathValidationRule;
        string m_sCERootPath;
        string m_sPathToCheck;

        public CEGameValidationRule()
        {
            m_rootPathValidationRule = new CERootPathValidationRule();
            m_pathValidationRule = new CEPathValidationRule(false);
        }

        /// <summary>
        /// Checks whether the given path is valid
        /// </summary>
        /// <param name="o">The path to check</param>
        /// <param name="reasons">Filled with reason why it is not valid</param>
        /// <param name="additionalArgs">Expects one parameter of type string which is the path to the CE root folder</param>
        /// <returns></returns>
        public override bool IsValid(object o, ref ReasonList reasons, params object[] additionalArgs)
        {
            base.IsValid(o, ref reasons, additionalArgs);

            if (o is string == false)
                throw new ArgumentException("o is not a string");

            if (additionalArgs.Count() <= 0)
                throw new ArgumentException("additionalArg does not contain the expected CE root path");

            if (additionalArgs[0] is string == false)
                throw new ArgumentException("additionalArgs element 0 is not a string");

           
            m_sCERootPath = additionalArgs[0] as string;
            m_sPathToCheck = o as string;

            // Check root folder first
            if (!m_rootPathValidationRule.IsValid(m_sCERootPath))
            {
                var r = new Reason()
                {
                    HumanReadableExplanation = Properties.ValidationReasons.CEGameCannotCheckRootInvalid,
                    ReasonType = EReason.eR_CheckGameRootInvalid,
                    Severity = EReasonSeverity.eRS_error
                };

                r.HumanReadableSolutions.Add(Properties.ValidationReasons.SolCEGameCannotCheckRootInvalid);

                m_reasons.Add(r);
            }
            else
             CheckName();

            reasons = m_reasons;
            return !m_reasons.ContainsError;
        }


        private void CheckName()
        {
            //Must not be empty
            if (string.IsNullOrWhiteSpace(m_sPathToCheck))
            {
                m_reasons.Add(new Reason()
                    {
                        HumanReadableExplanation = Properties.ValidationReasons.CEGameNameIsEmpty,
                        Severity = EReasonSeverity.eRS_error,
                        ReasonType = EReason.eR_CheckGameNameEmpty

                    });

                return;
            }
            // The name is valid
            m_pathValidationRule.IsValid(m_sPathToCheck, ref m_reasons);

            if (IsPath())
            {
                var r = new Reason()
                {
                    HumanReadableExplanation = Properties.ValidationReasons.CEGameNameIsPath,
                    Severity = EReasonSeverity.eRS_error,
                    ReasonType = EReason.eR_CheckGameNameIsPath
                };
                r.HumanReadableSolutions.Add(Properties.ValidationReasons.SolGameNameIsPath);
                m_reasons.Add(r);
            }
            else
            {
                string sCorrectedRoot = OmgUtils.Path.PathUtils.CheckFolderPath(m_sCERootPath);
                string sFullGamePath = OmgUtils.Path.PathUtils.CheckFolderPath(sCorrectedRoot += m_sPathToCheck);

                if (!Directory.Exists(sFullGamePath))
                {
                    var r = new Reason()
                    {
                        HumanReadableExplanation = Properties.ValidationReasons.CEGameNotExist,
                        Severity = EReasonSeverity.eRS_warning,
                        ReasonType = EReason.eR_CheckGamePathNotExist
                    };
                    r.HumanReadableSolutions.Add(Properties.ValidationReasons.SolCEGameNotExist);
                    m_reasons.Add(r);
                }
            }
        }

        private bool IsPath()
        {
            var pathIndicators = new List<char>();

            pathIndicators.Add('/');
            pathIndicators.Add('\\');
            pathIndicators.Add(':');

            foreach (var indicator in pathIndicators)
            {
                if (m_sPathToCheck.Contains(indicator))
                    return true;
            }

            return false;
        }
    }
}
