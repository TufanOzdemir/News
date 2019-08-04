using Models.HelperModels;
using Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DbModels
{
    [Table("Notifications")]
    public class Notification : DataEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
