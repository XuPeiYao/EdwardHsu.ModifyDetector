using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector.Tests.Models
{
    public class TestNode4: ModifyDetector
    {
        public TestNode4()
        {
            TestID = Guid.NewGuid();
        }

        [ModifyDetectTarget(Order = 0)]
        public Guid TestID { get; set; } 
    }
}
