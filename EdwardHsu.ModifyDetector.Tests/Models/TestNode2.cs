using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector.Tests.Models
{
    public class TestNode2 : ModifyDetector
    {
        public TestNode2()
        {
            Id = Guid.NewGuid();
            Name = "Untitled";
            Description = "Untitled";
            Children = new List<TestNode2>();

            UpdateDetectorState();
        }

        [ModifyDetectTarget(Order = 0)] 
        public Guid Id;

        [ModifyDetectTarget(Order = 1)] 
        public string Name;

        [ModifyDetectTarget(Order = 2)] 
        public string Description;

        [ModifyDetectTarget(Order = 3)] 
        public TestNode2 Parent;

        [ModifyDetectTarget(Order = 4)] 
        public IList<TestNode2> Children;

    }
}
