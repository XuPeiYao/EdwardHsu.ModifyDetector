using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    /// <summary>
    /// Modified Member
    /// </summary>
    public class ModifiedMember
    {
        /// <summary>
        /// Modified member type
        /// </summary>
        public ModifiedMemberType Type { get; private set; }
        
        /// <summary>
        /// Member info
        /// </summary>
        public MemberInfo Member { get; private set; }

        /// <summary>
        /// Element at index
        /// </summary>
        public int? ElementAt { get; set; }

        /// <summary>
        /// Children
        /// </summary>
        public IList<ModifiedMember> Children { get; internal set; }

        public ModifiedMember(MemberInfo member)
        {
            Member = member;
            Type = ModifiedMemberType.ObjectMember;
        }

        public ModifiedMember(int elementAt)
        {
            ElementAt = elementAt;
            Type = ModifiedMemberType.ArrayElement;
        }
    }

    /// <summary>
    /// Modified member type
    /// </summary>
    public enum ModifiedMemberType
    {
        ObjectMember,
        ArrayElement
    }
}
