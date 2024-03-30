using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector
{
    /// <summary>
    /// Attribute for modify detect target
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field )]
    public class ModifyDetectTargetAttribute:Attribute
    {
        /// <summary>
        /// Order of the target
        /// </summary>
        public int Order { get; set; } = int.MaxValue;
    }
}
