using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector.Tests.Models
{
    public class TestNode1: ModifyDetector
    {
        public TestNode1()
        {
            Id = Guid.NewGuid();
            Name = "Untitled";
            Description = "Untitled";
            Children = new List<TestNode1>();

            UpdateDetectorState();
        }

        [ModifyDetectTarget(Order = 0)]
        public Guid Id { get; set; } 

        [ModifyDetectTarget(Order = 1)]
        public string Name { get; set; }

        [ModifyDetectTarget(Order = 2)]
        public string Description { get; set; }

        [ModifyDetectTarget(Order = 3)]
        public TestNode1 Parent { get; set; }

        [ModifyDetectTarget(Order = 4)]
        public IList<TestNode1> Children { get; set; }
        
    }
}
