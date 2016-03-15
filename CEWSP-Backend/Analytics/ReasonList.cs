using System.Collections.Generic;

namespace CEWSP_Backend.Analytics
{
    /// <summary>
    /// A list of reason that provides additional functionality
    /// </summary>
    public class ReasonList : List<Reason>
    {
        private int m_nAddedIssuesCount;

        /// <summary>
        /// Does this list contain one or more reasons that have a severity of "error"?
        /// </summary>
        public bool ContainsError
        {
            get;
            private set;
        }

        public new void Add(Reason r)
        {
            if (r.Severity == EReasonSeverity.eRS_error)
                ContainsError = true;

            ++m_nAddedIssuesCount;
            r.ID = m_nAddedIssuesCount;

            base.Add(r);
        }
    }
}