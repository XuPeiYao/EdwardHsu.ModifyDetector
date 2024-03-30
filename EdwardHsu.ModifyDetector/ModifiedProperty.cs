using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    /// <summary>
    /// Modified Property 
    /// </summary>
    public class ModifiedProperty: ModifiedMember
    {
        public new PropertyInfo Member { get; internal set; }

        public ModifiedProperty(PropertyInfo member): base(member)
        {
            Member = member;
            Children = null;
        }
    }
}
