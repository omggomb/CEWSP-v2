using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.ValidationRules
{
    class PorjectNameValidationRule : BaseValidationRule
    {
        string m_sNameToCheck;

        public override bool IsValid(object o, ref ReasonList reasons, params object[] additionalArgs)
        {
            base.IsValid(o, ref reasons, additionalArgs);

            if (o is string == false)
                throw new ArgumentException("o is not a project name");

            m_sNameToCheck = o as string;

            CheckIsEmpty();

            CheckAlreadyExists();

            reasons = m_reasons;

            return !m_reasons.ContainsError;
        }

        private void CheckIsEmpty()
        {
            if (string.IsNullOrWhiteSpace(m_sNameToCheck))
            {
                var r = new Reason()
                {
                    HumanReadableExplanation = Properties.ValidationReasons.ProjectNameEmpty,
                    Severity = EReasonSeverity.eRS_error,
                    ReasonType = EReason.eR_ProjectNameEmpty
                };
                m_reasons.Add(r);
            }
        }

        private void CheckAlreadyExists()
        {
            if (Backend.ApplicationBackend.FoundProjectsNames.Contains(m_sNameToCheck))
            {
                var r = new Reason()
                {
                    HumanReadableExplanation = Properties.ValidationReasons.ProjectNameExists,
                    Severity = EReasonSeverity.eRS_error,
                    ReasonType = EReason.eR_ProjectNameExists
                };
                m_reasons.Add(r);
            }
        }
    }
}
