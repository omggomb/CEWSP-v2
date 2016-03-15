using System.Collections.Generic;
using System.Diagnostics;

namespace CEWSP_Backend.Analytics.ValidationRules
{
    public class CEEditorPathValidationRule : BaseValidationRule
    {
        private List<string> m_possibleProductNames = new List<string>() { "Editor", "CRYENGINE 3 (R) Sandbox 3 (TM)" };

        public override bool IsValid(object o, ref ReasonList reasons, params object[] additionalArgs)
        {
            if (!base.IsValid(o, ref reasons, additionalArgs))
                return false;

            var pathValidationRule = new CEPathValidationRule(true);

            if (!pathValidationRule.IsValid(o, ref reasons, additionalArgs))
                return false;

            var versionInfo = FileVersionInfo.GetVersionInfo(o as string);

            if (!m_possibleProductNames.Contains(versionInfo.ProductName))
            {
                reasons.Add(new Reason()
                {
                    HumanReadableExplanation = Properties.ValidationReasons.CEEditorPathIsNotEditorExe,
                    Severity = EReasonSeverity.eRS_error,
                    ReasonType = EReason.eR_CEEditorPathIsNotEditorExe
                });

                return false;
            }

            return true;
        }
    }
}