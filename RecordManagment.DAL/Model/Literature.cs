﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Model
{
    public class Literature :BaseEntity
    {
        public String? Name { get; set; }
        public String? Isbn { get; set; }
        public int LendTimeInDays { get; set; }
    }
}
