using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.ValidationRules
{
    /// <summary>
    /// A basic implementation of the IValidationRule interface.
    /// Always returns true!
    /// </summary>
    public class BaseValidationRule : IValidationRule
    {
        protected ReasonList m_reasons;

        public virtual bool IsValid(object o, ref ReasonList reasons, params object[] additionalArgs)
        {
            if (o == null)
                throw new ArgumentNullException("o");

            if (reasons == null)
                reasons = new ReasonList();

            m_reasons = reasons;

            return true;
        }

        public bool IsValid(object o, params object[] additionalArgs)
        {
            ReasonList list = null;

            return IsValid(o, ref list, additionalArgs);
        }

        public ReasonList Reasons
        {
            get
            {
                return m_reasons;
            }
        }
    }
}
