using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.Analytics
{

    /// <summary>
    /// A list of all reasons
    /// </summary>
    public enum EReason
    {
        /// <summary>
        /// The specified path does neither exist as file nor as directory
        /// </summary>
        eR_PathDoesNotExist,
        /// <summary>
        /// CE does not handle whitespaces well
        /// </summary>
        eR_PathContainsWhitespaces,
        /// <summary>
        /// CE does not handle anything else that standard ASCII characters well
        /// </summary>
        eR_PathContainsNonLatinCharacters,
        /// <summary>
        /// The supposed CE root path does not contain any of the expected folders (Bin32, Bin64, Code)
        /// </summary>
        eR_PathIsMissingFolderToBeRoot,
        /// <summary>
        /// When checking the game folder the given root folder was invalid
        /// </summary>
        eR_CheckGameRootInvalid,
        /// <summary>
        /// The given name is a path, only a valid name is allowed
        /// </summary>
        eR_CheckGameNameIsPath,
        eR_CheckGamePathNotExist,
        eR_CheckGameNameEmpty,
        eR_ProjectNameExists,
        eR_ProjectNameEmpty
    }

    /// <summary>
    /// Indicates whether the issue needs to be fixed
    /// </summary>
    public enum EReasonSeverity
    {
        /// <summary>
        /// A warning does not hinder the work of CEWSP and can be ignored if desired
        /// </summary>
        eRS_warning,
        /// <summary>
        /// An error must be fixed or else CEWSP will cease working
        /// </summary>
        eRS_error
    }

    /// <summary>
    /// A reason, why a specific value is not valid
    /// </summary>
    public class Reason
    {
        public Reason()
        {
            Severity = EReasonSeverity.eRS_warning;
            ReasonType = EReason.eR_PathDoesNotExist;
            HumanReadableExplanation = "No explanation found.";
            HumanReadableSolutions = new List<string>();

            m_nReasonID = -1;
            m_bIsIDSet = false;
        }

        /// <summary>
        /// ID of this reason
        /// </summary>
        int m_nReasonID;

        /// <summary>
        /// Has the ID been set already
        /// </summary>
        bool m_bIsIDSet;

        /// <summary>
        /// Severity of this issue
        /// </summary>
        public EReasonSeverity Severity { get; set; }

        /// <summary>
        /// Internal type of this reason
        /// </summary>
        public EReason ReasonType { get; set; }

        /// <summary>
        /// A brief explanation of the issue
        /// </summary>
        public string HumanReadableExplanation { get; set; }

        /// <summary>
        /// A list of solutions to this issue
        /// </summary>
        public List<string> HumanReadableSolutions { get; set; }


        /// <summary>
        /// This id is set by the ReasonList, thus it is only different within that list.
        /// Can only be set once.
        /// </summary>
        public int ID
        {
            get
            {
                return m_nReasonID;
            }
            set
            {
                if (!m_bIsIDSet)
                {
                    m_nReasonID = value;
                    m_bIsIDSet = true;
                }
            }
        }
    }
}
