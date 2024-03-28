using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.ModifyDetector.Tests.Models
{
    public class Student : ModifyDetector
    {
        public Student()
        {
        }

        [ModifyDetectTarget(Order = 0)]
        public int Id { get; set; }

        [ModifyDetectTarget(Order = 1)]
        public FullName Name { get; set; }
    }

    public class FullName : ModifyDetector
    {
        public FullName()
        {
        }

        [ModifyDetectTarget(Order = 0)]
        public string FirstName { get; set; }

        [ModifyDetectTarget(Order = 1)]
        public string LastName { get; set; }
    }
}
