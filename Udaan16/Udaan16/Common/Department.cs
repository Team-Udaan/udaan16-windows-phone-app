using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udaan16.Common
{
    public class Department
    {
        public string Alias { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public List<Event> Events { get; set; }

        public Department(string name, string alias)
        {
            this.Title = name;
            this.Alias = alias;
            this.Image = "/Assets/DepartmentLogos/" + alias + ".png";
            Events = new List<Event>();
        }
    }
}
