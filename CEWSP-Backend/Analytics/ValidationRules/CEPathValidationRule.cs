using System;
using System.Collections.Generic;

using System.IO;

namespace CEWSP_Backend.Analytics.ValidationRules
{
    /// <summary>
    /// Checks whether the given path is valid for use with CE
    /// </summary>
    public class CEPathValidationRule : BaseValidationRule
    {
        /// <summary>
        /// Should this instance take into account wether the path actually exists?
        /// If set to false, no issue will be given regardless of whether the path exists or not
        /// </summary>
        private bool m_bShouldCheckIfExists;

        /// <summary>
        /// Determines whether non existing paths are treated as error or warning
        /// Ineffect if m_bShouldCheckIfExists is false
        /// </summary>
        private bool m_bNonExistingPathIsError;

        private string m_sPathToCheck;

        public CEPathValidationRule(bool bCheckIfExists, bool bNonExistingPathIsError = false)
        {
            m_bShouldCheckIfExists = bCheckIfExists;
            m_bNonExistingPathIsError = bNonExistingPathIsError;
        }

        public override bool IsValid(object o, ref ReasonList reasons, params object[] additionalArgs)
        {
            base.IsValid(o, ref reasons, additionalArgs);

            if (!(o is string))
                throw new ArgumentException("CEPathvValidationRule was called without using a string.");

            m_sPathToCheck = o as string;

            if (m_bShouldCheckIfExists)
                DoPathExistenceCheck();

            CheckSpaces();

            CheckCharacters();

            return !m_reasons.ContainsError;
        }

        private void CheckCharacters()
        {
            var exceptions = new List<char>();
            exceptions.Add('\\');
            exceptions.Add('/');
            exceptions.Add('.');
            exceptions.Add('_');
            exceptions.Add('-');
            exceptions.Add(':');

            string sFoundNonLatins = "";
            bool bFoundNonLatins = false;

            for (int i = 0; i < m_sPathToCheck.Length; i++)
            {
                if (exceptions.Contains(m_sPathToCheck[i]))
                    continue;

                char currentChar = m_sPathToCheck[i];
                int charValue = (int)currentChar;

                // It' not a digit...
                if (charValue < 48 || charValue > 57)
                {
                    // It's not a letter A-Z or a-z
                    if ((charValue < 65 || charValue > 90) && (charValue < 97 || charValue > 122))
                    {
                        sFoundNonLatins += currentChar + ", ";

                        if (!bFoundNonLatins)
                            bFoundNonLatins = true;
                    }
                }
            }

            if (bFoundNonLatins)
            {
                var reason = new PredefinedReasons.PathContainsNonLatinCharactersReason();
                reason.HumanReadableExplanation += "( " + sFoundNonLatins + ")";

                m_reasons.Add(reason);
            }
        }

        private void CheckSpaces()
        {
            for (int i = 0; i < m_sPathToCheck.Length; i++)
            {
                if (Char.IsWhiteSpace(m_sPathToCheck[i]))
                {
                    var reason = new PredefinedReasons.PathContainsSpacesReasong();
                    m_reasons.Add(reason);

                    break;
                }
            }
        }

        private void DoPathExistenceCheck()
        {
            if (!Directory.Exists(m_sPathToCheck) && !File.Exists(m_sPathToCheck))
            {
                var reason = new PredefinedReasons.PathNonExistentReason();

                if (m_bNonExistingPathIsError)
                    reason.Severity = EReasonSeverity.eRS_error;
                else
                    reason.Severity = EReasonSeverity.eRS_warning;

                m_reasons.Add(reason);
            }
        }
    }
}