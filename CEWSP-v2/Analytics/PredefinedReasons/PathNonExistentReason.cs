using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.PredefinedReasons
{
    public class PathNonExistentReason : Reason
    {
        public PathNonExistentReason() : base()
        {
            ReasonType = EReason.eR_PathDoesNotExist;
            Severity = EReasonSeverity.eRS_warning;
            HumanReadableExplanation = Properties.ValidationReasons.PathDoesNotExist;

            HumanReadableSolutions.Add(Properties.ValidationReasons.SolPathDoesNotExistCreate);
            HumanReadableSolutions.Add(Properties.ValidationReasons.SolPathDoesNotExistChooseValid);
        }
    }
}
