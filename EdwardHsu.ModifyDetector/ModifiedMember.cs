using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    public class ModifiedMember
    {
        public ModifiedMemberType Type { get; private set; }

        public MemberInfo Member { get; private set; }

        public int? ElementAt { get; set; }

        public IList<ModifiedMember> Children { get; set; }

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

    public enum ModifiedMemberType
    {
        ObjectMember,
        ArrayElement
    }
}
