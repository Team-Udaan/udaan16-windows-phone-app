using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udaan16.Common
{
    public class Event
    {
        //[JsonProperty("name")]
        public string name { get; set; }
        //[JsonProperty("description")]
        public string Description { get; set; }
        //[JsonProperty("numberOfParticipants")]
        public string NoOfParticipants { get; set; }
        //[JsonProperty("prize")]
        public string prize { get; set; }
        //[JsonProperty("department")]
        public string Dept { get; set; }
        //[JsonProperty("manager")]
        public List<Manager> Managers { get; set; }
        //[JsonProperty("fees")]
        public string Fee { get; set; }

    }
}
