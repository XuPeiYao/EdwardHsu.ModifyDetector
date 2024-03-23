using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    public class ModifiedProperty: ModifiedMember
    {
        public new PropertyInfo Member { get; set; }
        public IList<ModifiedMember> Children { get; set; }

        public ModifiedProperty(PropertyInfo member):base(member)
        {
        }
    }
}
