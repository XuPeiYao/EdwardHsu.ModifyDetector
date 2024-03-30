using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    /// <summary>
    /// Interface for modify detector
    /// </summary>
    public interface IModifyDetector
    {
        /// <summary>
        /// Update the detector state
        /// </summary>
        void UpdateDetectorState();

        /// <summary>
        /// Check if the object has modified
        /// </summary>
        /// <param name="modifiedMembers">Modified members</param>
        /// <returns>Has modified or not</returns>
        bool HasModified(out IEnumerable<ModifiedMember> modifiedMembers);
    }
}
