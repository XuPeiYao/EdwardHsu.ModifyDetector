using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field )]
    public class ModifyDetectTargetAttribute:Attribute
    {
        public int Order { get; set; } = int.MaxValue;
    }
}
