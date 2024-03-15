using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore实体配置DataAnnotations
{
    [Table("T_Teacher")]
    public class Teacher
    {
        public Guid id { get; set; }
        [MaxLength(40)]
        public string name { get; set; }
        [DataType("int")]
        public double salary { get; set; }
        [Column("inputDate")]
        public DateTime indate { get; set; }
    }
}
