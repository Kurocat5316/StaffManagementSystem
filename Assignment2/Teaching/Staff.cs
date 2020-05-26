using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment2.Teaching;

namespace Assignment2.Teaching
{
    public class Staff
    {
        public int Id { get; set; }
        public string familyName { get; set; }
        public string giveName { get; set; }
        public string title { get; set; }
        public Type.Campus campus { get; set; }
        public string phone { get; set; }
        public string room { get; set; }
        public string email { get; set; }
        public Uri photo { get; set; }
        public Type.Category category { get; set; }
        public List<Unit> units { get; set; }
        public List<Event> classTime { get; set; }
        public List<Event> consult { get; set; }
    }
}
