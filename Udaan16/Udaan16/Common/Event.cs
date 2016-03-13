﻿using System.Collections.Generic;

namespace Udaan16.Common
{
    public class Event
    {
        public string name { get; set; }
        public string Description { get; set; }
        public string NoOfParticipants { get; set; }
        public List<Manager> Managers { get; set; }
        public string Fee { get; set; }

    }
}
