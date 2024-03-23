using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    public interface IModifyDetector
    {
        void UpdateDetectorState();
        bool HasModified(out IEnumerable<ModifiedMember> modifiedMembers);
    }
}
