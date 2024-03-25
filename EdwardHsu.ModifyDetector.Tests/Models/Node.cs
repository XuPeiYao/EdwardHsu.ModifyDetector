using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector.Tests.Models
{
    public class Node: ModifyDetector
    {
        public Node()
        {
            Id = Guid.NewGuid();
            Name = "Untitled";
            Description = "Untitled";
            Children = new List<Node>();

            UpdateDetectorState();
        }

        [ModifyDetectTarget(Order = 0)]
        public Guid Id { get; set; } 

        [ModifyDetectTarget(Order = 1)]
        public string Name { get; set; }

        [ModifyDetectTarget(Order = 2)]
        public string Description { get; internal set; }

        [ModifyDetectTarget(Order = 3)]
        public Node Parent { get; set; }

        [ModifyDetectTarget(Order = 4)]
        public IList<Node> Children { get; set; }
        
    }
}
