﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udaan16.Common
{
    public class Devs
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Git { get; set; }
        public string Contact { get; set; }

        public Devs(string n, string e, string g, string c)
        {
            Name = n;
            Email = e;
            Git = g;
            Contact = c;
        }
    }
}
