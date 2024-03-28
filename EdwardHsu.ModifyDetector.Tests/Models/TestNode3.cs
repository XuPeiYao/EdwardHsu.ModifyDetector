using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector.Tests.Models
{
    public class TestNode3: ModifyDetector
    {
        public TestNode3()
        {
            Id = Guid.NewGuid();
            Name = "Untitled";
            Description = "Untitled";
        }

        [ModifyDetectTarget(Order = 0)]
        public Guid Id { get; set; } 

        [ModifyDetectTarget(Order = 1)]
        public string Name { get; set; }

        [ModifyDetectTarget(Order = 2)]
        public string Description { get; set; }
    }
}
