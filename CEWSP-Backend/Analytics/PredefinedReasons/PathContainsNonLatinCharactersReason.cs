namespace CEWSP_Backend.Analytics.PredefinedReasons
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