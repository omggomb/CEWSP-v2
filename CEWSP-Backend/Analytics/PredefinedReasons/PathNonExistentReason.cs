namespace CEWSP_Backend.Analytics.PredefinedReasons
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