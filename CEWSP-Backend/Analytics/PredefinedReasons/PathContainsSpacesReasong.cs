namespace CEWSP_Backend.Analytics.PredefinedReasons
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