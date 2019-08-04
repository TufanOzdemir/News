using Models.HelperModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DbModels
{
    [Table("Posts")]
    public class Post : DataEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsLastMinute { get; set; }
        public string UserId { get; set; }
        public int CityId { get; set; }
    }
}
