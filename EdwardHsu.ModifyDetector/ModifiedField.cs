using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    /// <summary>
    /// Modified Field
    /// </summary>
    public class ModifiedField: ModifiedMember
    {
        public new FieldInfo Member { get; internal set; }

        public ModifiedField(FieldInfo member) : base(member)
        {
            Member = member;
            Children = null;
        }
    }
}
