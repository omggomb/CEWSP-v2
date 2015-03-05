using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.PredefinedReasons
{
    public class PathContainsSpacesReasong : Reason
    {
        public PathContainsSpacesReasong()
            : base()
        {
            ReasonType = EReason.eR_PathContainsWhitespaces;
            Severity = EReasonSeverity.eRS_warning;
            HumanReadableExplanation = Properties.ValidationReasons.PathContainsSpaces;

            HumanReadableSolutions.Add(Properties.ValidationReasons.SolPathContainsSpaces);     
        }
    }
}
