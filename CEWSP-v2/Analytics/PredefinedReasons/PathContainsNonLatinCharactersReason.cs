using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.PredefinedReasons
{
    public class PathContainsNonLatinCharactersReason : Reason
    {
        public PathContainsNonLatinCharactersReason()
            : base()
        {
            ReasonType = EReason.eR_PathContainsNonLatinCharacters;
            Severity = EReasonSeverity.eRS_warning;
            HumanReadableExplanation = Properties.ValidationReasons.PathContainsNonLatinCharacters;

            HumanReadableSolutions.Add(Properties.ValidationReasons.SolPathContainsNonLatinCharacters);     
        }
    }
}
