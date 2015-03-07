using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics.ValidationRules
{
    /// <summary>
    /// The rule the analyser uses to check whether the given object is valid 
    /// </summary>
    interface IValidationRule
    {
        /// <summary>
        /// Determines whether the given objects is valid
        /// </summary>
        /// <param name="o">Object to be checked</param>
        /// <param name="reasons">List of reasons why the object is not valid, or zero entries if it is valid</param>
        /// <returns>True if valid, else false</returns>
        bool IsValid(object o, ref ReasonList reasons, params object[] additionalArgs);

        /// <summary>
        /// Determines whether the object is valid or not
        /// </summary>
        /// <param name="o">Object to be checked</param>
        /// <returns>True if valid, else false</returns>
        bool IsValid(object o, params object[] additionalArgs);
    }
}
