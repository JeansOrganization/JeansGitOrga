﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guid主键
{
    [Table("T_Student")]
    public class Student
    {
        public long id { get; set; }

        public string name { get; set; }

        public DateTime indate { get; set; }
    }
}
