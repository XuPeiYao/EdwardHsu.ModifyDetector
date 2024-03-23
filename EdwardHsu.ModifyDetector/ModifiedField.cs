using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    public class ModifiedField:ModifiedMember
    {
        public new FieldInfo Member { get; set; }
        public IList<ModifiedMember> Children { get; set; }

        public ModifiedField(FieldInfo member) : base(member)
        {
        }
    }
}
