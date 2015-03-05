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

        public virtual bool IsValid(object o, out ReasonList reasons)
        {
            reasons = new ReasonList();
            return true;
        }

        public bool IsValid(object o)
        {
            ReasonList list;

            return IsValid(o, out list);
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
