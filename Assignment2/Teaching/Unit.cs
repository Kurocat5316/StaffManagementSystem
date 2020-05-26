using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Teaching
{
    public class Unit
    {
        public string code { get; set; }
        public string title { get; set; }
        public Staff coordinator { get; set; }
        public List<UnitClass> unitClass { get; set; }
    }
}
