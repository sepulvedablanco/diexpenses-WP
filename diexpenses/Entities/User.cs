﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diexpenses.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string AuthToken { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    }
}