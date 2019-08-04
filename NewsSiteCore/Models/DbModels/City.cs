using Models.HelperModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DbModels
{
    [Table("Cities")]
    public class City : DataEntity
    {
        public string Name { get; set; }
        public int Code { get; set; }
    }
}
